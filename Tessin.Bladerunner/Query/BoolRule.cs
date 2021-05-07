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

        private string _field;

        private bool _value;

        public BoolRule(string field, bool value = false)
        {
            _field = field;
            _value = value;
        }

        public Expression ToExpression(ParameterExpression p)
        {
            var property = Expression.Field(p, _field);
            var constant = Expression.Constant(_value);
            return Expression.Equal(property, constant);
        }

        public object Render(QueryBuilder<T> builder)
        {
            var chkValue = new CheckBox()
            {
                Checked = _value
            };
            chkValue.Click += (_, __) =>
            {
                _value = chkValue.Checked;
            };
            return chkValue;
        }
    }
}
