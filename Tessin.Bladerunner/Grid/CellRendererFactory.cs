using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Grid
{
    public class CellRendererFactory<T>
    {

        public ICellRenderer<T> Default(IContentFormatter formatter)
        {
            return new DefaultCell<T>(formatter);
        }

        public ICellRenderer<T> Text()
        {
            return new TextCell<T>();
        }

        public ICellRenderer<T> Number(string format = "N0")
        {
            return new NumberCell<T>(format);
        }

        public ICellRenderer<T> Percentage()
        {
            return new NumberCell<T>("P2");
        }

        public ICellRenderer<T> Date(string format = "yyyy-MM-dd")
        {
            return new DateCell<T>(format);
        }

    }
}
