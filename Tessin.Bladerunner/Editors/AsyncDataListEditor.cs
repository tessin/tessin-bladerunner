using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class AsyncDataListEditor<T> : IFieldEditor<T>
    {
        private readonly Func<string, Task<IEnumerable<Option>>> _optionsQuery;
        private AsyncDataListBox _selectBox;
        private Field _field;

        public AsyncDataListEditor(Func<string, Task<IEnumerable<Option>>> optionsQuery)
        {
            _optionsQuery = optionsQuery;
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            object value = editorFieldInfo.GetValue(obj);

            //var selectedOption = _options.Where(e => e.Value.Equals(value)).Select(e => e.Label).FirstOrDefault();

            _selectBox = new AsyncDataListBox(_optionsQuery, null)
            {
                //SelectedOption = selectedOption
            };

            _selectBox.Updated += (_) => preview();

            //_selectBox.HtmlElement.SetAttribute("class", "entity-editor-async-data-list");

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            editorFieldInfo.SetValue(obj, _selectBox.SelectedOption);
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