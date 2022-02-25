using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Grid
{
    public class DateCell<T> : ICellRenderer<T>
    {
        private readonly string _format;

        public DateCell(string format)
        {
            _format = format;
        }

        public Control Render(object value, GridColumn<T> column, T _)
        {
            if (value == null) return new Literal("-");
            
            var _value = Convert.ToDateTime(value);

            return new Literal(_value.ToString(_format));
        }
    }
}
