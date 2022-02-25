using LINQPad.Controls;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Grid
{
    public class DefaultCell<T> : ICellRenderer<T>
    {
        private readonly IContentFormatter _formatter;

        public DefaultCell(IContentFormatter formatter)
        {
            _formatter = formatter;
        }

        public Control Render(object value, GridColumn<T> column, T _)
        {
            return _formatter.Format(value);
        }
    }
}