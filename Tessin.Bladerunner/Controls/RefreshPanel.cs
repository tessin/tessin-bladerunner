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

    public class RefreshPanel : DumpContainer
    {
        private readonly AnyTaskFactory _taskFactory;

        //private bool _first = true;

        private readonly bool _addPadding = false;

        private readonly DebounceDispatcher _debounceDispatcher;

        public RefreshPanel(object[] controls, Func<Task<object>> onRefreshAsync, int debounceInterval = 250, bool addPadding = true)
            : this(controls, AnyTask.Factory<object>(onRefreshAsync), debounceInterval, addPadding)
        {

        }

        public RefreshPanel(object[] controls, Func<object> onRefreshAsync, int debounceInterval = 250, bool addPadding = true)
            : this(controls, AnyTask.Factory<object>(() => Task.FromResult(onRefreshAsync())), debounceInterval, addPadding)
        {

        }

        public RefreshPanel(object[] controls, AnyTaskFactory taskFactory, int debounceInterval = 250, bool addPadding = true)
        {
            _addPadding = addPadding;

            _debounceDispatcher = new DebounceDispatcher(debounceInterval);
            _taskFactory = taskFactory;

            this.Style = "width:100%;"; //todo: replace with class

            if (controls != null)
            {
                foreach (var control in controls)
                {
                    if (control is IRefreshable refreshable)
                    {
                        refreshable.Updated += (_) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is LINQPad.Controls.ITextControl textBox)
                    {
                        textBox.TextInput += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is SearchBox searchBox)
                    {
                        searchBox.TextInput += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is LINQPad.Controls.CheckBox checkBox)
                    {
                        checkBox.Click += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is LINQPad.Controls.DataListBox dataListBox)
                    {
                        dataListBox.TextInput += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is LINQPad.Controls.TextArea textArea)
                    {
                        textArea.TextInput += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                    else if (control is LINQPad.Controls.FilePicker filePicker)
                    {
                        filePicker.TextInput += (_, __) =>
                       {
                           _Refresh();
                       };
                    }
                    else if (control is LINQPad.Controls.SelectBox selectBox)
                    {
                        selectBox.SelectionChanged += (_, __) =>
                        {
                            _Refresh();
                        };
                    }
                }
            }
            _Refresh();
        }

        private static Control Element(string name, string @class, string content)
        {
            var el = new Control(name);
            el.HtmlElement.InnerText = content;
            if (@class != null)
            {
                el.HtmlElement.SetAttribute("class", @class);
            }
            return el;
        }

        private void _Refresh()
        {
            //lock (this)
            {
                this.Content = Element("div", "loading", "Loading...");

                //if (_first)
                //{
                //    _first = false;
                //    await Task.Run(() => _taskFactory.Run().Result).ContinueWith(async e =>
                //    {
                //        if (_addPadding)
                //        {
                //            await ControlExtensions.AddPadding(this, e.Result);
                //        }
                //        else
                //        {
                //            this.Content = e.Result;
                //        }
                //    });
                //}
                //else
                {
                    _debounceDispatcher.Debounce(() =>
                    {
                        try
                        {
                            Task.Run(() => _taskFactory.Run().Result).ContinueWith(async e =>
                            {
                                if (_addPadding)
                                {
                                    await ControlExtensions.AddPadding(this, e.Result.Render());
                                }
                                else
                                {
                                    this.Content = e.Result.Render();
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
    }
}
