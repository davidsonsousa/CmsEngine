using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CmsEngine.Helpers
{
    public sealed class ExpressionFilter
    {
        public string PropertyName { get; set; }
        public Operations Operation { get; set; }
        public object Value { get; set; }
    }
    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static Expression<Func<T, bool>> GetExpression<T>(IList<ExpressionFilter> filters, LogicalOperators logicalOperator)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            while (filters.Count > 0)
            {
                var f1 = filters[0];
                var nullCheck = new ExpressionFilter
                {
                    PropertyName = f1.PropertyName,
                    Operation = Operations.NotEqual,
                    Value = null
                };

                if (exp == null)
                {
                    exp = GetExpression<T>(param, nullCheck, filters[0], LogicalOperators.AndAlso);
                }
                else
                {
                    switch (logicalOperator)
                    {
                        case LogicalOperators.And:
                            exp = Expression.And(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperators.AndAlso));
                            break;
                        case LogicalOperators.Or:
                            exp = Expression.Or(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperators.AndAlso));
                            break;
                        case LogicalOperators.OrElse:
                            exp = Expression.OrElse(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperators.AndAlso));
                            break;
                        case LogicalOperators.AndAlso:
                        default:
                            exp = Expression.AndAlso(exp, GetExpression<T>(param, nullCheck, filters[0], LogicalOperators.AndAlso));
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
                case Operations.Equals:
                    return Expression.Equal(member, constant);

                case Operations.NotEqual:
                    return Expression.NotEqual(member, constant);

                case Operations.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Operations.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operations.LessThan:
                    return Expression.LessThan(member, constant);

                case Operations.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operations.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Operations.NotContain:
                    return Expression.Not(Expression.Call(member, containsMethod, constant));

                case Operations.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Operations.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, ExpressionFilter filter1, ExpressionFilter filter2, LogicalOperators logicalOperator)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            switch (logicalOperator)
            {
                case LogicalOperators.And:
                    return Expression.And(bin1, bin2);
                case LogicalOperators.Or:
                    return Expression.Or(bin1, bin2);
                case LogicalOperators.OrElse:
                    return Expression.OrElse(bin1, bin2);
                case LogicalOperators.AndAlso:
                default:
                    return Expression.AndAlso(bin1, bin2);
            }
        }
    }
}
