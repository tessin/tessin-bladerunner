using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class MultiSelectBox : Control, IRefreshable
    {
        private readonly Div _divContainer;

        private CheckBox[] _checkBoxes;

        private readonly DumpContainer _checkBoxContainer;

        public MultiSelectBox(object[] options, int[] selectedIndexes = null, Action<MultiSelectBox> onSelectionChanged = null)
        {
            if (options != null && options.Length != 0)
            {
                Options = options;
            }
            if (selectedIndexes != null)
            {
                SelectedIndexes = selectedIndexes;
            }
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

        private object[] _options = new string[0];
        public object[] Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = (value ?? new string[0]);
                if (base.IsRendered)
                {
                    Update();
                }
            }
		}

        private int[] _selectedIndexes;
        public int[] SelectedIndexes
        {
            get
            {
                return _selectedIndexes ?? new int[] {};
            }
            set
            {
                _selectedIndexes = value;
                UpdateChecked();
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
                _checkBoxes = _options.Select((e,i) => new CheckBox(e.ToString(), isChecked: _selectedIndexes.Contains(i))).ToArray();
                AttachCheckBoxes();
                _checkBoxContainer.Content = Layout.Vertical(false, _checkBoxes);
            }
        }

        private void UpdateChecked()
        {
            if (_checkBoxes != null)
            {
                foreach (var pair in _checkBoxes.Select((c, i) => (c, i)))
                {
                    pair.c.Checked = _selectedIndexes?.Contains(pair.i) ?? false;
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
            if (base.IsRendered)
            {
                SelectionChanged?.Invoke(this, null);
                Updated?.Invoke(SelectedIndexes);
            }
        }

        public event RefreshEvent Updated;
    }
}
