using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    //todo: update and move to _Shared/_Shared.linq
    public class TessinLogo : Div
    {
        public TessinLogo() : base()
        {
            this.HtmlElement.InnerHtml =
                @"<img src=""https://tessinzbannerzuo3b.blob.core.windows.net/public/email/tessin-logo-email.png"" width=""164"" height=""54"">";
        }
    }
}
