using System;
using System.Linq.Expressions;

namespace Tessin.Bladerunner.Query
{
    public interface IQueryRule<T>
    {
        int RuleIndex { get; set; }

        bool Negate { get; set; }

        object Render(QueryBuilder<T> builder);

        Expression<Func<T, bool>> ToExpression();
    }
}
