using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class MultiSelectEditor<T> : IFieldEditor<T>
    {
        private MultiSelectBox _selectBox;
        private Field _field;

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

            int[] _selectedIndexes = null;

            if (value is IEnumerable<int> selectedItems)
            {
                _selectedIndexes = selectedItems.Select(e => Array.IndexOf(_options.Select(f => f.Value).ToArray(), e)).Where(e => e != -1).ToArray();
            }

            _selectBox = new MultiSelectBox(_options, _selectedIndexes);
            _selectBox.SelectionChanged += (sender, args) => preview();

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            editorFieldInfo.SetValue(obj, _selectBox.SelectedOptions.Select(e => (int)((Option)e).Value).ToList());
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