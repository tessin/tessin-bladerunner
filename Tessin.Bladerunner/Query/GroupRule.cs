using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LinqKit;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Query
{
	public enum QueryOperator
	{
		And,
		Or
	}

	public class GroupRule<T> : IQueryRule<T>
	{
        public int RuleIndex { get; set; }

		public object Render(QueryBuilder<T> builder)
		{
            var dc = new DumpContainer {Style = "margin-left:1em;"};

            Action refresh = null;

			object RenderRule(IQueryRule<T> rule)
            {
                if (rule is GroupRule<T> group)
				{
					return group.Render(builder);
				}

                var lstRules = new SelectBox(builder.Labels.ToArray(), rule.RuleIndex)
                {
                    Width = "10em"
                };
                lstRules.SelectionChanged += (_, __) => {
                    var index = _rules.IndexOf(rule);
                    _rules.Remove(rule);
                    var newRule = builder.Rules[lstRules.SelectedIndex]();
                    newRule.RuleIndex = lstRules.SelectedIndex;
                    _rules.Insert(index, newRule);
                    refresh();
                };
                var btnDelete = new IconButton(Icons.Delete, (_) =>
                {
                    _rules.Remove(rule);
                    refresh();
                });
                return Layout.Horizontal(true, lstRules, rule.Render(builder), btnDelete);
            }

			refresh = () => {
                dc.Content = Layout.Vertical(false, _rules.Select(RenderRule));
                dc.Refresh();
            };

			var ruleButton = new IconButton(Icons.Plus, (_) => {
				_rules.Add(builder.Rules[0]());
				refresh();
			});

			var groupButton = new IconButton(Icons.Plus, (_) =>
			{
				_rules.Add(new GroupRule<T>(QueryOperator.And));
				refresh();
			});

            var deleteButton = new IconButton(Icons.Delete, (_) =>
            {

            });

			refresh();

            var groupId = Guid.NewGuid().ToString();
            var optAnd = new RadioButton(groupId, "AND", true, (_) => { _operator = QueryOperator.And; });
			var optOr = new RadioButton(groupId, "OR", false, (_) => { _operator = QueryOperator.Or; });

			return Layout.Vertical(true,
				Layout.Horizontal(true, optAnd, optOr, ruleButton, groupButton, deleteButton),
				dc
			);
		}

		private QueryOperator _operator;

		private List<IQueryRule<T>> _rules;

        public GroupRule(QueryOperator op, params IQueryRule<T>[] rules)
		{
			_operator = op;
			_rules = new List<IQueryRule<T>>(rules);
		}

		public Expression<Func<T, bool>> ToExpression()
		{
            var builder = _operator == QueryOperator.And ? PredicateBuilder.New<T>(true) : PredicateBuilder.New<T>(false);
            foreach (var rule in _rules)
            {
                builder = _operator == QueryOperator.And ? builder.And(rule.ToExpression()) : builder.Or(rule.ToExpression());
            }

			return builder;
		}
	}
}
