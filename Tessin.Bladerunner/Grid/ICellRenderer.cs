using LINQPad.Controls;

namespace Tessin.Bladerunner.Grid
{
    public interface ICellRenderer<T>
    {
        Control Render(object value, GridColumn<T> column, T row);
    }
}
