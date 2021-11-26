using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tessin.Bladerunner.Grid;

namespace Tessin.Bladerunner.Controls
{
    public static class FilterPanelHelper
    {
        public static FilterPanel<T> Create<T>(IEnumerable<T> rows, Func<string, Func<T, bool>> predicate)
        {
            return new FilterPanel<T>(rows, predicate);
        }
    }

    public class FilterPanel<T> : Control
    {
        public FilterPanel(IEnumerable<T> records, Func<string, Func<T, bool>> predicate)
        {
            var txtSearch = new SearchBox();

            var refreshPanel = new RefreshPanel(new[] { txtSearch }, () =>
            {
                var searchText = txtSearch.Text.Trim();

                var linq = records;

                if (!string.IsNullOrEmpty(searchText))
                {
                    linq = linq.Where(predicate(searchText));
                }

                if(linq.Any())
                {
                    return new EntityGrid<T>(linq).Render();
                }
                else
                {
                    return "No matching records.";
                }
            });

            VisualTree.Add(Layout.Vertical(txtSearch, refreshPanel));
        }
    }

}
