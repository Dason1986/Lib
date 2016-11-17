using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Library.Data
{
    /// <summary>
    /// 操作条件集合
    /// </summary>
    public class QueryConditionCollection : KeyedCollection<string, QueryConditionItem>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public QueryConditionCollection()
            : base()
        {
        }

        /// <summary>
        /// 从指定元素提取键
        /// </summary>
        /// <param name="item">从中提取键的元素</param>
        /// <returns>指定元素的键</returns>
        protected override string GetKeyForItem(QueryConditionItem item)
        {
            return item.Key;
        }

        public Expression<Func<T, bool>> GetExpression<T>()
        {
            if (this.Count() == 0)
            {
                return c => true;
            }

            //构建 c=>Body中的c
            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            //获取最小的判断表达式
            var list = Items.Select(item => GetExpression<T>(param, item));
            //再以逻辑运算符相连
            var body = list.Aggregate(Expression.AndAlso);

            //将二者拼为c=>Body
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private Expression GetExpression<T>(ParameterExpression param, QueryConditionItem item)
        {
            //属性表达式
            LambdaExpression exp = GetPropertyLambdaExpression<T>(item, param);

            //常量表达式
            var constant = ChangeTypeToExpression(item, exp.Body.Type);

            //以判断符或方法连接
            return ExpressionDict[item.Op](exp.Body, constant);
        }

        private LambdaExpression GetPropertyLambdaExpression<T>(QueryConditionItem item, ParameterExpression param)
        {
            //获取每级属性如c.Users.Proiles.UserId
            var props = item.Name.Split('.');

            Expression propertyAccess = param;

            Type typeOfProp = typeof(T);

            int i = 0;
            do
            {
                PropertyInfo property = typeOfProp.GetProperty(props[i]);
                if (property == null) return null;
                typeOfProp = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                i++;
            } while (i < props.Length);

            return Expression.Lambda(propertyAccess, param);
        }

        #region ChangeType
        /// <summary>
        /// 转换SearchItem中的Value的类型，为表达式树
        /// </summary>
        /// <param name="item"></param>
        /// <param name="conversionType">目标类型</param>
        private Expression ChangeTypeToExpression(QueryConditionItem item, Type conversionType)
        {
            if (item.DataValue == null)
                return Expression.Constant(item.DataValue, conversionType);

            #region 数组
            if (item.Op == QueryConditionType.In)
            {
                var arr = (item.DataValue as Array);
                var expList = new List<Expression>();
                //确保可用
                if (arr != null)
                    for (var i = 0; i < arr.Length; i++)
                    {
                        //构造数组的单元Constant
                        var newValue = arr.GetValue(i);
                        expList.Add(Expression.Constant(newValue, conversionType));
                    }

                //构造inType类型的数组表达式树，并为数组赋初值
                return Expression.NewArrayInit(conversionType, expList);
            }
            #endregion

            var value = conversionType.GetTypeInfo().IsEnum ? Enum.Parse(conversionType, (string)item.DataValue)
                : Convert.ChangeType(item.DataValue, conversionType);

            return Expression.Constant(value, conversionType);
        }
        #endregion

        #region SearchMethod 操作方法
        private readonly Dictionary<QueryConditionType, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<QueryConditionType, Func<Expression, Expression, Expression>>
                {
                    {
                        QueryConditionType.Eq,
                        (left, right) => { return Expression.Equal(left, right); }
                        },
                    {
                        QueryConditionType.Gt,
                        (left, right) => { return Expression.GreaterThan(left, right); }
                        },
                    {
                        QueryConditionType.Gte,
                        (left, right) => { return Expression.GreaterThanOrEqual(left, right); }
                        },
                    {
                        QueryConditionType.Lt,
                        (left, right) => { return Expression.LessThan(left, right); }
                        },
                    {
                        QueryConditionType.Lte,
                        (left, right) => { return Expression.LessThanOrEqual(left, right); }
                        },
                    {
                        QueryConditionType.Contains,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("Contains"), right);
                            }
                        },
                    {
                        QueryConditionType.In,
                        (left, right) =>
                            {
                                if (!right.Type.IsArray) return null;
                                //调用Enumerable.Contains扩展方法
                                MethodCallExpression resultExp =
                                    Expression.Call(
                                        typeof (Enumerable),
                                        "Contains",
                                        new[] {left.Type},
                                        right,
                                        left);

                                return resultExp;
                            }
                        },
                    {
                        QueryConditionType.Neq,
                        (left, right) => { return Expression.NotEqual(left, right); }
                        },
                    {
                        QueryConditionType.StartWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("StartsWith", new[] {typeof (string)}), right);

                            }
                        },
                    {
                        QueryConditionType.EndWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("EndsWith", new[] {typeof (string)}), right);
                            }
                     }
                };
        #endregion
    }

    public enum QueryConditionType
    {
        EndWith,
        StartWith,
        Neq,
        Eq,
        Gt,
        Gte,
        Lt,
        Lte,
        Contains,
        In
    }

    /// <summary>
    /// 操作条件
    /// </summary>
    public class QueryConditionItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 条件操作类型
        /// </summary>
        public QueryConditionType Op { get; set; }

        ///// <summary>
        ///// DataValue是否包含单引号，如'DataValue'
        ///// </summary>
        //public bool IsIncludeQuot { get; set; }

        /// <summary>
        /// 数据的值
        /// </summary>
        public object DataValue { get; set; }
    }
    public interface IModelBinder
    {

    }
    public interface IModelBinderProvider
    {

    }
    public class QueryConditionModelBinderPrivdier : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType != typeof(QueryConditionCollection))
            {
                return null;
            }

            return new QueryConditionModelBinder(context.MetadataProvider);
        }
    }
}
