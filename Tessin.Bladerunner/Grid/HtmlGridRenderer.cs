using System.Collections.Generic;
using System.Linq;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;
using Literal = LINQPad.Controls.Literal;
using Table = Tessin.Bladerunner.Controls.Table;
using TableCell = Tessin.Bladerunner.Controls.TableCell;
using TableRow = Tessin.Bladerunner.Controls.TableRow;

namespace Tessin.Bladerunner.Grid
{
    public class HtmlGridRenderer<T> : IGridRenderer<T>
    {
        public object Render(EntityGrid<T> g)
        {
            if (!g._rows.Any()) return g._emptyContent;

            bool[] isEmptyColumn = Enumerable.Repeat(true, g._columns.Values.Count(e => !e.Removed)).ToArray();

            Table RenderTable(IEnumerable<TableRow> tableRows)
            {
                var table = new Table(tableRows);
                table.ClearStyles();
                table.SetClass("entity-grid");

                if (g._gridWidth != null)
                {
                    table.Styles["width"] = g._gridWidth;
                }

                return table;
            }

            TableCell RenderCell(int index, GridColumn<T> column, Control content, bool isHeader = false)
            {
                var cell = new TableCell(isHeader, content);

                if (!isHeader && isEmptyColumn[index])
                {
                    if (content is not EmptySpan && content is not EmptyIcon)
                    {
                        isEmptyColumn[index] = false;
                    }
                }

                cell.Styles["white-space"] = "nowrap";

                if (column.CellAlignment == CellAlignment.Right)
                {
                    cell.Styles["text-align"] = "right";
                }
                else if (column.CellAlignment == CellAlignment.Center)
                {
                    cell.Styles["text-align"] = "center";
                }

                return cell;
            }

            TableCell RenderHeaderCell(int index, GridColumn<T> column, Control content)
            {
                var cell = RenderCell(index, column, content, true);
                cell.HtmlElement.SetAttribute("width", column.Width);
                return cell;
            }

            TableCell RenderSummaryCell(int index, GridColumn<T> column, Control content)
            {
                var cell = RenderCell(index, column, content);
                cell.SetClass("columntotal");
                return cell;
            }

            var columns = g._columns.Values.Where(e => !e.Removed && e.CellRenderer != null)
                .OrderBy(e => e.Order).ToList();

            TableRow RenderRow(T e)
            {
                bool highlight = g._highlightRowPredicate?.Invoke(e) ?? false;

                var row = new TableRow(
                    columns.Select((f, i) => RenderCell(i, f, f.CellRenderer.Render(f.GetValue(e), f, e)))
                );

                if (highlight) row.AddClass("highlight");
                
                return row;
            }

            var header = new TableRow(columns.Select((e, i) =>
                RenderHeaderCell(i, e, new Literal(e.Label.IsMatch("_+") ? "" : e.Label))));

            var rows = g._rows.Select(RenderRow);

            var renderedRows = new[] { header }.Concat(rows).ToArray();

            if (columns.Any(e => e.SummaryMethod != null))
            {
                var summary = new TableRow(columns.Select((e, i) =>
                    RenderSummaryCell(i, e,
                        e.SummaryMethod != null
                            ? e.CellRenderer.Render(e.SummaryMethod(g._rows), e, default(T))
                            : new Literal(""))));
                renderedRows = renderedRows.Append(summary).ToArray();
            }

            if (g._removeEmptyColumns && isEmptyColumn.Any(e => e))
            {
                var indexes = isEmptyColumn.Select((e, i) => (e, i)).Where(x => x.e).Select(x => x.i).Reverse()
                    .ToArray();
                foreach (var row in renderedRows)
                {
                    var _row = row;
                    foreach (var index in indexes)
                    {
                        _row.Cells.RemoveAt(index);
                    }
                }
            }
            
            return RenderTable(renderedRows);
        }
    }
}