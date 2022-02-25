using LINQPad;
using System;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Controls
{
    public static class PaginatorHelper
    {
        public static Paginator<T> Create<T>(IEnumerable<T> records, int pageSize, Func<IEnumerable<T>, object> render)
        {
            return new Paginator<T>(records, pageSize, render);
        }
    }

    public class Paginator<T> : DumpContainer
    {
        private readonly int _pageSize;

        private readonly Button _loadButton;

        private readonly List<T> _loadedRecords;

        private readonly Func<IEnumerable<T>, object> _render;

        private readonly IEnumerator<T> _enumerator;

        private bool _finished = false;

        public Paginator(IEnumerable<T> records, int pageSize, Func<IEnumerable<T>, object> render)
        {
            _pageSize = pageSize;
            _render = render;
            _loadedRecords = new List<T>();
            _enumerator = records.GetEnumerator();

            _loadButton = new Button("Load More", (_) =>
            {
                Fetch();
                Update();
            });


            Fetch();
            Update();

            this.ContentChanged += (sender, args) =>
            {
                Util.InvokeScript(false, "ScrollTo", _loadButton.HtmlElement.ID);
            };
        }

        public void Fetch()
        {
            for (int i = 0; i < _pageSize; i++)
            {
                if (!_enumerator.MoveNext())
                {
                    _finished = true;
                    return;
                }
                _loadedRecords.Add(_enumerator.Current);
            }
        }

        public void Update()
        {
            this.Content = Layout.Vertical(_render(_loadedRecords), !_finished ? _loadButton : null);
        }
    }

}
