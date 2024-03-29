﻿using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tessin.Bladerunner.Alerts;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{

    public static class EntityEditorHelper
    {
        [Obsolete("Use Scaffold.Editor instead.")]
        public static EntityEditor<T> Create<T>(T obj, Action<T> save = null, Action<T> preview = null, string actionVerb = "Save", Control toolbar = null) where T : new()
        {
            return new EntityEditor<T>(obj, save, preview, actionVerb, toolbar);
        }
    }

    public class EntityEditor<T> : IAlertBuilderEditor, IRenderable where T : new()
    {
        private Dictionary<string, EditorField<T>> _fields;

        private readonly Action<T> _save;

        private readonly Action<T> _preview;

        private T _obj;

        private readonly EditorFactory<T> _factory;

        private string _actionVerb;

        private Control _toolbar;

        private List<string> _groups = new();

        [Obsolete("Use Scaffold.Editor instead.")]
        public EntityEditor(T obj, Action<T> save = null, Action<T> preview = null, string actionVerb = "Save", Control toolbar = null)
        {
            _save = save;
            _preview = preview;
            _obj = obj;
            _factory = new EditorFactory<T>();
            _actionVerb = actionVerb;
            _toolbar = toolbar;
            Scaffold();
        }

        private void Scaffold()
        {
            _fields = new Dictionary<string, EditorField<T>>();

            var type = _obj.GetType();

            var fields = type
                .GetFields()
                .Select(e => new { e.Name, Type = e.FieldType, FieldInfo = e, PropertyInfo = (PropertyInfo)null })
                .Union(
                    type
                        .GetProperties()
                        .Select(e => new { e.Name, Type = e.PropertyType, FieldInfo = (FieldInfo)null, PropertyInfo = e }
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
                return _factory.Number(type.SuggestDecimals());
            }

            if (type.IsDate())
            {
                return _factory.Date();
            }

            return null;
        }

        public object Render()
        {
            return Render(null);
        }
        
        public object Render(Controls.Button saveButton)
        {
            var fields = _fields.Values.Where(e => !e.Removed && e.Editor != null).ToList();

            object RenderRow(IEnumerable<EditorField<T>> fields)
            {
                if (fields.Count() == 1)
                {
                    var field = fields.First();
                    return field.Editor.Render(_obj, field, Updated(field));
                }
                return Layout.Horizontal(fields.Select(e => e.Editor.Render(_obj, e, Updated(e))));
            }

            Control RenderRows(IEnumerable<EditorField<T>> fields)
            {
                return Layout.Gap(true)
                    .Vertical(fields.OrderBy(h => h.Order)
                        .GroupBy(f => f.Row).Select(RenderRow));
            }

            var columns = fields
                .GroupBy(e => e.Column)
                .OrderBy(e => e.Key)
                .Select(e => Layout.Class("entity-editor--column").Gap(false).Vertical(
                    e.GroupBy(f => f.Group)
                    .OrderBy(f => _groups.IndexOf(f.Key))
                    .Select(f =>
                        Layout.Gap(false).Vertical(
                            f.Key == null
                            ? RenderRows(f)
                            : new CollapsablePanel(f.Key, RenderRows(f))
                        ))
                        .ToArray())).ToArray();

            Action Updated(EditorField<T> updatedField)
            {
                return () =>
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

                    if (updatedField != null)
                    {
                        foreach (var dependency in updatedField.Dependencies)
                        {
                            dependency.field.Editor.Update(dependency.transformer(pObj));
                        }
                    }
                    else
                    {
                        foreach (var dependency in fields.Where(e => e.Dependencies.Any()).SelectMany(e => e.Dependencies))
                        {
                            dependency.field.Editor.Update(dependency.transformer(pObj));
                        }
                    }

                    _preview?.Invoke(pObj);
                };
            }

            var validationLabel = Typography.Error("");

            Func<T> onSave = () =>
            {
                var validationErrors = fields.Select(e => e.Editor.Validate(_obj, e)).Count(e => !e);
                validationLabel.HtmlElement.InnerText = "";

                if (validationErrors == 0)
                {
                    foreach (var field in fields)
                    {
                        field.Editor.Save(_obj, field);
                    }
                    return _obj;
                }
                else
                {
                    validationLabel.HtmlElement.InnerText = $"{validationErrors} validation error{(validationErrors > 1 ? "s" : "")}";
                    return default(T);
                }
            };

            Updated(null)();

            if (saveButton == null)
            {
                saveButton = new Controls.Button(_actionVerb, (_) =>
                {
                    T obj = onSave();
                    if (obj != null)
                    {
                        _save?.Invoke(_obj);
                    }
                });

                return new HeaderPanel(
                    Layout.Middle().Horizontal(saveButton, _toolbar, validationLabel),
                    Layout.Vertical(
                        Layout.Horizontal(
                            columns
                        ),
                        new Spacer(height: "50px")
                    ),
                    width: null
                );
            }
            else
            {
                saveButton.Validate = () =>
                {
                    T obj = onSave();
                    saveButton.Tag = obj;
                    return obj != null;
                };
                return Layout.Gap(false).Vertical(
                    validationLabel,
                    Layout.Vertical(
                        Layout.Horizontal(
                            columns
                        ),
                        new Spacer(height: "50px")
                    )
                );
            }
        }

        public EntityEditor<T> Editor(Expression<Func<T, object>> field, Func<EditorFactory<T>, IFieldEditor<T>> editor)
        {
            var hint = GetField(field);
            hint.Editor = editor(_factory);
            return this;
        }

        public EntityEditor<T> Editor(Expression<Func<T, object>> field, IFieldEditor<T> editor)
        {
            var hint = GetField(field);
            hint.Editor = editor;
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

        private EntityEditor<T> _Place(int col, Guid? row, params Expression<Func<T, object>>[] fields)
        {
            int order = _fields.Values.Where(e => e.Column == col)
                .Select(e => (int?)e.Order).Max() ?? 0;

            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Removed = false;
                if (hint.Group == null)
                {
                    hint.Order = ++order;
                }
                hint.Column = col;
                hint.Row = row ?? Guid.NewGuid();
            }

            return this;
        }

        public EntityEditor<T> Place(int col, params Expression<Func<T, object>>[] fields)
        {
            return _Place(col, null, fields);
        }

        public EntityEditor<T> Place(params Expression<Func<T, object>>[] fields)
        {
            return _Place(1, null, fields);
        }

        public EntityEditor<T> Place(bool row, params Expression<Func<T, object>>[] fields)
        {
            return _Place(1, row ? Guid.NewGuid() : null, fields);
        }

        public EntityEditor<T> Place(int col, bool row, params Expression<Func<T, object>>[] fields)
        {
            return _Place(col, row ? Guid.NewGuid() : null, fields);
        }

        public EntityEditor<T> Group(string group, int column, params Expression<Func<T, object>>[] fields)
        {
            int order = 0;

            if (!_groups.Contains(group))
            {
                _groups.Add(group);
            }

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

        public EntityEditor<T> Derived<TU, TV>(Expression<Func<T, TU>> field, Expression<Func<T, TV>> derivedFrom, Func<T, TU> transformer)
        {
            var _derivedFrom = GetField(derivedFrom);
            var _field = GetField(field);
            _derivedFrom.Dependencies.Add((_field, x => transformer(x)));
            return this;
        }

        public EntityEditor<T> Enabled(bool enabled, params Expression<Func<T, object>>[] fields)
        {
            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Enabled = enabled;
            }
            return this;
        }

        public EntityEditor<T> Required(params Expression<Func<T, object>>[] fields)
        {
            foreach (var expr in fields)
            {
                var hint = GetField(expr);
                hint.Required = true;
                hint.Validators.Add(e =>
                {
                    bool valid = true;

                    if (e == null)
                    {
                        valid = false;
                    }
                    else
                    {
                        valid = e switch
                        {
                            null => false,
                            Guid guid => guid != Guid.Empty,
                            DateTime dt => dt != DateTime.MinValue,
                            string str => !string.IsNullOrWhiteSpace(str),
                            int i => i != 0,
                            double i => i != 0.0,
                            _ => true
                        };
                    }

                    return (valid, "Required field");
                });
            }

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
