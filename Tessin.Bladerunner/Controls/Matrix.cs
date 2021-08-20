using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;

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
        public static Matrix<MatrixCell> Create(List<MatrixCell> cells)
        {
            return new(cells, c => c.Column, c => c.Row, c => c.FirstOrDefault()?.Value);
        }

        public Matrix(
            List<T> cells, 
            Expression<Func<T, string>> colExpr, 
            Expression<Func<T, string>> rowExpr,
            Func<IEnumerable<T>, object> cellRenderer)
        {
            this.AddClass("matrix");

            var cols = cells.AsQueryable().GroupBy(colExpr).Select(e => e.Key).Distinct().ToList();
            var rows = cells.AsQueryable().GroupBy(rowExpr).Select(e => e.Key).Distinct().ToList();

            var colFunc = colExpr.Compile();
            var rowFunc = rowExpr.Compile();

            var table = new Table();

            table.ClearStyles();

            table.Rows.Add(new TableRow(true, cols.Select(e => new TableCell(true, new Literal(e.ToString()))).ToArray().Prepend(new TableCell(true, new Literal("")))));


            TableCell MakeCell(object content)
            {
                Control child;
                if (content == null || content is string || content.GetType().IsNumeric())
                {
                    child = new Literal(content?.ToString() ?? "");
                }
                else
                {
                    var dc = new DumpContainer {Content = content};
                    child = dc;
                }

                return new TableCell(false, child);
            }

            foreach (var row in rows)
            {
                table.Rows.Add(new TableRow(false,
                    cols.Select(horizontalKey => cellRenderer(cells.Where(e => colFunc(e).Equals(horizontalKey) && rowFunc(e).Equals(row)))).Select(MakeCell).ToArray()
                        .Prepend(new TableCell(true, new Literal(row)))
                ));
            }

            this.VisualTree.Add(table);
        }

    }
}
