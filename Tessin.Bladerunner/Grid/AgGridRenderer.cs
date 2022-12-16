using System.Linq;
using LINQPad;

namespace Tessin.Bladerunner.Grid
{
    public class AgGridRenderer<T> : IGridRenderer<T>
    {
        static AgGridRenderer()
        {
            Util.HtmlHead.AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/styles/ag-grid.min.css");
            Util.HtmlHead.AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/styles/ag-theme-balham.min.css");
            Util.HtmlHead.AddScriptFromUri("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/ag-grid-community.min.js");
        }
        
        public object Render(EntityGrid<T> g)
        {
            if (!g._rows.Any()) return g._emptyContent;
            
            var gridId = Utils.ShortLocalUid();
        }
    }
}