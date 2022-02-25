using LINQPad.Controls;
using System;
using System.Linq.Expressions;

namespace Tessin.Bladerunner.Query
{
    public class GuidRule<T> : IQueryRule<T>
    {
        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private Guid _value;

        private Expression<Func<T, Guid>> _expr;

        public GuidRule(Expression<Func<T, Guid>> expr)
        {
            _expr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var txtValue = new TextBox()
            {
                Text = _value == Guid.Empty ? "" : _value.ToString(),
                Width = "20em"
            };
            txtValue.TextInput += (_, __) =>
            {
                if (Guid.TryParse(txtValue.Text, out Guid guid))
                {
                    _value = guid;
                }
                else
                {
                    _value = Guid.Empty;
                }
            };
            return txtValue;
        }
    }
}
