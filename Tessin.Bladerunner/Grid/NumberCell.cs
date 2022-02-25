using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Grid
{
    public class NumberCell<T> : ICellRenderer<T>
    {
        private readonly string _format;

        public NumberCell(string format)
        {
            _format = format;
        }

        public Control Render(object value, GridColumn<T> column, T _)
        {
            if (value == null) return new Literal("n/a");

            var _value = Convert.ToDouble(value);

            if (_value == 0) return new Literal("-");

            return new Literal(_value.ToString(_format));
        }
    }
}
