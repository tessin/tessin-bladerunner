using System;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Grid
{
    public static class CellRendererHelper
    {
        public static ICellRenderer<T> Create<T>(Func<object, GridColumn<T>, T, Control> renderer)
        {
            return new FuncCellRenderer<T>(renderer);
        }
    }
    
    public class FuncCellRenderer<T> : ICellRenderer<T>
    {
        private readonly Func<object, GridColumn<T>, T, Control> _renderer;

        public FuncCellRenderer(Func<object, GridColumn<T>, T, Control> renderer)
        {
            _renderer = renderer;
        }

        public Control Render(object value, GridColumn<T> column, T row)
        {
            return _renderer(value, column, row);
        }
    }
}