﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Tessin.Bladerunner.Query
{
	public interface IQueryRule<T>
    {
        int RuleIndex { get; set; }

        object Render(QueryBuilder<T> builder);

        Expression ToExpression(ParameterExpression p);
    }
}