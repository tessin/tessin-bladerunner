using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Query
{
	public class StringRule<T> : IQueryRule<T>
    {
        private string[] operators = new[]
        {
            "Equals",
            "Contains"
        };

        public int RuleIndex { get; set; }

        private string _field { get; set; }

        private string _value { get; set; }

        private string _operator { get; set; }

        public StringRule(string field, string value = null)
        {
            _field = field;
            _value = value;
        }

        public Expression ToExpression(ParameterExpression p)
        {
            if (_operator == "Contains")
            {
                var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                return Expression.Call(Expression.Field(p, _field), contains, Expression.Constant(_value));
            }
            else
            {
                return Expression.Equal(Expression.Field(p, _field), Expression.Constant(_value));
            }
        }

        public object Render(QueryBuilder<T> builder)
        {
            var txtOperator = new SelectBox(operators)
            {
                Width = "10em",
                SelectedOption = _operator
            };
            txtOperator.SelectionChanged += (_, __) => {
                _operator = (string)txtOperator.SelectedOption;
            };

            var txtValue = new TextBox()
            {
                Width = "10em",
                Text = _value
            };
            txtValue.TextInput += (_, __) => {
                _value = txtValue.Text;
            };

            return Layout.Horizontal(true, txtOperator, txtValue);
        }
    }
}
