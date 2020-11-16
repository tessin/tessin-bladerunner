using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class EntityEditor<T>
    {
        private Action<T> _save;

        private T _obj;

        private readonly FieldEditorMapper<T> _mapper;
	
        public EntityEditor(T obj, Action<T> save)
        {
            _save = save;
            _obj = obj;
            _mapper = new FieldEditorMapper<T>();
        }
	
        public object Render()
        {
            var rendered = new List<object>();
            
            var type = _obj.GetType();

            var fields = type.GetFields();

            var editors = _mapper.Map(fields).ToList();

            rendered.AddRange(fields.Zip(editors, (field, editor) =>  (field, editor))
                .Select(e => e.editor.Render(_obj, e.field)).Where(e => e!=null));
            
            rendered.Add(new Button("Save"));

            return LINQPad.Util.VerticalRun(rendered);
        }

        public EntityEditor<T> EditorFor(Expression<Func<T,object>> field, Func<EditorFactory<T>,IFieldEditor<T>> editor)
        {
            var factory = new EditorFactory<T>();
            var name = GetNameFromMemberExpression(field.Body);
            _mapper.FieldMappings[name] = editor(factory);
            return this;
        }

        public EntityEditor<T> EditorFor<TU>(IFieldEditor<T> editor)
        {
            _mapper.TypeMappings[typeof(TU)] = editor;
            return this;
        }

        public EntityEditor<T> LabelFor(Expression<Func<T, object>> field, string label)
        {
            return this;
        }

        public EntityEditor<T> Order(IEnumerable<Expression<Func<T, object>>> order)
        {
            return this;
        }

        public EntityEditor<T> Remove(Expression<Func<T,object>> field)
        {
            var name = GetNameFromMemberExpression(field.Body);
            _mapper.Remove.Add(name);
            return this;
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
    }
}
