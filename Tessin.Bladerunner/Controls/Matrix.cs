﻿using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tessin.Bladerunner.Controls
{
    public class MatrixCell
    {
        public MatrixCell()
        {

        }

        public MatrixCell(string column, string row, object value)
        {
            Column = column;
            Row = row;
            Value = value;
        }

        public string Column { get; set; }

        public string Row { get; set; }

        public object Value { get; set; }
    }


    public class Matrix<T> : Div where T : new()
    {
        private IContentFormatter _contentFormatter;

        public static Matrix<MatrixCell> Create(List<MatrixCell> cells)
        {
            return new(cells, c => c.Column, c => c.Row, (c, _, _) => c.FirstOrDefault()?.Value);
        }

        public Matrix(
            List<T> cells,
            Expression<Func<T, string>> colExpr,
            Expression<Func<T, string>> rowExpr,
            Func<IEnumerable<T>, string, string, object> cellRenderer,
            IContentFormatter formatter = null
        )
        {
            _contentFormatter = formatter ?? new DefaultContentFormatter();

            this.AddClass("matrix");

            var cols = cells.AsQueryable().GroupBy(colExpr).Select(e => e.Key).Distinct().ToList();
            var rows = cells.AsQueryable().GroupBy(rowExpr).Select(e => e.Key).Distinct().ToList();

            this.Styles["grid-template-columns"] = $"auto repeat({cols.Count}, 1fr);";

            var colFunc = colExpr.Compile();
            var rowFunc = rowExpr.Compile();

            Div MakeColHeader(object label)
            {
                var div = new Div(new Span(label.ToString()));
                div.SetClass("col-header");
                return div;
            }

            Div MakeRowHeader(object label)
            {
                var div = new Div(new Span(label.ToString()));
                div.SetClass("row-header");
                return div;
            }

            Div MakeCell(object content)
            {
                var div = new Div(_contentFormatter.Format(content));
                div.SetClass("cell");
                return div;
            }

            this.VisualTree.AddRange(cols.Select(MakeColHeader).ToArray().Prepend(new Div(new Literal(""))));

            foreach (var row in rows)
            {
                this.VisualTree.AddRange(
                    cols.Select(col => cellRenderer(
                        cells.Where(e => colFunc(e).Equals(col) && rowFunc(e).Equals(row)), col, row)).Select(MakeCell).ToArray()
                        .Prepend(MakeRowHeader(row))
                );
            }
        }

    }
}
