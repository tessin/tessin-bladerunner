using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication.ExtendedProtection;
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

        public bool Negate { get; set; }

        public GroupRule<T> _parent;
        
        private DumpContainer _dc;

        private QueryOperator _operator;

        private List<IQueryRule<T>> _rules;

        public void Refresh(QueryBuilder<T> builder)
        {
            Action refresh = null;

            object RenderRule(IQueryRule<T> rule)
            {
                if (rule is GroupRule<T> group)
                {
                    return group.Render(builder);
                }

                var lstRules = new SelectBox(builder.Labels.ToArray(), rule.RuleIndex)
                {
                    Width = "15em"
                };

                lstRules.SelectionChanged += (_, __) => {
                    var index = _rules.IndexOf(rule);
                    _rules.Remove(rule);
                    var newRule = builder.Rules[lstRules.SelectedIndex]();
                    newRule.RuleIndex = lstRules.SelectedIndex;
                    _rules.Insert(index, newRule);
                    refresh();
                };

                var chkNegate = new CheckBox("NOT", false, (chk) =>
                {
                    rule.Negate = chk.Checked;
                });
                
                var btnDelete = new IconButton(Icons.Delete, (_) =>
                {
                    _rules.Remove(rule);
                    refresh();
                });

                return Layout.Horizontal(true, lstRules, chkNegate, rule.Render(builder), btnDelete);
            }

            refresh = () => {
                if (_dc != null)
                {
                    _dc.Content = Layout.Vertical(true, _rules.Select(RenderRule));
                    _dc.Refresh();
                }
            };

            refresh();
        }

		public object Render(QueryBuilder<T> builder)
		{
            _dc = new DumpContainer {Style = "margin-left:1em;margin-top:0.6em;"};

			var ruleButton = new IconButton(Icons.Plus, (_) => {
				_rules.Add(builder.Rules[0]());
				Refresh(builder);
			}, tooltip:"Add Rule");

            IconButton groupButton = null;
            IconButton deleteButton = null;

            ContextMenu contextMenu = null;

            if (_parent != null)
            {
                deleteButton = new IconButton(Icons.Delete, (_) =>
                {
                    _parent._rules.Remove(this);
                    _parent.Refresh(builder);
                });

                groupButton = new IconButton(Icons.FolderOpen, (_) =>
                {
                    _rules.Add(new GroupRule<T>(QueryOperator.And, this));
                    Refresh(builder);
                });
            }
            else
            {
                contextMenu = new ContextMenu(
                    new IconButton(Icons.DotsHorizontal),
                    new ContextMenu.Item("Add Group", (_) => {
                        _rules.Add(new GroupRule<T>(QueryOperator.And, this));
                        Refresh(builder);
                    }, icon: Icons.FolderOpen),
                    new ContextMenu.Item("Save...", (_) => {
                        //todo?
                    }),
                    new ContextMenu.Item("Load...", (_) => {
                        //todo?
                    })
                );
            }

            Refresh(builder);

            var groupId = Guid.NewGuid().ToString();
            var optAnd = new RadioButton(groupId, "AND", true, (_) => { _operator = QueryOperator.And; });
			var optOr = new RadioButton(groupId, "OR", false, (_) => { _operator = QueryOperator.Or; });

			return Layout.Vertical(true,
				Layout.Horizontal(true, optAnd, optOr, ruleButton, groupButton, deleteButton, contextMenu),
				_dc
			);
		}

        public GroupRule(QueryOperator op, GroupRule<T> parent, params IQueryRule<T>[] rules)
		{
			_operator = op;
            _parent = parent;
			_rules = new List<IQueryRule<T>>(rules);
		}

		public Expression<Func<T, bool>> ToExpression()
		{
            var builder = _operator == QueryOperator.And ? PredicateBuilder.New<T>(true) : PredicateBuilder.New<T>(false);
            foreach (var rule in _rules)
            {
                var expr = rule.ToExpression();

                if (rule.Negate)
                {
                    expr = PredicateBuilder.Not(expr);
                }

                builder = _operator == QueryOperator.And ? builder.And(expr) : builder.Or(expr);
            }
            return builder;
		}
	}
}
