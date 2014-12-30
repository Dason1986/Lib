﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.FastReflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConstructorInvoker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object Invoke(params object[] parameters);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConstructorInvoker : IConstructorInvoker
    {
        private readonly Func<object[], object> _mInvoker;

        /// <summary>
        /// 
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorInfo"></param>
        public ConstructorInvoker(ConstructorInfo constructorInfo)
        {
            this.ConstructorInfo = constructorInfo;
            this._mInvoker = InitializeInvoker(constructorInfo);
        }

        private Func<object[], object> InitializeInvoker(ConstructorInfo constructorInfo)
        {
            // Target: (object)new T((T0)parameters[0], (T1)parameters[1], ...)

            // parameters to execute
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            var parameterExpressions = new List<Expression>();
            var paramInfos = constructorInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            // new T((T0)parameters[0], (T1)parameters[1], ...)
            var instanceCreate = Expression.New(constructorInfo, parameterExpressions);

            // (object)new T((T0)parameters[0], (T1)parameters[1], ...)
            var instanceCreateCast = Expression.Convert(instanceCreate, typeof(object));

            var lambda = Expression.Lambda<Func<object[], object>>(instanceCreateCast, parametersParameter);

            return lambda.Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoke(params object[] parameters)
        {
            return this._mInvoker(parameters);
        }

        #region IConstructorInvoker Members

        object IConstructorInvoker.Invoke(params object[] parameters)
        {
            return this.Invoke(parameters);
        }

        #endregion
    }
}
