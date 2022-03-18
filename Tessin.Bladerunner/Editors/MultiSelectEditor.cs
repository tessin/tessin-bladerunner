using System;
using System.Collections;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class MultiSelectEditor<T> : IFieldEditor<T>
    {
        private MultiSelectBox _selectBox;
        private Field _field;

        private Type _elementType;

        private readonly Option[] _options;

        public MultiSelectEditor(Option[] options)
        {
            _options = options;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            object value = editorFieldInfo.GetValue(obj);

            _elementType = editorFieldInfo.Type.GenericTypeArguments.Single();

            int[] _selectedIndexes = null;

            if (value is IEnumerable selectedItems)
            {
                _selectedIndexes = selectedItems.Cast<object>().Select(e => Array.IndexOf(_options.Select(f => f.Value).ToArray(), e)).Where(e => e != -1).ToArray();
            }

            _selectBox = new MultiSelectBox(_options, _selectedIndexes);
            _selectBox.SelectionChanged += (sender, args) => preview();

            _selectBox.Enabled = editorFieldInfo.Enabled;

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            var col = _selectBox.SelectedOptions.Select(e => ((Option)e).Value).ToList();
            var mi = typeof(Utils).GetMethod(nameof(Utils.ConvertFactory));
            editorFieldInfo.SetValue(obj, mi.MakeGenericMethod(_elementType).Invoke(null, new object[] { col }));
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}