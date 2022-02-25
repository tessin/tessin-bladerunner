using LINQPad.Controls;
using System;
using System.Linq.Expressions;

namespace Tessin.Bladerunner.Query
{
    public class StringRule<T> : IQueryRule<T>
    {
        private string[] operators = new[]
        {
            "is",
            "is empty",
            "contains",
            "starts with",
            "ends with"
        };

        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private string _value { get; set; }

        private string _operator { get; set; }

        private Expression<Func<T, string>> _expr { get; set; }

        public StringRule(Expression<Func<T, string>> expr)
        {
            _expr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));
            if (_operator == "contains")
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Invoke(_expr, p), method, Expression.Constant(_value)), p);
            }
            if (_operator == "starts with")
            {
                var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Invoke(_expr, p), method, Expression.Constant(_value)), p);
            }
            if (_operator == "ends with")
            {
                var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                return Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Invoke(_expr, p), method, Expression.Constant(_value)), p);
            }
            if (_operator == "is empty")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(null)), p);
            }
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var txtOperator = new SelectBox(operators)
            {
                Width = "10em",
                SelectedOption = _operator
            };
            txtOperator.SelectionChanged += (_, __) =>
            {
                _operator = (string)txtOperator.SelectedOption;
            };

            var txtValue = new TextBox()
            {
                Width = "10em",
                Text = _value
            };
            txtValue.TextInput += (_, __) =>
            {
                _value = txtValue.Text;
            };

            return Layout.Horizontal(txtOperator, txtValue);
        }
    }
}
