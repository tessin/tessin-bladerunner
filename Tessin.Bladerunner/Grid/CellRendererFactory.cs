using System;
using LINQPad.Controls;
using Tessin.Bladerunner.Controls;
using Literal = Tessin.Bladerunner.Controls.Literal;

namespace Tessin.Bladerunner.Grid
{
    public class CellRendererFactory<T>
    {
        private readonly IContentFormatter _formatter;

        public CellRendererFactory(IContentFormatter formatter)
        {
            _formatter = formatter;
        }

        public ICellRenderer<T> Default(IContentFormatter formatter = null)
        {
            return new DefaultCell<T>(formatter ?? _formatter);
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

        public ICellRenderer<T> Link(Action<T> onClick)
        {
            return new LinkCell<T>(onClick);
        }
        
        public ICellRenderer<T> Custom(Func<object, GridColumn<T>, T, Control> renderer)
        {
            return CellRendererHelper.Create(renderer);
        }
        
        public ICellRenderer<T> Custom(Func<object, Control> renderer)
        {
            return CellRendererHelper.Create<T>((v,_,_) => renderer(v));
        }

        public ICellRenderer<T> Custom(Func<object, string> renderer)
        {
            return CellRendererHelper.Create<T>((v,_,_) => new Literal(renderer(v)));
        }
    }
}
