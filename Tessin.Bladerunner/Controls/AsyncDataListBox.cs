using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Controls;
using Tessin.Bladerunner.Editors;

namespace Tessin.Bladerunner.Controls
{
    public class AsyncDataListBox : Div, ITextControl, IRefreshable
    {
        private readonly Func<string, Task<IEnumerable<Option>>> _queryOptions;

        private readonly Func<object, Task<Option>> _findOption;

        private readonly DebounceDispatcher _debounceDispatcher;

        private readonly Div _loadingDiv = new Div();

        private readonly TextBox _textBox = new TextBox(width: "250px");

        private readonly Control _dataList = new Control("datalist");

        private readonly Dictionary<string, Option> _knownLabels = new Dictionary<string, Option>();

        private readonly Dictionary<string, IEnumerable<Option>> _cache = new Dictionary<string, IEnumerable<Option>>();

        public AsyncDataListBox(Func<string, Task<IEnumerable<Option>>> queryOptions, Func<object,Task<Option>> findOption = null,  int debounceInterval = 250)
        {
            _debounceDispatcher = new DebounceDispatcher(debounceInterval);
            this.SetClass("async-data-list-box");
            _queryOptions = queryOptions;
            _findOption = findOption;

            _loadingDiv.SetClass("async-data-list-box--loading");

            _textBox.HtmlElement["list"] = _dataList.HtmlElement.ID;
            _textBox.HtmlElement["autocomplete"] = "off";
            _textBox.HtmlElement["spellcheck"] = "false";
            _textBox.TextInput += (sender, args) => OnTextInput();

            VisualTree.Add(_loadingDiv);
            VisualTree.Add(_textBox);
            VisualTree.Add(_dataList);
            _loadingDiv.SetVisibility(false);
        }

        private void OnTextInput()
        {
            if (!_skipUpdate)
            {
                if (!_updateSelectedValue())
                {
                    var key = _textBox.Text;
                    if (string.IsNullOrEmpty(key)) return;
                    if (_cache.ContainsKey(key))
                    {
                        _updateDataList(_cache[key]);
                    }
                    else
                    {
                        _debounceDispatcher.Debounce(() =>
                        {
                            _loadingDiv.SetVisibility(true);
                            _queryOptions(_textBox.Text).ContinueWith(e =>
                            {
                                _cache[key] = e.Result;
                                _loadingDiv.SetVisibility(false);
                                _updateDataList(e.Result);
                            });
                        });
                    }
                }
            }
        }

        protected override Control FocusableControl => _textBox;

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public Option SelectedOption { get; private set; }

        public object SelectedValue => SelectedOption?.Value;

        public object SelectedLabel => SelectedOption?.Label;

        public string Width
        {
            get
            {
                return _textBox.Width;
            }
            set
            {
                _textBox.Width = value;
            }
        }

        public string Text
        {
            get
            {
                return _textBox.Text;
            }
            set
            {
                _textBox.Text = value;
            }
        }

        public event EventHandler TextInput
        {
            add
            {
                _textBox.TextInput += value;
            }
            remove
            {
                _textBox.TextInput -= value;
            }
        }

        private void _updateDataList(IEnumerable<Option> options)
        {
            foreach (var option in options)
            {
                _trackOption(option);
            }
            _dataList.HtmlElement.InnerHtml = string.Join("", options.Select(e => $@"<option>{e}</option>").ToArray());
            _updateSelectedValue();
        }

        private void _trackOption(Option option)
        {
            var key = option.Label.ToLower();
            if (!_knownLabels.ContainsKey(key))
            {
                _knownLabels.Add(key, option);
            }
        }

        private bool _updateSelectedValue()
        {
            var key = Text.ToLower();
            if (key != "" && _knownLabels.ContainsKey(key))
            {
                _addValidatedStyle();
                SelectedOption = _knownLabels[key];
                Updated?.Invoke(SelectedOption);
                return true;
            }

            _clearOption();

            return false;
        }

        private void _clearOption()
        {
            _removeValidatedStyle();
            if (SelectedOption != null)
            {
                SelectedOption = null;
                Updated?.Invoke(null);
            }
        }

        private bool _skipUpdate = false;

        public async Task SetValueAsync(object value)
        {
            _skipUpdate = true;
            if (_findOption != null)
            {
                var option = await _findOption(value);
                if (option != null)
                {
                    _trackOption(option);
                    SelectedOption = option;
                    _addValidatedStyle();
                    Updated?.Invoke(SelectedOption);
                    Text = option.Label;
                }
                else
                {
                    _clearOption();
                }
            }
            _skipUpdate = false;
        }

        private void _addValidatedStyle()
        {
            //_textBox.AddClass("validated");
            _textBox.Styles["text-decoration"] = "underline";
            _dataList.HtmlElement.InnerHtml = "";
        }

        private void _removeValidatedStyle()
        {
            //_textBox.RemoveClass("validated");
            _textBox.Styles["text-decoration"] = null;
        }

        public event RefreshEvent Updated;
    }
}
