﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Data.Linq
{
    /// <summary>
    ///
    /// </summary>
    public static class SpecExprExtensions
    {
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

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExpr = one.Parameters[0];
            var body = Expression.Not(one.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one,
            Expression<Func<T, bool>> another)
        {
            // 首先定义好一个ParameterExpression
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            // 将表达式树的参数统一替换成我们定义好的candidateExpr
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);

            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
    }

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