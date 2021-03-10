using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;
using Tessin.Bladerunner.Editors;

namespace Tessin.Bladerunner.Grid
{
    public interface ICellRenderer<T>
    {
        Control Render(object value, GridColumn<T> column);
    }
}
