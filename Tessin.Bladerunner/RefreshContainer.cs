﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner
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

    public class RefreshContainer : DumpContainer
    {
        private readonly Func<AnyTask> _onRefreshAsync;

        private bool _first = true;

        private readonly DebounceDispatcher _debounceDispatcher;

        public RefreshContainer(object[] controls, Func<object> onRefreshAsync, int debounceInterval = 250) 
            : this(controls, () => Task.Run(async () => await Task.FromResult(onRefreshAsync())), debounceInterval)
        {
        }

        public RefreshContainer(object[] controls, Func<AnyTask> onRefreshAsync, int debounceInterval = 250)
        {
            _debounceDispatcher = new DebounceDispatcher(debounceInterval);
            _onRefreshAsync = onRefreshAsync;

            foreach (var control in controls)
            {
                if (control is IRefreshable refreshable)
                {
                    refreshable.Updated +=  (_) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is TextBox textBox)
                {
                    textBox.TextInput +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is SearchBox searchBox)
                {
                    searchBox.TextInput +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Click +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is DataListBox dataListBox)
                {
                    dataListBox.TextInput +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is TextArea textArea)
                {
                    textArea.TextInput +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is FilePicker filePicker)
                {
                    filePicker.TextInput +=  (_, __) =>
                    {
                         _Refresh();
                    };
                }
                else if (control is SelectBox selectBox)
                {
                    selectBox.SelectionChanged += (_, __) =>
                    {
                         _Refresh();
                    };
                }
            }
            _Refresh();
        }

        private void _Refresh()
        {
            //lock (this)
            {
                if (_first)
                {
                    _first = false;
                    Task.Run(() => _onRefreshAsync().Result).ContinueWith(e =>
                    {
                        this.Content = e.Result;
                    });
                }
                else
                {
                    _debounceDispatcher.Debounce(() =>
                    {
                        Task.Run(() => _onRefreshAsync().Result).ContinueWith(e =>
                        {
                            this.Content = e.Result;
                        });
                    });
                }
            }
        }
	}
}
