using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tessin.Bladerunner.Editors
{
    public class FieldEditorMapper<T>
    {
        public HashSet<string> Remove = new HashSet<string>();

        public Dictionary<string, IFieldEditor<T>> FieldMappings = new Dictionary<string, IFieldEditor<T>>();

        public Dictionary<Type, IFieldEditor<T>> TypeMappings = new Dictionary<Type, IFieldEditor<T>>();

        public FieldEditorMapper()
        {
        }

        public IEnumerable<IFieldEditor<T>> Map(IEnumerable<FieldInfo> fields)
        {
            var factory = new EditorFactory<T>();

            foreach (var field in fields)
            {
                if (Remove.Contains(field.Name))
                {
                    yield return factory.Hidden();
                }
                else if (FieldMappings.ContainsKey(field.Name))
                {
                    yield return FieldMappings[field.Name];
                }
                else if (TypeMappings.ContainsKey(field.FieldType))
                {
                    yield return TypeMappings[field.FieldType];
                }
                else if (field.Name.EndsWith("Id") && (field.FieldType == typeof(Guid) || field.FieldType == typeof(int)))
                {
                    yield return factory.Literal();
                }
                else if (field.FieldType == typeof(bool) || field.FieldType == typeof(bool?))
                {
                    yield return factory.Bool();
                }
                else if (field.FieldType == typeof(string))
                {
                    yield return factory.Text();
                }
                else if (field.FieldType.IsNumeric())
                {
                    yield return factory.Number();
                }
                else if (field.FieldType.IsDate())
                {
                    yield return factory.Date();
                }
                else
                {
                    yield return factory.Literal();
                }
            }
        }
    }
}