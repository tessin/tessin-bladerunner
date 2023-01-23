using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;
using Literal = LINQPad.Controls.Literal;

namespace Tessin.Bladerunner.Grid
{
    
    [Obsolete("Use Scaffold.Grid instead.")]
    public static class EntityGridHelper
    {
        public static EntityGrid<T> Create<T>(IEnumerable<T> rows)
        {
            return new EntityGrid<T>(rows);
        }
    }

    public class EntityGrid<T> : IRenderable
    {
        private IGridRenderer<T> _gridRenderer = new AgGridRenderer<T>();

        internal IEnumerable<T> _rows;

        internal Dictionary<string, GridColumn<T>> _columns;

        internal CellRendererFactory<T> _rendererFactory;

        internal string _gridWidth;

        private readonly IContentFormatter _formatter = new DefaultContentFormatter();

        internal Control _emptyContent;

        internal bool _removeEmptyColumns = false;

        internal Func<T, bool> _highlightRowPredicate;

        public EntityGrid(IEnumerable<T> rows, string gridWidth = null)
        {
            _rows = rows;
            _gridWidth = gridWidth;
            _rendererFactory = new CellRendererFactory<T>(_formatter);
            _emptyContent = new Div(new Literal(""));
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

                //if (field.Type.IsAssignableFrom(typeof(Controls.Icon)))
                //{
                //    cellAlignment = CellAlignment.Center;
                //}

                if (field.Type == typeof(Controls.Icon) || field.Type == typeof(Controls.EmptyIcon))
                {
                    cellAlignment = CellAlignment.Center;
                }

                //if (field.Type.IsDate())
                //{
                //    cellRenderer = _rendererFactory.Date();
                //}

                //_, __ etc is ok, _XYZ is not
                var removed = field.Name.IsMatch("^_+[^_]+$");

                _columns[field.Name] =
                    new GridColumn<T>(field.Name, order++, cellRenderer, cellAlignment, field.FieldInfo, field.PropertyInfo, removed);
            }
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

        public EntityGrid<T> Totals(Expression<Func<T, object>> field, Func<IEnumerable<T>, object> summaryMethod)
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

        public EntityGrid<T> HighlightRow(Func<T, bool> predicate)
        {
            _highlightRowPredicate = predicate;
            return this;
        }

        public EntityGrid<T> Empty(object content)
        {
            _emptyContent = _formatter.Format(content);
            return this;
        }

        public EntityGrid<T> UseJsGrid()
        {
            this._gridRenderer = new AgGridRenderer<T>();
            return this;
        }
        
        public EntityGrid<T> UseHtmlGrid()
        {
            this._gridRenderer = new HtmlGridRenderer<T>();
            return this;
        }

        public object Render()
        {
            return _gridRenderer.Render(this);
        }
    }
}
