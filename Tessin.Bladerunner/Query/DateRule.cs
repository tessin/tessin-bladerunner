using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Query
{

    public class DateRule<T> : IQueryRule<T>
    {
        private string[] operators = new[]
        {
            "Equals"
        };

        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private string _value { get; set; }

        private string _operator { get; set; }

        private Expression<Func<T, DateTime>> _expr { get; set; }

        private Expression<Func<T, DateTime?>> _nullableExpr { get; set; }

        private bool IsNullable => _nullableExpr != null;

        public DateRule(Expression<Func<T, DateTime>> expr)
        {
            _expr = expr;
        }

        public DateRule(Expression<Func<T, DateTime?>> expr)
        {
            _nullableExpr = expr;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));

            /*
            if (_operator == "Contains")
            {
                var contains = typeof(string).GetMethod("Contains", new[] {typeof(string)});
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Call(Expression.Invoke(_expr, p), contains, Expression.Constant(_value)), p);
            }
            */

            return Expression.Lambda<Func<T, bool>>(
                Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var txtOperator = new SelectBox(operators)
            {
                Width = "10em",
                SelectedOption = _operator
            };
            txtOperator.SelectionChanged += (_, __) => { _operator = (string) txtOperator.SelectedOption; };

            var txtValue = new TextBox()
            {
                Width = "10em",
                Text = _value
            };
            txtValue.TextInput += (_, __) => { _value = txtValue.Text; };

            return Layout.Horizontal(txtOperator, txtValue);
        }
    }
}
