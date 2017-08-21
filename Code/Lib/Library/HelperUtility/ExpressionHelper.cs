using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.HelperUtility
{

    /// <summary>
    ///
    /// </summary>
    public static class ExpressionHelper
    {
        #region True or False

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        #endregion

        #region GetMemberInfo

        /// <summary>
        /// Converts an expression into a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member;

        }
        #endregion
        #region Property(属性表达式)

        /// <summary>
        /// 创建属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="propertyName">属性名,支持多级属性名，与句点分隔，范例：Customer.Name</param>
        public static Expression Property(this Expression expression, string propertyName)
        {
            if (propertyName.All(t => t != '.'))
                return Expression.Property(expression, propertyName);
            var propertyNameList = propertyName.Split('.');
            Expression result = null;
            for (int i = 0; i < propertyNameList.Length; i++)
            {
                if (i == 0)
                {
                    result = Expression.Property(expression, propertyNameList[0]);
                    continue;
                }
                result = result.Property(propertyNameList[i]);
            }
            return result;
        }

        /// <summary>
        /// 创建属性表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="member">属性</param>
        public static Expression Property(this Expression expression, MemberInfo member)
        {
            return Expression.MakeMemberAccess(expression, member);
        }

        #endregion
        #region And(与表达式)

        /// <summary>
        /// 与操作表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression And(this Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            return Expression.And(left, right);
        }

        /// <summary>
        /// 与操作表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var one = parameterReplacer.Replace(left.Body);
            var another = parameterReplacer.Replace(right.Body);

            var body = Expression.And(one, another);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        #endregion

        #region Or(或表达式)

        /// <summary>
        /// 或操作表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression Or(this Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            return Expression.OrElse(left, right);
        }

        /// <summary>
        /// 或操作表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);
            var one = parameterReplacer.Replace(left.Body);
            var another = parameterReplacer.Replace(right.Body);

            var body = Expression.Or(one, another);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        #endregion

        #region NotEqual
        /// <summary>
        /// 或操作表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression NotEqual(this Expression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            return Expression.NotEqual(left, right);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotEqual<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.NotEqual(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);

        }
        #endregion

        #region Constant(获取常量) 


        /// <summary> 
        /// 获取常量表达式 
        /// </summary> 
        /// <param name="expression">表达式</param> 
        /// <param name="value">值</param> 
        public static ConstantExpression Constant(Expression expression, object value)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                return Expression.Constant(value);
            return Expression.Constant(value, memberExpression.Type);
        }


        #endregion

        #region like

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="valueSelector"></param>
        /// <param name="value"></param>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        public static Expression<Func<TElement, bool>> BuildLikeExpression<TElement>(this Expression<Func<TElement, string>> valueSelector, string value, char wildcard)
        {
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            var method = GetLikeMethod(value, wildcard);

            value = value.Trim(wildcard);
            var body = Expression.Call(valueSelector.Body, method, Expression.Constant(value));

            var parameter = valueSelector.Parameters.Single();
            return Expression.Lambda<Func<TElement, bool>>(body, parameter);
        }

        private static MethodInfo GetLikeMethod(string value, char wildcard)
        {
            var methodName = "Contains";

            var textLength = value.Length;
            value = value.TrimEnd(wildcard);
            if (textLength > value.Length)
            {
                methodName = "StartsWith";
                textLength = value.Length;
            }

            value = value.TrimStart(wildcard);
            if (textLength > value.Length)
            {
                methodName = (methodName == "StartsWith") ? "Contains" : "EndsWith";
                textLength = value.Length;
            }

            var stringType = typeof(string);
            return stringType.GetMethod(methodName, new Type[] { stringType });
        }

        #endregion

        #region Greater(大于表达式)

        /// <summary>
        /// 创建大于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression Greater(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }

        /// <summary>
        /// 创建大于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression Greater(this Expression left, object value)
        {
            return left.Greater(Constant(left, value));
        }

        #endregion
        #region GreaterEqual(大于等于表达式)

        /// <summary>
        /// 创建大于等于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression GreaterEqual(this Expression left, Expression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }

        /// <summary>
        /// 创建大于等于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression GreaterEqual(this Expression left, object value)
        {
            return left.GreaterEqual(Constant(left, value));
        }

        #endregion

        #region Less(小于表达式)

        /// <summary>
        /// 创建小于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression Less(this Expression left, Expression right)
        {
            return Expression.LessThan(left, right);
        }

        /// <summary>
        /// 创建小于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression Less(this Expression left, object value)
        {
            return left.Less(Constant(left, value));
        }

        #endregion

        #region LessEqual(小于等于表达式)

        /// <summary>
        /// 创建小于等于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public static Expression LessEqual(this Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }

        /// <summary>
        /// 创建小于等于运算表达式
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression LessEqual(this Expression left, object value)
        {
            return left.LessEqual(Constant(left, value));
        }

        #endregion

        #region StartsWith(头匹配)

        /// <summary>
        /// 头匹配
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression StartsWith(this Expression left, object value)
        {
            return left.Call("StartsWith", new[] { typeof(string) }, value);
        }

        #endregion

        #region EndsWith(尾匹配)

        /// <summary>
        /// 尾匹配
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression EndsWith(this Expression left, object value)
        {
            return left.Call("EndsWith", new[] { typeof(string) }, value);
        }

        #endregion

        #region Contains(模糊匹配)

        /// <summary>
        /// 模糊匹配
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="value">值</param>
        public static Expression Contains(this Expression left, object value)
        {
            return left.Call("Contains", new[] { typeof(string) }, value);
        }

        #endregion
        #region Call(调用方法表达式)

        /// <summary>
        /// 创建调用方法表达式
        /// </summary>
        /// <param name="instance">调用的实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="values">参数值列表</param>
        internal static Expression Call(this Expression instance, string methodName, params Expression[] values)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), values);
        }

        /// <summary>
        /// 创建调用方法表达式
        /// </summary>
        /// <param name="instance">调用的实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="values">参数值列表</param>
        internal static Expression Call(this Expression instance, string methodName, params object[] values)
        {
            if (values == null || values.Length == 0)
                return Expression.Call(instance, instance.Type.GetMethod(methodName));
            return Expression.Call(instance, instance.Type.GetMethod(methodName), values.Select(Expression.Constant));
        }

        /// <summary>
        /// 创建调用方法表达式
        /// </summary>
        /// <param name="instance">调用的实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paramTypes">参数类型列表</param>
        /// <param name="values">参数值列表</param>
        internal static Expression Call(this Expression instance, string methodName, Type[] paramTypes, params object[] values)
        {
            if (values == null || values.Length == 0)
                return Expression.Call(instance, instance.Type.GetMethod(methodName, paramTypes));
            return Expression.Call(instance, instance.Type.GetMethod(methodName, paramTypes), values.Select(Expression.Constant));
        }

        #endregion

        #region GetConditionCount(获取查询条件个数) 


        /// <summary> 
        /// 获取查询条件个数 
        /// </summary> 
        /// <param name="expression">谓词表达式,范例1：t => t.Name == "A" ，结果1。 
        /// 范例2：t => t.Name == "A" &amp;&amp; t.Age =1 ，结果2。</param> 
        public static int GetConditionCount(this LambdaExpression expression)
        {
            if (expression == null)
                return 0;
            var result = expression.ToString().Replace("AndAlso", "|").Replace("OrElse", "|");
            return result.Split('|').Count();
        }


        #endregion


        #region GetAttribute(获取特性) 
        /// <summary> 
        /// 获取成员 
        /// </summary> 
        /// <param name="expression">表达式,范例：t => t.Name</param> 
        public static MemberInfo GetMember(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            if (memberExpression == null)
                return null;
            return memberExpression.Member;
        }

        /// <summary> 
        /// 获取成员表达式 
        /// </summary> 
        /// <param name="expression">表达式</param> 
        static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression == null)
                return null;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetMemberExpression(((LambdaExpression)expression).Body);
                case ExpressionType.Convert:
                    return GetMemberExpression(((UnaryExpression)expression).Operand);
                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;
            }
            return null;
        }

        /// <summary> 
        /// 获取特性 
        /// </summary> 
        /// <typeparam name="TAttribute">特性类型</typeparam> 
        /// <param name="expression">属性表达式</param> 
        public static TAttribute[] GetAttribute<TAttribute>(Expression expression) where TAttribute : Attribute
        {
            var memberInfo = GetMember(expression);
            return memberInfo.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().ToArray();
        }


        /// <summary> 
        /// 获取特性 
        /// </summary> 
        /// <typeparam name="TEntity">实体类型</typeparam> 
        /// <typeparam name="TProperty">属性类型</typeparam> 
        /// <typeparam name="TAttribute">特性类型</typeparam> 
        /// <param name="propertyExpression">属性表达式</param> 
        public static TAttribute[] GetAttribute<TEntity, TProperty, TAttribute>(Expression<Func<TEntity, TProperty>> propertyExpression) where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(propertyExpression);
        }


        /// <summary> 
        /// 获取特性 
        /// </summary> 
        /// <typeparam name="TProperty">属性类型</typeparam> 
        /// <typeparam name="TAttribute">特性类型</typeparam> 
        /// <param name="propertyExpression">属性表达式</param> 
        public static TAttribute[] GetAttribute<TProperty, TAttribute>(Expression<Func<TProperty>> propertyExpression) where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(propertyExpression);
        }


        #endregion

        internal class ParameterReplacer : ExpressionVisitor
        {
            public ParameterReplacer(ParameterExpression paramExpr)
            {
                this.ParameterExpression = paramExpr;
            }

            public ParameterExpression ParameterExpression { get; private set; }

            public Expression Replace(Expression expr)
            {
                return this.Visit(expr);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                return this.ParameterExpression;
            }
        }
    }
}