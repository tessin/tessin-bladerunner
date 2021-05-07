using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Query
{
	public delegate IQueryRule<T> RuleFactory<T>();

    public class QueryBuilder<T>
    {
        public GroupRule<T> Root { get; set; }

        public List<RuleFactory<T>> Rules { get; set; } = new List<RuleFactory<T>>();

        public List<string> Labels { get; set; } = new List<string>();

        public QueryBuilder()
        {
            Root = new GroupRule<T>(QueryOperator.And);
        }

        public object Render()
        {
            return Root.Render(this);
        }

        public QueryBuilder<T> AddRule(string label, RuleFactory<T> rule)
        {
            Rules.Add(rule);
            Labels.Add(label);
            return this;
        }
    }
}
