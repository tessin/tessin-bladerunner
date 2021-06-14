using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Query
{
	public class SelectRule<T> : IQueryRule<T>
    {

        public int RuleIndex { get; set; }

        public bool Negate { get; set; }

        private string _value;

        private readonly string[] _options;

        private readonly Expression<Func<T, string>> _expr;

        public SelectRule(Expression<Func<T, string>> expr, string[] options)
        {
            _expr = expr;
            _options = options;
            _value = _options[0];
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var p = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Invoke(_expr, p), Expression.Constant(_value)), p);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var lstOptions = new SelectBox(SelectBoxKind.DropDown, _options, 0,
                (lst) => { _value = (string) lst.SelectedOption; }) {Width = "20em", SelectedOption = _value};
            return Layout.Horizontal(true, lstOptions);
        }
    }
}
