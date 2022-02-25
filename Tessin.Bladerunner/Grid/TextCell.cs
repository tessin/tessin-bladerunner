using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Grid
{
    public class TextCell<T> : ICellRenderer<T>
    {
        public Control Render(object value, GridColumn<T> column, T _)
        {
            var _value = Convert.ToString(value);
            return new Literal(_value);
        }
    }
}
