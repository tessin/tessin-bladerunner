using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Tessin.Bladerunner.Grid;

namespace Tessin.Bladerunner.Controls
{
    public static class FilterPanelHelper
    {
        public static FilterPanel<T> Create<T>(IEnumerable<T> rows, Func<string, Func<T, bool>> predicate, Action<EntityGrid<T>> gridModifier = null)
        {
            return new FilterPanel<T>(rows, predicate, gridModifier);
        }
    }

    public class FilterPanel<T> : Control
    {
        public FilterPanel(IEnumerable<T> records, Func<string, Func<T, bool>> predicate, Action<EntityGrid<T>> gridModifier = null)
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

                if (linq.Any())
                {
                    var grid = new EntityGrid<T>(linq);

                    if (gridModifier != null)
                    {
                        gridModifier(grid);
                    }
                    
                    return grid.Render();
                }

                return "No matching records.";
            });

            VisualTree.Add(Layout.Vertical(txtSearch, refreshPanel));
        }
    }

}
