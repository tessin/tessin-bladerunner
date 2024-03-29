﻿using System;
using System.Collections.Generic;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;
using Tessin.Bladerunner.Editors;
using Tessin.Bladerunner.Grid;

namespace Tessin.Bladerunner
{
    public static class Scaffold
    {

        public static EntityGrid<T> ToGrid<T>(this IEnumerable<T> rows)
        {
            return  new EntityGrid<T>(rows);
        }
        
        public static EntityGrid<T> Grid<T>(IEnumerable<T> rows)
        {
            return new EntityGrid<T>(rows);
        }

        public static EntityEditor<T> Editor<T>(T obj, Action<T> save = null, Action<T> preview = null, string actionVerb = "Save", Control toolbar = null) where T : new()
        {
            return new EntityEditor<T>(obj, save, preview, actionVerb, toolbar);
        }
        
        public static EntityEditor<T> ToEditor<T>(this T obj, Action<T> save = null, Action<T> preview = null, string actionVerb = "Save", Control toolbar = null) where T : new()
        {
            return new EntityEditor<T>(obj, save, preview, actionVerb, toolbar);
        }
        
        public static _PropertyListBuilder<T> PropertyList<T>(T obj)
        {
            return new _PropertyListBuilder<T>(obj);
        }
        
        //--------------------------------------------------------------------------------------------------------------

        internal static object Render(this object renderable)
        {
            if (renderable is IRenderable _renderable)
            {
                return _renderable.Render();
            }
            return renderable;
        }
        
    }
}