using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CmsEngine.Application.Helpers;
using CmsEngine.Core;

namespace CmsEngine.Application.Extensions
{
    public static class EnumerableExtensions
    {
        public static Expression<Func<T, bool>> GetSearchExpression<T>(this IEnumerable<T> element, string searchValue, IEnumerable<PropertyInfo> properties)
        {
            var expressionFilter = new List<ExpressionFilter>();

            foreach (var property in properties)
            {
                expressionFilter.Add(new ExpressionFilter
                {
                    PropertyName = property.Name,
                    Operation = Operation.Contains,
                    Value = searchValue
                });
            }

            return ExpressionBuilder.GetExpression<T>(expressionFilter, LogicalOperator.Or);
        }
    }
}
