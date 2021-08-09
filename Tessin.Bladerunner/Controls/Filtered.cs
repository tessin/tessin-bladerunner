using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad.Controls;

//todo: remove? 
namespace Tessin.Bladerunner.Controls
{
    public class Filtered<T> : Control
    {
        private IEnumerable<T> _records;

        public Filtered(IEnumerable<T> records, Func<string, Func<T,bool>> predicate)
        {
            _records = records;

            var txtSearch = new SearchBox(placeHolder:"Filter");

            var refreshPanel = new RefreshContainer(new[] {txtSearch}, () =>
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
