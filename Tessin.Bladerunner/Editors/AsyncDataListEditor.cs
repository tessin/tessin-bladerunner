using LINQPad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class AsyncDataListEditor<T> : IFieldEditor<T>
    {
        private readonly Func<string, Task<IEnumerable<Option>>> _queryOptions;
        private readonly Func<object, Task<Option>> _findOption;
        private AsyncDataListBox _selectBox;
        private Field _field;

        public AsyncDataListEditor(
            Func<string, Task<IEnumerable<Option>>> queryOptions,
            Func<object, Task<Option>> findOption = null)
        {
            _queryOptions = queryOptions;
            _findOption = findOption;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            object value = editorFieldInfo.GetValue(obj);

            _selectBox = new AsyncDataListBox(_queryOptions, _findOption)
            {
            };

            if (value != null)
            {
                _selectBox.SetValueAsync(value).GetAwaiter().GetResult();
            }

            _selectBox.Updated += (_) => preview();

            return _field = new Field(editorFieldInfo.Label, _selectBox, editorFieldInfo.Description, editorFieldInfo.Helper, editorFieldInfo.Required);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            editorFieldInfo.SetValue(obj, _selectBox.SelectedOption?.Value);
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            void SetError(string message)
            {
                _selectBox.Styles["border-color"] = "#aa0000";
                _field.SetError(message);
            }

            void ClearError()
            {
                _selectBox.Styles["border-color"] = null;
                _field.SetError("");
            }

            var option = _selectBox.SelectedOption;

            (bool, string)? error = editorFieldInfo.Validators
                .Select(e => e(option?.Value))
                .Where(e => !e.Item1)
                .Select(e => ((bool, string)?)e)
                .FirstOrDefault();

            if (error is { Item1: false })
            {
                SetError(error.Value.Item2);
                return false;
            }
            else
            {
                ClearError();
            }

            return true;
        }

        public void SetVisibility(bool value)
        {
            _field.SetVisibility(value);
        }
    }
}