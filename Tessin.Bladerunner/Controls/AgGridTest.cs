using System;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class AgGridTest : Control
    {
        static AgGridTest()
        {
            Util.HtmlHead.AddStyles(@"
.rag-green-outer {
    background-color:#4CAF50 !important;
}
.ag-theme-alpine {
    --ag-foreground-color: rgb(126, 46, 132);
    --ag-background-color: rgb(249, 245, 227);
    --ag-header-foreground-color: rgb(204, 245, 172);
    --ag-header-background-color: rgb(209, 64, 129);
    --ag-odd-row-background-color: rgb(0, 0, 0, 0.03);
    --ag-header-column-resize-handle-color: rgb(126, 46, 132);
    
    --ag-selected-row-background-color: rgb(0, 255, 0, 0.1);

    --ag-font-size: 17px;
    --ag-font-family: monospace;

    --ag-grid-size: 3px;
    --ag-list-item-height: 20px;
}
");
            Util.HtmlHead.AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/styles/ag-grid.min.css");
            Util.HtmlHead.AddCssLink("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/styles/ag-theme-balham.min.css");
            Util.HtmlHead.AddScriptFromUri("https://cdnjs.cloudflare.com/ajax/libs/ag-grid/25.1.0/ag-grid-community.min.js");
        }

        private string ShortLocalUid()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
        
        public AgGridTest()
        {
            var gridName = ShortLocalUid();
            var buttonName = ShortLocalUid();
            
            var container = new Div();
            container.SetClass("ag-grid-test ag-theme-alpine");
            container.HtmlElement.SetAttribute("style", "width:0x;");
            container.HtmlElement.SetAttribute("name", gridName);
            this.VisualTree.Add(container);
            
            Util.HtmlHead.AddScript($@"
setTimeout(function() {{
    var maxInt = 2147483647;
    const gridOptions = {{
    domLayout:'normal',
    suppressColumnVirtualisation: true,
    rowBuffer: maxInt, 
    columnDefs: [
        //{{ field: 'make', filter: 'agTextColumnFilter' }},
        {{ field: 'price', filter: 'agNumberColumnFilter', valueFormatter: (params) => {{
            return '£' + params.value;
        }} }},
        {{ field: 'model', filter: 'agTextColumnFilter' }},
        {{ field: 'price', filter: 'agNumberColumnFilter', cellRenderer: 
            function(params) {{
                if(!!params.data.buttonName)
                {{
                    var node = document.querySelector('[name=""' + params.data.buttonName + '""]');
                    node.parentNode.removeChild(node);
                    return node;
                }}            
                return '';
            }} 
        }}
    ],
    //rowClassRules: {{
    //  'rag-green-outer': function(params) {{ return params.data.price > 30000; }}
    //}},
    defaultColDef: {{ minWidth: 100, flex: 1, resizable: true, sortable: true }},
    onFirstDataRendered: function(event) {{
        const allColumnIds = [];
        const columns = event.columnApi.getAllColumns()        
        columns.forEach((column) => {{
            allColumnIds.push(column.getId());
        }});
        var width = (columns.length-1) * 10;
        event.columnApi.autoSizeColumns(allColumnIds, true);
        columns.forEach((column) => {{
            width += column.getActualWidth();   
        }});
        const gridDiv = document.querySelector('[name=""{gridName}""]');
        gridDiv.style.width = width + 'px';
    }},  
    rowData: [
        {{ make: 'Toyota', model: 'Celica', price: 35000, buttonName: '{buttonName}' }},
        {{ make: 'Ford', model: 'Mondeo', price: 32000 }},
        {{ make: 'Porsche', model: 'Boxter', price: 72000 }},   
        {{ make: 'Honda', model: 'Accord', price: 28000 }},
        {{ make: 'Nissan', model: 'Altima', price: 30000 }},
        {{ make: 'Chevrolet', model: 'Camaro', price: 40000 }},
        {{ make: 'Mazda', model: 'Miata', price: 25000 }},
        {{ make: 'Subaru', model: 'Impreza', price: 27000 }},
        {{ make: 'Tesla', model: 'Model 3', price: 49000 }},
        {{ make: 'Toyota', model: 'Prius', price: 31000 }},
        {{ make: 'Hyundai', model: 'Elantra', price: 22000 }},
        {{ make: 'Kia', model: 'Optima', price: 26000 }},
        {{ make: 'Volkswagen', model: 'Golf', price: 25000 }},
        {{ make: 'BMW', model: '3 Series', price: 45000 }},
        {{ make: 'Mercedes-Benz', model: 'C-Class', price: 48000 }},
        {{ make: 'Audi', model: 'A4', price: 42000 }},
        {{ make: 'Fiat', model: '500', price: 21000 }},
        {{ make: 'Lincoln', model: 'Navigator', price: 74000 }},
        {{ make: 'Cadillac', model: 'Escalade', price: 80000 }},
        {{ make: 'Jeep', model: 'Wrangler', price: 35000 }},
        {{ make: 'Dodge', model: 'Charger', price: 38000 }},
        {{ make: 'Chrysler', model: '300', price: 36000 }},
        {{ make: 'GMC', model: 'Sierra', price: 40000 }},
        {{ make: 'Ram', model: '1500', price: 37000 }},
        {{ make: 'Ford', model: 'F-150', price: 39000 }},
        {{ make: 'Aston Martin', model: 'DB11', price: 160000 }},
        {{ make: 'Bentley', model: 'Continental GT', price: 200000 }},
        {{ make: 'Rolls-Royce', model: 'Ghost', price: 300000 }},
        {{ make: 'Lamborghini', model: 'Huracan', price: 280000 }},
        {{ make: 'McLaren', model: '720S', price: 240000 }},
        {{ make: 'Ferrari', model: '488', price: 350000 }},
        {{ make: 'Maserati', model: 'Ghibli', price: 65000 }},
        {{ make: 'Porsche', model: '911', price: 120000 }},
        {{ make: 'Land Rover', model: 'Range Rover', price: 80000 }},
        {{ make: 'Lexus', model: 'LS', price: 75000 }},
        {{ make: 'Infiniti', model: 'Q50', price: 40000 }},
        {{ make: 'Acura', model: 'RLX', price: 50000 }},
        {{ make: 'Genesis', model: 'G80', price: 45000 }},
        {{ make: 'Buick', model: 'Enclave', price: 40000 }},
        {{ make: 'Lincoln', model: 'Aviator', price: 60000 }},
        {{ make: 'Cadillac', model: 'XT4', price: 35000 }},
        {{ make: 'Jeep', model: 'Grand Cherokee', price: 45000 }},
        {{ make: 'Dodge', model: 'Durango', price: 40000 }},
        {{ make: 'Chrysler', model: 'Pacifica', price: 35000 }},
        {{ make: 'GMC', model: 'Yukon', price: 50000 }},
        {{ make: 'Ram', model: '2500', price: 40000 }},
        {{ make: 'Ford', model: 'Expedition', price: 45000 }}        
    ]
}};

const gridDiv = document.querySelector('[name=""{gridName}""]');
new agGrid.Grid(gridDiv, gridOptions); 
}}, 500);
");
            var button = new Button("Hej", (_) =>
            {
                Util.ClearResults();
            });
            button.HtmlElement.SetAttribute("name", buttonName);

            var hiddenContainer = new Div(button)
            {
                Styles =
                {
                    ["display"] = "none"
                }
            };

            this.VisualTree.Add(hiddenContainer);


        }
    }
}