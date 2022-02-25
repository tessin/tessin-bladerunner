using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            Root = new GroupRule<T>(QueryOperator.And, null);
        }

        public object Render()
        {
            return Root.Render(this);
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, string>> expr)
        {
            Rules.Add(() => new StringRule<T>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, string>> expr, string[] options)
        {
            Rules.Add(() => new SelectRule<T>(expr, options));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, bool>> expr)
        {
            Rules.Add(() => new BoolRule<T>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, Guid>> expr)
        {
            Rules.Add(() => new GuidRule<T>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, DateTime>> expr)
        {
            Rules.Add(() => new DateRule<T>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, DateTime?>> expr)
        {
            Rules.Add(() => new DateRule<T>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, Expression<Func<T, int>> expr)
        {
            Rules.Add(() => new NumberRule<T, int>(expr));
            Labels.Add(label);
            return this;
        }

        public QueryBuilder<T> AddRule(string label, RuleFactory<T> rule)
        {
            Rules.Add(rule);
            Labels.Add(label);
            return this;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return this.Root.ToExpression();
        }

    }
}
