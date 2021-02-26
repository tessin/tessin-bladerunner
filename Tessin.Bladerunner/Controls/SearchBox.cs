using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class SearchBox : TextBox
    {
        public SearchBox(string initialText = "", string placeHolder = "Search") : base(initialText, width:"230px")
        {
            this.SetClass("search-box");
            this.HtmlElement.SetAttribute("placeholder", placeHolder);
        }
    }
}
