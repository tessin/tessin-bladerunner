using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

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
