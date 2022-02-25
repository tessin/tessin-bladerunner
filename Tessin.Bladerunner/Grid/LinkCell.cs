using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Grid
{
    public class LinkCell<T> : ICellRenderer<T>
    {
        private readonly Action<T> _onClick;

        public LinkCell(Action<T> onClick)
        {
            _onClick = onClick;
        }

        public Control Render(object value, GridColumn<T> column, T row)
        {
            var _value = Convert.ToString(value);
            return new Hyperlink(_value, _ => _onClick(row));
        }
    }
}
