using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class LinqPadUtils
    {
        private static object _VerticalRun(bool withGaps, IEnumerable<object> elements)
        {
            if (!withGaps)
            {
                return LINQPad.Util.VerticalRun(elements);
            }
            return LINQPad.Util.VerticalRun(elements.Select(e => LINQPad.Util.WithStyle(e, "margin-bottom:0.4em;display:block;")));
        }

        public static object VerticalRun(bool withGaps, IEnumerable<object> elements)
        {
            return _VerticalRun(withGaps, elements);
        }

        public static object VerticalRun(bool withGaps, params object[] elements)
        {
            return _VerticalRun(withGaps, elements);
        }

        public static object DumpMatrix<T, T1, T2>(IQueryable<T> raw, Expression<Func<T, T1>> verticalExpr, Expression<Func<T, T2>> horizontalExpr, Func<IEnumerable<T>, object> cellRenderer) where T1 : IComparable where T2 : IComparable
        {
            var verticalKeys = raw.GroupBy(verticalExpr).Select(e => e.Key).Distinct().ToList();
            var horizontalKeys = raw.GroupBy(horizontalExpr).Select(e => e.Key).Distinct().ToList();

            var verticalFunc = verticalExpr.Compile();
            var horizontalFunc = horizontalExpr.Compile();

            var table = new Table();

            table.Rows.Add(new TableRow(true, horizontalKeys.Select(e => new TableCell(true, new Literal(e.ToString()))).ToArray().Prepend(new TableCell(true, new Literal("")))));

            foreach (var verticalKey in verticalKeys)
            {
                table.Rows.Add(new TableRow(false,
                    horizontalKeys.Select(horizontalKey => cellRenderer(raw.Where(e => horizontalFunc(e).Equals(horizontalKey) && verticalFunc(e).Equals(verticalKey)))).Select(e => new TableCell(false, new Literal(e.ToString()))).ToArray()
                        .Prepend(new TableCell(true, new Literal(verticalKey.ToString())))
                ));
            }

            return table;
        }
	}
}
