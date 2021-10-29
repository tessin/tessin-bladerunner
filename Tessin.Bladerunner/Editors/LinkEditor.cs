using System;
using System.Collections.Generic;
using System.Reflection;

using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Editors
{
    public class LinkEditor<T> : IFieldEditor<T>
    {
        private readonly Action<T> _onClick;

        private readonly Func<T, string> _fetchUrl;

        public LinkEditor(Action<T> onClick)
        {
            _onClick = onClick;
        }

        public LinkEditor(Func<T, string> fetchUrl)
        {
            _fetchUrl = fetchUrl;
        }


        public LinkEditor()
        {
        }

        public void Update(object value)
        {

        }

        public object Render(T obj, EditorField<T> editorFieldInfo, Action preview)
        {
            var label = Convert.ToString(editorFieldInfo.GetValue(obj));

            Control link;

            if (_fetchUrl != null)
            {
                var url = _fetchUrl(obj);
                link = new Controls.Hyperlink(label, url);
            }
            else if(_onClick != null)
            {
                link = new Controls.Hyperlink(label, (_) => _onClick(obj));
            }
            else
            {
                link = new Controls.Hyperlink(label, label);
            }

            link.HtmlElement.SetAttribute("class", "entity-editor-link");

            return new Field(editorFieldInfo.Label, link, editorFieldInfo.Description, editorFieldInfo.Helper);
        }

        public void Save(T obj, EditorField<T> editorFieldInfo)
        {
            //ignore
        }

        public bool Validate(T obj, EditorField<T> editorFieldInfo)
        {
            return true;
        }

        public void SetVisibility(bool value)
        {
            //todo
        }
    }
}