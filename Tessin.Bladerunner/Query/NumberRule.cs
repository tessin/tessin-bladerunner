using LINQPad.Controls;
using System;
using System.Linq.Expressions;

namespace Tessin.Bladerunner.Query
{
    public class NumberRule<T, TNumeric> : IQueryRule<T>
    {
        private readonly string[] _operators = new[]
        {
            "is",
            "is more than",
            "is more or equal to",
            "is less than",
            "is less or equal to",
            "is empty"
        };

        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private TNumeric _value { get; set; }

        private string _operator { get; set; }

        private Expression<Func<T, TNumeric>> _expr { get; set; }

        public NumberRule(Expression<Func<T, TNumeric>> expr)
        {
            _expr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));

            if (_operator == "is more than")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
            }
            if (_operator == "is more or equal to")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
            }
            if (_operator == "is less than")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.LessThan(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
            }
            if (_operator == "is less or equal to")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
            }
            if (_operator == "is empty")
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(null)), p);
            }

            return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var txtOperator = new SelectBox(_operators)
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
                Text = _value.ToString()
            };
            txtValue.TextInput += (_, __) =>
            {
                try
                {
                    _value = (TNumeric)Convert.ChangeType(txtValue.Text, typeof(TNumeric));
                }
                catch (Exception)
                {
                    //ignore
                }
            };

            return Layout.Horizontal(txtOperator, txtValue);
        }
    }
}
