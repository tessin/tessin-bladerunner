using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Query
{
	public class ExpressionRule<T> : IQueryRule<T>
    {
        public int RuleIndex { get; set; }

        private string _field;

        private Expression<Func<T, bool>> _expr;

        public ExpressionRule(string field)
        {
            _field = field;
        }

        public ExpressionRule(Expression<Func<T, bool>> expr)
        {
            _expr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return _expr;
        }

        public object Render(QueryBuilder<T> builder)
        {
            return "";
        }
    }
}
