using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{

    public static class EntityEditorHelper
    {
        public static EntityEditor<T> Create<T>(T obj, Action<T> save, Action<T> preview = null) where T : new()
        {
            return new EntityEditor<T>(obj, save, preview);
        }
    }

    public class EntityEditor<T> where T : new()
    {
        private Dictionary<string, EditorField<T>> _fields;

        private readonly Action<T> _save;

        private readonly Action<T> _preview;

        private T _obj;

        private readonly EditorFactory<T> _factory;

        private string _actionVerb;

        public EntityEditor(T obj, Action<T> save, Action<T> preview = null, string actionVerb = "Save")
        {
            _save = save;
            _preview = preview;
            _obj = obj;
            _factory = new EditorFactory<T>();
            _actionVerb = actionVerb;
            Scaffold();
        }

        private void Scaffold()
        {
            _fields = new Dictionary<string, EditorField<T>>();

            var type = _obj.GetType();

            var fields = type
                .GetFields()
                .Select(e => new {e.Name, Type = e.FieldType, FieldInfo = e, PropertyInfo = (PropertyInfo) null})
                .Union(
                    type
                        .GetProperties()
                        .Select(e => new {e.Name, Type = e.PropertyType, FieldInfo = (FieldInfo) null, PropertyInfo = e}
                        )
                )
                //.OrderBy(e => e.Name)
                .ToList();

            int order = fields.Count();
            foreach (var field in fields)
            {
                _fields[field.Name] =
                    new EditorField<T>(field.Name, field.Name, order++, ScaffoldEditor(field.Name, field.Type),
                        field.FieldInfo, field.PropertyInfo);
            }
        }

        private IFieldEditor<T> ScaffoldEditor(string name, Type type)
        {
            if (name.EndsWith("Id") && (type == typeof(Guid) || type == typeof(int) || type == typeof(string)))
            {
                return _factory.Literal();
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return _factory.Bool();
            }

            if (type == typeof(string))
            {
                return _factory.Text();
            }

            if (type.IsNumeric())
            {
                return _factory.Number();
            }

            if (type.IsDate())
            {
                return _factory.Date();
            }

            return null;
        }

        public object Render()
        {
            var fields = _fields.Values.Where(e => !e.Removed && e.Editor != null).ToList();

            var columns = fields
                .GroupBy(e => e.Column)
                .OrderBy(e => e.Key)
                .Select(e => Layout.Gap(false).Vertical(
                    e.GroupBy(f => f.Group)
                    .OrderBy(f => f.Key)
                    .Select(f =>
                        Layout.Gap(false).Vertical( 
                            f.Key == null
                            ? Layout.Gap(false).Vertical(
                                f.OrderBy(h => h.Order).Select(h => h.Editor.Render(_obj, h, Updated)).ToArray())
                            : new CollapsablePanel(f.Key,
                                Layout.Gap(false).Vertical(
                                    f.OrderBy(h => h.Order).Select(h => h.Editor.Render(_obj, h, Updated)).ToArray()))
                        ))
                        .ToArray())).ToArray();

            void Updated()
            {
                var pObj = new T();
                foreach (var field in fields)
                {
                    field.Editor.Save(pObj, field);
                }

                foreach (var field in fields)
                {
                    field.Editor.SetVisibility(field.ShowIf(pObj));
                }

                _preview?.Invoke(pObj);
            }

            var validationLabel = Typography.Error("");

            var saveButton = new Button(_actionVerb, (_) =>
            {
                var validationErrors = fields.Select(e => e.Editor.Validate(_obj, e)).Count(e => !e);

                if (validationErrors == 0)
                {
                    foreach (var field in fields)
                    {
                        field.Editor.Save(_obj, field);
                    }
                    _save?.Invoke(_obj);
                }
                else
                {
                    validationLabel.HtmlElement.InnerText = $"{validationErrors} validation error{(validationErrors > 1 ? "s" : "")}.";
                }
            });

            Updated();

            return new HeaderPanel(
                Layout.Horizontal(saveButton, validationLabel), 
                Layout.Horizontal( //columns
                    columns
                )
            );
        }

        public EntityEditor<T> Editor(Expression<Func<T, object>> field, Func<EditorFactory<T>, IFieldEditor<T>> editor)
        {
            var hint = GetField(field);
            hint.Editor = editor(_factory);
            return this;
        }

        public EntityEditor<T> Editor<TU>(Func<EditorFactory<T>, IFieldEditor<T>> editor)
        {
            foreach (var hint in _fields.Values.Where(e => e.Type is TU))
            {
                hint.Editor = editor(_factory);
            }

            return this;
        }

        public EntityEditor<T> Description(Expression<Func<T, object>> field, string description)
        {
            var hint = GetField(field);
            hint.Description = description;
            return this;
        }

        public EntityEditor<T> Label(Expression<Func<T, object>> field, string label)
        {
            var hint = GetField(field);
            hint.Label = label;
            return this;
        }

        private EntityEditor<T> _Place(int col, params Expression<Func<T, object>>[] fields)
        {
            int order = 0;
            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Removed = false;
                hint.Order = order++;
                hint.Column = col;
            }

            return this;
        }

        public EntityEditor<T> Place(int col, params Expression<Func<T, object>>[] fields)
        {
            return _Place(col, fields);
        }

        public EntityEditor<T> Place(params Expression<Func<T, object>>[] fields)
        {
            return _Place(1, fields);
        }

        public EntityEditor<T> Group(string group, int column, params Expression<Func<T, object>>[] fields)
        {
            int order = 0;
            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Group = group;
                hint.Order = order++;
                hint.Column = column;
            }
            return this;
        }

        public EntityEditor<T> Group(string group, params Expression<Func<T, object>>[] fields)
        {
            return Group(group, 1, fields);
        }

        public EntityEditor<T> Remove(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = true;
            return this;
        }

        public EntityEditor<T> Add(Expression<Func<T, object>> field)
        {
            var hint = GetField(field);
            hint.Removed = false;
            return this;
        }

        public EntityEditor<T> Clear()
        {
            foreach (var field in _fields.Values)
            {
                field.Removed = true;
            }
            return this;
        }

        public EntityEditor<T> Helper(Expression<Func<T, object>> field, Func<Control, object> helper)
        {
            var hint = GetField(field);
            hint.Helper = helper;
            return this;
        }

        public EntityEditor<T> Validate<TU>(Expression<Func<T, TU>> field, Func<TU, (bool, string)> validator) 
        {
            var hint = GetField(field);
            hint.Validators.Add((o) => validator((TU)o));
            return this;
        }

        public EntityEditor<T> Required(Expression<Func<T, string>> field)
        {
            var hint = GetField(field);
            hint.Validators.Add(e => (string.IsNullOrWhiteSpace(((string)e)), "Required field.")) ;
            return this;
        }

        public EntityEditor<T> ShowIf(Expression<Func<T, object>> field, Func<T, bool> predicate)
        {
            var hint = GetField(field);
            hint.ShowIf = predicate;
            return this;
        }

        private EditorField<T> GetField<TU>(Expression<Func<T, TU>> field)
        {
            var name = Utils.GetNameFromMemberExpression(field.Body);
            return _fields[name];
        }

    }
}
