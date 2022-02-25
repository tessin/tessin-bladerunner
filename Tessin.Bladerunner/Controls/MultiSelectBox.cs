using LINQPad;
using LINQPad.Controls;
using System;
using System.Linq;

namespace Tessin.Bladerunner.Controls
{
    public class MultiSelectBox : Control, IRefreshable
    {
        private readonly Div _divContainer;

        private CheckBox[] _checkBoxes;

        private readonly DumpContainer _checkBoxContainer;

        private int[] _selectedIndexes;

        public MultiSelectBox(Option[] options, int[] selectedIndexes = null, Action<MultiSelectBox> onSelectionChanged = null)
        {
            if (options != null && options.Length != 0)
            {
                Options = options;
            }

            _selectedIndexes = selectedIndexes;

            if (onSelectionChanged != null)
            {
                SelectionChanged += delegate
                {
                    onSelectionChanged(this);
                };
            }

            _checkBoxContainer = new DumpContainer();
            _divContainer = new Div(_checkBoxContainer).SetClass("multi-select-box");

            VisualTree.Add(_divContainer);
        }

        private Option[] _options = Array.Empty<Option>();
        public Option[] Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = (value ?? Array.Empty<Option>());
                if (base.IsRendered)
                {
                    Update();
                }
            }
        }

        public int[] SelectedIndexes
        {
            get
            {
                //return _checkBoxes?.Select((e, i) => (e, i)).Where(x => x.e.Checked).Select(x => x.i).ToArray() ?? Array.Empty<int>();
                return _selectedIndexes;
            }
            set
            {
                _selectedIndexes = value;
                UpdateChecked(value ?? new int[] { });
            }
        }

        public object[] SelectedOptions => (from i in SelectedIndexes select Options[i]).ToArray();

        public event EventHandler SelectionChanged;

        protected override void OnRendering(EventArgs e)
        {
            Update();
            base.OnRendering(e);
        }

        private void Update()
        {
            if (Options != null)
            {
                DetachCheckBoxes();
                _checkBoxes = _options.Select((e, i) => new CheckBox(e.Label, isChecked: _selectedIndexes.Contains(i))).ToArray();
                _checkBoxContainer.Content = Layout.Gap(false).Vertical(_checkBoxes);
                AttachCheckBoxes();
            }
        }

        private void UpdateChecked(int[] selectedIndexes)
        {
            if (_checkBoxes != null)
            {
                foreach (var pair in _checkBoxes.Select((c, i) => (c, i)))
                {
                    pair.c.Checked = selectedIndexes?.Contains(pair.i) ?? false;
                }
            }
        }

        private void DetachCheckBoxes()
        {
            if (_checkBoxes != null)
            {
                foreach (var checkBox in _checkBoxes)
                {
                    checkBox.Click -= CheckBoxHandler;
                }
            }
        }

        private void AttachCheckBoxes()
        {
            if (_checkBoxes != null)
            {
                foreach (var checkBox in _checkBoxes)
                {
                    checkBox.Click += CheckBoxHandler;
                }
            }
        }

        private void CheckBoxHandler(object sender, EventArgs e)
        {
            _selectedIndexes =
                _checkBoxes?.Select((e, i) => (e, i)).Where(x => x.e.Checked).Select(x => x.i).ToArray() ??
                Array.Empty<int>();
            SelectionChanged?.Invoke(this, null);
            Updated?.Invoke(SelectedOptions);
        }

        public event RefreshEvent Updated;
    }
}
