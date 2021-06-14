using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Query
{
	public class BoolRule<T> : IQueryRule<T>
    {
        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private Expression<Func<T, bool>> _expr;

        public BoolRule(Expression<Func<T, bool>> expr)
        {
            _expr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(true)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            return null;
        }
    }
}
