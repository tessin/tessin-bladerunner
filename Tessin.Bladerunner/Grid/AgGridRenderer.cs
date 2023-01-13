using System;
using System.Linq;
using LINQPad;
using LINQPad.Controls;

/*
 * Todo:
 * - Styling
 * - Recalculate totals when filtering
 * - Remember the last table width to reduce jank
 * - Center alignment
 * - Date formatting
 */

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
            
            var columns = g._columns.Values.Where(e => !e.Removed /* e.CellRenderer != null */)
                .OrderBy(e => e.Order).ToList();

            var hidden = new Div()
            {
                Styles =
                {
                    ["display"] = "none"
                }
            };

            var grid = new Div();
            grid.SetClass("ag-theme-alpine");
            grid.HtmlElement.SetAttribute("style", "width:0x;");
            grid.HtmlElement.SetAttribute("name", gridId);

            string RenderColumnDefs()
            {
                return string.Join($",{Environment.NewLine}", columns.Select(RenderColumnDef).ToArray());
            }

            string AddHiddenControl(Control control)
            {
                var controlId = Utils.ShortLocalUid();
                control.HtmlElement.SetAttribute("name", controlId);
                hidden.Children.Add(control);
                return controlId;
            }
            
            bool IsEmpty(GridColumn<T> column)
            {
                bool _IsEmpty(object val)
                {
                    return val switch
                    {
                        null or "" => true,
                        string strContent => false,
                        long and 0 => true,
                        long longContent => false,
                        int and 0 => true,
                        int intContent => false,
                        double and 0 => true,
                        double doubleContent => false,
                        decimal and 0 => true,
                        decimal decimalContent => false,
                        bool boolContent => !boolContent,
                        DateTime dateContent => false,
                        DateTimeOffset dateTimeOffset => false,
                        Guid guid => guid == Guid.Empty,
                        Control control => false,
                        DumpContainer dumpContainer => false,
                        _ => false
                    };
                }
                
                return g._rows.All(e => _IsEmpty(column.GetValue(e)));
            }
            
            string RenderColumnDef(GridColumn<T> column, int index)
            {
                string fieldId = $"field_{index}";
                string filter = null;
                string cellRenderer = "null";
                string headerName = column.Name.IsMatch("_+") ? "" : column.Label;
                string type = "";
                bool hide = false;
                string valueFormatter = "null";

                if (column.CellAlignment == CellAlignment.Right)
                {
                    type = "'rightAligned'";
                }
                else if (column.CellAlignment == CellAlignment.Center)
                {
                    //not implemented
                }
                
                if (g._removeEmptyColumns)
                {
                    hide = IsEmpty(column);
                }
                
                if (column.Type == typeof(Control) || column.Type.IsSubclassOf(typeof(Control)))
                {
                    cellRenderer = $"HiddenControlCellRenderer('{fieldId}')";
                }
                else if(column.Type == typeof(bool))
                {
                    cellRenderer = $"HiddenControlCellRenderer('{fieldId}')";
                }
                else if (column.Type.IsNumeric())
                {
                    filter = "agNumberColumnFilter";
                    valueFormatter = "DefaultNumberFormatter";
                }
                else if(column.Type.IsDate())
                {
                    filter = "agDateColumnFilter";
                }
                else if(column.Type == typeof(string))
                {
                    filter = "agTextColumnFilter";
                }

                return $"{{field: '{fieldId}', hide: {hide.ToJSVal()}, headerName: '{headerName}', filter: {filter.ToJSVal()}, cellRenderer: {cellRenderer}, type: [{type}], valueFormatter: {valueFormatter}}}";
            }

            string RenderCell(int index, GridColumn<T> column, T e)
            {
                string fieldId = $"field_{index}";
                object value = column.GetValue(e);
                
                if (value is Control control)
                {
                    value = AddHiddenControl(control);
                }
                else if (value is bool boolValue)
                {
                    value = AddHiddenControl(boolValue ? new Controls.Icon(Icons.CheckBold, theme: Theme.Success) : Controls.Icon.Empty());
                }
                
                return $"{fieldId}: {value?.ToJSVal() ?? "null"}";
            }

            string RenderRow(T e)
            {
                bool highlighted = g._highlightRowPredicate?.Invoke(e) ?? false;
                return
                    $"{{{string.Join(", ", columns.Select((f, i) => RenderCell(i, f, e)).ToArray())}, highlighted: {highlighted.ToJSVal()}}}";
            }

            string RenderData()
            {
                return string.Join($",{Environment.NewLine}", g._rows.Select(RenderRow).ToArray());
            }
            
            string RenderTotals()
            {
                if (columns.Any(e => e.SummaryMethod != null))
                {
                    return "{" + string.Join(", ", columns.Select((f, i) => RenderTotal(i, f)).ToArray()) + "}";
                }
                return "";
            }
            
            string RenderTotal(int index, GridColumn<T> column)
            {
                string fieldId = $"field_{index}";
                object value = null;
                if (column.SummaryMethod != null)
                {
                    value = column.SummaryMethod(g._rows);
                }
                return $"{fieldId}: {value.ToJSVal()}";
            }
            
               Util.HtmlHead.AddScript($@"
setTimeout(function() {{
    function HiddenControlCellRenderer(fieldId) {{
        return function(params) {{
            if(!!params.data[fieldId]) {{
                var node = document.querySelector('[name=""' + params.data[fieldId] + '""]');
                node.parentNode.removeChild(node);
                return node;
            }}            
            return '';
        }}
    }}

    function DefaultNumberFormatter(params) {{
       if(params.value === null || params.value === undefined) {{
            return null;
        }}
        if(Number.isInteger(params.value)) {{
            return params.value.toLocaleString('en-US', {{ minimumFractionDigits: 0 }}).replace(/[,]/, ' ');
        }} else {{
           return params.value.toLocaleString('en-US', {{ minimumFractionDigits: 0, maximumFractionDigits: 4 }}).replace(/[,]/, ' ');
        }}         
    }}
    
    var maxInt = 2147483647;
    const gridOptions = {{
    domLayout:'normal',
    suppressCellSelection: true,
    suppressColumnVirtualisation: true, //can be false if there's nothing in the hidden layer
    rowBuffer: maxInt, 
    defaultColDef: {{ minWidth: 100, flex: 1, resizable: true, sortable: true }},
    onFirstDataRendered: function(event) {{
        const allColumnIds = [];
        const columns = event.columnApi.getAllColumns().filter(function(e) {{ return !e.getColDef().hide }});        
        columns.forEach((column) => {{
            allColumnIds.push(column.getId());
        }});
        var width = (columns.length-1) * 5;
        event.columnApi.autoSizeColumns(allColumnIds, true);
        columns.forEach((column) => {{
            width += column.getActualWidth();   
        }});
        const gridDiv = document.querySelector('[name=""{gridId}""]');
        gridDiv.style.width = width + 'px';
    }},  
    columnDefs: [
        {RenderColumnDefs()}
    ],
    rowData: [
        {RenderData()}       
    ],
    pinnedBottomRowData: [
        {RenderTotals()}
    ],
    rowClassRules: {{
      'ag-highlighted': function(params) {{ return params.data.highlighted; }}
    }},
}};

const gridDiv = document.querySelector('[name=""{gridId}""]');
new agGrid.Grid(gridDiv, gridOptions); 
}}, 500);
");
            
            return new Div(grid, hidden);
        }
    }
}