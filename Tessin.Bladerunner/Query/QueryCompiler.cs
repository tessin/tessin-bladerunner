using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Tessin.Bladerunner.Query
{
	public static class QueryCompiler
    {
        public static Expression<Func<T, bool>> Compile<T>(GroupRule<T> query)
        {
            var itemExpression = Expression.Parameter(typeof(T));
            var conditions = query.ToExpression(itemExpression);
            if (conditions.CanReduce)
            {
                conditions = conditions.ReduceAndCheck();
            }
            return Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
        }
    }

}
