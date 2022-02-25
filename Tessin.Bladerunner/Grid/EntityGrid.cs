using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;
using Literal = LINQPad.Controls.Literal;
using Table=Tessin.Bladerunner.Controls.Table;
using TableRow = Tessin.Bladerunner.Controls.TableRow;
using TableCell = Tessin.Bladerunner.Controls.TableCell;

namespace Tessin.Bladerunner.Grid
{
    public static class EntityGridHelper
    {
        public static EntityGrid<T> Create<T>(IEnumerable<T> rows) 
        {
            return new EntityGrid<T>(rows);
        }
    }
    
    public class EntityGrid<T>
    {
        private readonly IEnumerable<T> _rows;

        private Dictionary<string, GridColumn<T>> _columns;

        private readonly CellRendererFactory<T> _rendererFactory;

        private readonly string _width;

        private Action<T, Controls.TableRow> _rowAction;

        private readonly IContentFormatter _formatter = new DefaultContentFormatter();

        private Control _empty;

        private bool _removeEmptyColumns = false;

        public EntityGrid(IEnumerable<T> rows, string width = null)
        {
            _rows = rows;
            _width = width;
            _rendererFactory = new CellRendererFactory<T>(_formatter);
            _empty = new Div(new Literal(""));
            Scaffold();
        }

        private void Scaffold()
        {
            _columns = new Dictionary<string, GridColumn<T>>();

            var type = typeof(T);

            var fields = type
                .GetFields()
                .Select(e => new { e.Name, Type = e.FieldType, FieldInfo = e, PropertyInfo = (PropertyInfo)null })
                .Union(
                    type
                        .GetProperties()
                        .Select(e => new { e.Name, Type = e.PropertyType, FieldInfo = (FieldInfo)null, PropertyInfo = e })
                )
                //.OrderBy(e => e.Name)
                .ToList();

            int order = fields.Count();
            foreach (var field in fields)
            {
                var cellAlignment = CellAlignment.Left;

                var cellRenderer = _rendererFactory.Default();

                if (field.Type.IsNumeric())
                {
                    //cellRenderer = _rendererFactory.Number();
                    cellAlignment = CellAlignment.Right;
                }

                if (field.Type == typeof(bool))
                {
                    cellAlignment = CellAlignment.Center;
                }

                if (field.Type.IsAssignableFrom(typeof(Controls.Icon)))
                {
                    cellAlignment = CellAlignment.Center;
                }

                //if (field.Type.IsDate())
                //{
                //    cellRenderer = _rendererFactory.Date();
                //}

                var removed = field.Name.StartsWith("_") && field.Name.Length > 1;

                _columns[field.Name] =
                    new GridColumn<T>(field.Name, field.Name, order++, cellRenderer, cellAlignment, field.FieldInfo, field.PropertyInfo, removed);
            }
        }

        public Control Render()
        {
            if (!_rows.Any()) return _empty;

            bool[] isEmptyColumn = Enumerable.Repeat(true, _columns.Values.Count(e => !e.Removed)).ToArray();

            Table RenderTable(IEnumerable<TableRow> tableRows)
            {
                var table = new Table(tableRows);
                table.ClearStyles();
                table.SetClass("entity-grid");

                if (_width != null)
                {
                    table.Styles["width"] = _width;
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
                else if(column.CellAlignment == CellAlignment.Center)
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

            var columns = _columns.Values.Where(e => !e.Removed && e.CellRenderer != null)
                .OrderBy(e => e.Order).ToList();

            TableRow RenderRow(T e)
            {
                var row = new TableRow(
                    columns.Select((f,i) => RenderCell(i, f, f.CellRenderer.Render(f.GetValue(e), f, e)))
                );
                _rowAction?.Invoke(e, row);
                return row; 
            }

            var header = new TableRow(columns.Select((e,i) => RenderHeaderCell(i, e, new Literal(e.Label == "_" ? "" : e.Label))));

            var rows = _rows.Select(RenderRow);

            var renderedRows = new[] {header}.Concat(rows).ToArray();

            if (columns.Any(e => e.SummaryMethod != null))
            {
                var summary = new TableRow(columns.Select((e,i) => 
                    RenderSummaryCell(i, e, e.SummaryMethod != null ? e.CellRenderer.Render(e.SummaryMethod(_rows), e, default(T)) : new Literal(""))));
                renderedRows = renderedRows.Append(summary).ToArray();
            }

            if (_removeEmptyColumns && isEmptyColumn.Any(e => e))
            {
                var indexes = isEmptyColumn.Select((e, i) => (e, i)).Where(x => x.e).Select(x => x.i).Reverse().ToArray();
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

        private GridColumn<T> GetField(Expression<Func<T, object>> field)
        {
            var name = GetNameFromMemberExpression(field.Body);
            return _columns[name];
        }

        private static string GetNameFromMemberExpression(Expression expression)
        {
            while (true)
            {
                switch (expression)
                {
                    case MemberExpression memberExpression:
                        return memberExpression.Member.Name;
                    case UnaryExpression unaryExpression:
                        expression = unaryExpression.Operand;
                        continue;
                }

                throw new ArgumentException("Invalid expression.");
            }
        }

        public EntityGrid<T> Renderer(Expression<Func<T, object>> field, Func<CellRendererFactory<T>, ICellRenderer<T>> editor)
        {
            var hint = GetField(field);
            hint.CellRenderer = editor(_rendererFactory);
            return this;
        }

        public EntityGrid<T> Renderer<TU>(Func<CellRendererFactory<T>, ICellRenderer<T>> editor)
        {
            foreach (var hint in _columns.Values.Where(e => e.Type is TU))
            {
                hint.CellRenderer = editor(_rendererFactory);
            }
            return this;
        }

        public EntityGrid<T> Description(Expression<Func<T, object>> field, string description)
        {
            var hint = GetField(field);
            hint.Description = description;
            return this;
        }

        public EntityGrid<T> Label(Expression<Func<T, object>> field, string label)
        {
            var hint = GetField(field);
            hint.Label = label;
            return this;
        }

        public EntityGrid<T> Align(Expression<Func<T, object>> field, CellAlignment alignment)
        {
            var hint = GetField(field);
            hint.CellAlignment = alignment;
            return this;
        }

        public EntityGrid<T> Width(Expression<Func<T, object>> field, string width)
        {
            var hint = GetField(field);
            hint.Width = width;
            return this;
        }

        public EntityGrid<T> Order(params Expression<Func<T, object>>[] fields)
        {
            int order = 0;
            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Removed = false;
                hint.Order = order++;
            }
            return this;
        }

        public EntityGrid<T> Remove(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = true;
            return this;
        }

        public EntityGrid<T> Add(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = false;
            return this;
        }

        public EntityGrid<T> Clear()
        {
            foreach (var field in _columns.Values)
            {
                field.Removed = true;
            }
            return this;
        }

        public EntityGrid<T> Reset()
        {
            foreach (var field in _columns.Values)
            {
                field.Removed = false;
                field.CellAlignment = CellAlignment.Left;
                field.CellRenderer = _rendererFactory.Text();
            }
            return this;
        }

        public EntityGrid<T> Row(Action<T,TableRow> rowAction)
        {
            _rowAction = rowAction;
            return this;
        }

        public EntityGrid<T> Totals(Expression<Func<T, object>> field, Func<IEnumerable<T>,object> summaryMethod)
        {
            var _field = GetField(field);
            _field.SummaryMethod = summaryMethod;
            return this;
        }

        public EntityGrid<T> Totals(Expression<Func<T, object>> field)
        {
            var _field = GetField(field);
            object SummaryMethod(IEnumerable<T> rows)
            {
                return rows.Select(e => _field.GetValue(e)).Where(e => e != null).Aggregate((a, b) => (dynamic)a + (dynamic)b);
            }
            return Totals(field, SummaryMethod);
        }

        public EntityGrid<T> RemoveEmptyColumns()
        {
            _removeEmptyColumns = true;
            return this;
        }

        public EntityGrid<T> Empty(object content)
        {
            _empty = _formatter.Format(content);
            return this;
        }
    }
}
