using System;
using System.Collections.Generic;
using System.Text;
using LINQPad;
using LINQPad.Controls;

//todo: debouncer

namespace Tessin.Bladerunner
{
    public class RefreshContainer : DumpContainer
    {
        private readonly Func<object> _onRefresh;

        public RefreshContainer(Control[] controls, Func<object> onRefresh)
        {
            _onRefresh = onRefresh;

            foreach (var control in controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.TextInput += (_, __) =>
                    {
                        _Refresh();
                    };
                }
                if (control is CheckBox checkBox)
                {
                    checkBox.Click += (_, __) =>
                    {
                        _Refresh();
                    };
                }
                if (control is DataListBox dataListBox)
                {
                    dataListBox.TextInput += (_, __) =>
                    {
                        _Refresh();
                    };
                }
                if (control is TextArea textArea)
                {
                    textArea.TextInput += (_, __) =>
                    {
                        _Refresh();
                    };
                }
                if (control is FilePicker filePicker)
                {
                    filePicker.TextInput += (_, __) =>
                    {
                        _Refresh();
                    };
                }
            }
            _Refresh();
        }

        private void _Refresh()
        {
            this.Content = _onRefresh();
        }
	}
}
