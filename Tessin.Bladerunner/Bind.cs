﻿using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner
{
    [Obsolete]
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
