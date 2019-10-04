using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CmsEngine.Core;

namespace CmsEngine.Domain.Helpers
{
    public sealed class ExpressionFilter
    {
        public string PropertyName { get; set; }
        public Operation Operation { get; set; }
        public object Value { get; set; }
    }
    public static class ExpressionBuilder
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static Expression<Func<T, bool>> GetExpression<T>(IList<ExpressionFilter> filters, LogicalOperator logicalOperator)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            while (filters.Count > 0)
            {
                var f1 = filters[0];
                var nullCheck = new ExpressionFilter
                {
                    PropertyName = f1.PropertyName,
                    Operation = Operation.NotEqual,
                    Value = null
                };

                if (exp == null)
                {
                    exp = GetExpression<T>(param, nullCheck, filters[0], LogicalOperator.AndAlso);
                }
                else
                {
                    switch (logicalOperator)
                    {
                        case LogicalOperator.And:
                            exp = Expression.And(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperator.AndAlso));
                            break;
                        case LogicalOperator.Or:
                            exp = Expression.Or(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperator.AndAlso));
                            break;
                        case LogicalOperator.OrElse:
                            exp = Expression.OrElse(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperator.AndAlso));
                            break;
                        case LogicalOperator.AndAlso:
                        default:
                            exp = Expression.AndAlso(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperator.AndAlso));
                            break;
                    }
                }

                filters.Remove(f1);
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Operation.Equals:
                    return Expression.Equal(member, constant);

                case Operation.NotEqual:
                    return Expression.NotEqual(member, constant);

                case Operation.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Operation.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operation.LessThan:
                    return Expression.LessThan(member, constant);

                case Operation.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operation.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Operation.NotContain:
                    return Expression.Not(Expression.Call(member, containsMethod, constant));

                case Operation.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Operation.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, ExpressionFilter filter1, ExpressionFilter filter2, LogicalOperator logicalOperator)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            switch (logicalOperator)
            {
                case LogicalOperator.And:
                    return Expression.And(bin1, bin2);
                case LogicalOperator.Or:
                    return Expression.Or(bin1, bin2);
                case LogicalOperator.OrElse:
                    return Expression.OrElse(bin1, bin2);
                case LogicalOperator.AndAlso:
                default:
                    return Expression.AndAlso(bin1, bin2);
            }
        }
    }
}
