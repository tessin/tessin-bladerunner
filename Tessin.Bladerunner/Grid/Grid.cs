using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LINQPad.Controls;
using Table = LINQPad.Controls.Table;

namespace Tessin.Bladerunner.Grid
{
    public static class EntityGridHelper
    {
        public static Grid<T> Create<T>(IEnumerable<T> obj) where T : new()
        {
            return new Grid<T>(obj);
        }
    }
    
    public class Grid<T> where T : new()
    {
        private IEnumerable<T> _rows;

        private Dictionary<string, GridColumn<T>> _columns;

        private readonly CellRendererFactory<T> _rendererFactory;

        private string _width;

        private Action<T, TableRow> _rowAction;

        public Grid(IEnumerable<T> rows, string width = null)
        {
            _rows = rows;
            _width = width;
            _rendererFactory = new CellRendererFactory<T>();
            Scaffold();
        }

        private void Scaffold()
        {
            _columns = new Dictionary<string, GridColumn<T>>();

            //var type = _rows.GetType().GetGenericArguments()[0];

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

                var cellRenderer = _rendererFactory.Text();

                if (field.Type.IsNumeric())
                {
                    cellRenderer = _rendererFactory.Number();
                    cellAlignment = CellAlignment.Right;
                }

                if (field.Type.IsDate())
                {
                    cellRenderer = _rendererFactory.Date();
                }

                _columns[field.Name] =
                    new GridColumn<T>(field.Name, field.Name, order++, cellRenderer, cellAlignment, field.FieldInfo, field.PropertyInfo);
            }
        }

        public Control Render()
        {
            Table RenderTable(IEnumerable<TableRow> tableRows)
            {
                var table = new Table(tableRows);
                table.ClearStyles();

                if (_width != null)
                {
                    table.Styles["width"] = _width;
                }

                return table;
            }

            TableCell RenderCell(GridColumn<T> column, Control content, bool isHeader = false)
            {
                var cell = new TableCell(isHeader, content);

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

            TableCell RenderHeaderCell(GridColumn<T> column, Control content)
            {
                var cell = RenderCell(column, content, true);
                cell.HtmlElement.SetAttribute("width", column.Width);
                return cell;
            }

            TableCell RenderSummaryCell(GridColumn<T> column, Control content)
            {
                var cell = RenderCell(column, content);
                cell.SetClass("columntotal");
                return cell;
            }

            var columns = _columns.Values.Where(e => !e.Removed && e.CellRenderer != null)
                .OrderBy(e => e.Order).ToList();

            TableRow RenderRow(T e)
            {
                var row = new TableRow(
                    columns.Select(f => RenderCell(f, f.CellRenderer.Render(f.GetValue(e), f)))
                );
                _rowAction?.Invoke(e, row);
                return row; 
            }

            var header = new TableRow(columns.Select(e => RenderHeaderCell(e, new Literal(e.Label))));

            var rows = _rows.Select(RenderRow);

            var renderedRows = new[] {header}.Concat(rows);

            if (columns.Any(e => e.SummaryMethod != null))
            {
                var summary = new TableRow(columns.Select(e => 
                    RenderSummaryCell(e, e.SummaryMethod != null ? e.CellRenderer.Render(e.SummaryMethod(_rows),e) : new Literal(""))));
                renderedRows = renderedRows.Append(summary);
                
                return RenderTable(renderedRows);
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

        public Grid<T> Renderer(Expression<Func<T, object>> field, Func<CellRendererFactory<T>, ICellRenderer<T>> editor)
        {
            var hint = GetField(field);
            hint.CellRenderer = editor(_rendererFactory);
            return this;
        }

        public Grid<T> Renderer<TU>(Func<CellRendererFactory<T>, ICellRenderer<T>> editor)
        {
            foreach (var hint in _columns.Values.Where(e => e.Type is TU))
            {
                hint.CellRenderer = editor(_rendererFactory);
            }
            return this;
        }

        public Grid<T> Description(Expression<Func<T, object>> field, string description)
        {
            var hint = GetField(field);
            hint.Description = description;
            return this;
        }

        public Grid<T> Label(Expression<Func<T, object>> field, string label)
        {
            var hint = GetField(field);
            hint.Label = label;
            return this;
        }

        public Grid<T> Width(Expression<Func<T, object>> field, string width)
        {
            var hint = GetField(field);
            hint.Width = width;
            return this;
        }

        public Grid<T> Order(params Expression<Func<T, object>>[] fields)
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

        public Grid<T> Remove(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = true;
            return this;
        }

        public Grid<T> Add(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = false;
            return this;
        }

        public Grid<T> Clear()
        {
            foreach (var field in _columns.Values)
            {
                field.Removed = true;
            }
            return this;
        }

        public Grid<T> Row(Action<T,TableRow> rowAction)
        {
            _rowAction = rowAction;
            return this;
        }

        public Grid<T> Totals(Expression<Func<T, object>> field, Func<IEnumerable<T>,object> summaryMethod)
        {
            var _field = GetField(field);
            _field.SummaryMethod = summaryMethod;
            return this;
        }

        public Grid<T> Totals(Expression<Func<T, object>> field)
        {
            var _field = GetField(field);
            object SummaryMethod(IEnumerable<T> rows)
            {
                return rows.Select(e => _field.GetValue(e)).Where(e => e != null).Aggregate((a, b) => (dynamic)a + (dynamic)b);
            }
            return Totals(field, SummaryMethod);
        }

    }
}
