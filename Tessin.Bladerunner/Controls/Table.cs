using LINQPad.Controls;
using System.Collections.Generic;

namespace Tessin.Bladerunner.Controls
{
    public class Table : LINQPad.Controls.Table
    {
        public Table(bool noBorders, string cellPaddingStyle = "", string cellTextAlign = "left", string cellVerticalAlign = "top") : base(noBorders, cellPaddingStyle, cellTextAlign, cellVerticalAlign)
        {
        }

        public Table(string tableBorderStyle = "", string cellBorderStyle = "", string cellPaddingStyle = "", string cellTextAlign = "left", string cellVerticalAlign = "top") : base(tableBorderStyle, cellBorderStyle, cellPaddingStyle, cellTextAlign, cellVerticalAlign)
        {
        }

        public Table(IEnumerable<TableRow> rows, bool noBorders, string cellPaddingStyle = "", string cellTextAlign = "left", string cellVerticalAlign = "top") : base(rows, noBorders, cellPaddingStyle, cellTextAlign, cellVerticalAlign)
        {
        }

        public Table(IEnumerable<TableRow> rows, string tableBorderStyle = "", string cellBorderStyle = "", string cellPaddingStyle = "", string cellTextAlign = "left", string cellVerticalAlign = "top") : base(rows, tableBorderStyle, cellBorderStyle, cellPaddingStyle, cellTextAlign, cellVerticalAlign)
        {
        }
    }

    public class TableCell : LINQPad.Controls.TableCell
    {
        public TableCell(params Control[] children) : base(children)
        {
        }

        public TableCell(IEnumerable<Control> children) : base(children)
        {
        }

        public TableCell(bool isHeader, params Control[] children) : base(isHeader, children)
        {
        }

        public TableCell(bool isHeader, IEnumerable<Control> children) : base(isHeader, children)
        {
        }
    }

    public class TableRow : LINQPad.Controls.TableRow
    {
        public TableRow(params Control[] children) : base(children)
        {
        }

        public TableRow(bool isHeader, params Control[] children) : base(isHeader, children)
        {
        }

        public TableRow(params LINQPad.Controls.TableCell[] children) : base(children)
        {
        }

        public TableRow(bool isHeader, IEnumerable<Control> children) : base(isHeader, children)
        {
        }

        public TableRow(IEnumerable<LINQPad.Controls.TableCell> children = null) : base(children)
        {
        }
    }
}
