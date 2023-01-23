using LINQPad;
using LINQPad.Controls;
using System;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public delegate void RefreshEvent(object value);

    public interface IRefreshable
    {
        event RefreshEvent Updated;
    }

    public class RefreshValue<T> : IRefreshable
    {
        private T _value;

        public RefreshValue(T value)
        {
            Value = value;
        }

        public RefreshValue()
        {

        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Updated?.Invoke(value);
            }
        }

        public event RefreshEvent Updated;
    }

    public class RefreshPanel : Div
    {
        private readonly AnyTaskFactory _taskFactory;

        private readonly bool _addPadding;

        private readonly DebounceDispatcher _debounceDispatcher;
        private readonly DumpContainer _dumpContainer;

        public RefreshPanel(object[] controls, Func<Task<object>> onRefreshAsync, int debounceInterval = 250, bool addPadding = true)
            : this(controls, AnyTask.Factory<object>(onRefreshAsync), debounceInterval, addPadding)
        {

        }

        public RefreshPanel(object[] controls, Func<object> onRefreshAsync, int debounceInterval = 250, bool addPadding = true)
            : this(controls, AnyTask.Factory<object>(() => Task.FromResult(onRefreshAsync())), debounceInterval, addPadding)
        {

        }

        private RefreshPanel(object[] controls, AnyTaskFactory taskFactory, int debounceInterval = 250, bool addPadding = true)
        {
            _addPadding = addPadding;

            _debounceDispatcher = new DebounceDispatcher(debounceInterval);
            _taskFactory = taskFactory;

            this.HtmlElement.SetAttribute("class", "blade-content");

            this._dumpContainer = new DumpContainer();
            
            VisualTree.Add(this._dumpContainer);

            _Bind(controls);
            _Refresh();
        }

        private void _Bind(object[] controls)
        {
            if (controls != null)
            {
                foreach (var control in controls)
                {
                    if (control is IRefreshable refreshable)
                    {
                        refreshable.Updated += (_) => { _Refresh(); };
                    }
                    else if (control is LINQPad.Controls.ITextControl textBox)
                    {
                        textBox.TextInput += (_, __) => { _Refresh(); };
                    }
                    else if (control is LINQPad.Controls.CheckBox checkBox)
                    {
                        checkBox.Click += (_, __) => { _Refresh(); };
                    }
                    else if (control is LINQPad.Controls.FilePicker filePicker)
                    {
                        filePicker.TextInput += (_, __) => { _Refresh(); };
                    }
                    else if (control is LINQPad.Controls.SelectBox selectBox)
                    {
                        selectBox.SelectionChanged += (_, __) => { _Refresh(); };
                    }
                }
            }
        }

        private static Control _Element(string name, string @class, string content)
        {
            var el = new Control(name)
            {
                HtmlElement =
                {
                    InnerText = content
                }
            };
            if (@class != null)
            {
                el.HtmlElement.SetAttribute("class", @class);
            }
            return el;
        }

        private void _Refresh()
        {
            this._dumpContainer.Content = _Element("div", "loading", "Loading...");
            _debounceDispatcher.Debounce(() =>
            {
                try
                {
                    Task.Run(() => _taskFactory.Run().Result).ContinueWith(async e =>
                    {
                        if (_addPadding)
                        {
                            await ControlExtensions.AddPadding(this._dumpContainer, e.Result.Render());
                        }
                        else
                        {
                            this._dumpContainer.Content = e.Result.Render();
                        }
                    });
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }
    }
}
