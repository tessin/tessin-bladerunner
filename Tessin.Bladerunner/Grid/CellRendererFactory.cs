using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner.Grid
{
    public class CellRendererFactory<T> where T : new()
    {

        public ICellRenderer<T> Text()
        {
            return new TextCell<T>();
        }

        public ICellRenderer<T> Number(string format = "N0")
        {
            return new NumberCell<T>(format);
        }

        public ICellRenderer<T> Date(string format = "yyyy-MM-dd")
        {
            return new DateCell<T>(format);
        }

    }
}
