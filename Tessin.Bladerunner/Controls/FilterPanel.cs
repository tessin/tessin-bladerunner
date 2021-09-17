using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public class FilterPanel<T> : Control
    {
        public FilterPanel(IEnumerable<T> records, Func<string, Func<T, bool>> predicate)
        {
            var txtSearch = new SearchBox(placeHolder: "Filter");

            var refreshPanel = new RefreshPanel(new[] { txtSearch }, () =>
            {
                var searchText = txtSearch.Text.Trim();

                var linq = records;

                if (!string.IsNullOrEmpty(searchText))
                {
                    linq = linq.Where(predicate(searchText));
                }

                return linq.ToList();
            });

            VisualTree.Add(txtSearch);
            VisualTree.Add(refreshPanel);
        }
    }

}
