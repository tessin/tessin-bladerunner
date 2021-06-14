using System;
using System.Collections.Generic;
using System.Text;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public static class Bind<TFrom, TTo> where TFrom : Control where TTo : Control
    {
        public static void OneWay(TFrom fromControl, TTo toControl, Action<TTo, object> setter)
        {
            if (fromControl is TextBox tb)
            {
                tb.TextInput += (sender, args) => { setter(toControl, tb.Text); };
            }
        }
    }
}
