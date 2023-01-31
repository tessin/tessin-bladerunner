using System;
using System.Collections.Generic;
using System.Text;

namespace Tessin.Bladerunner
{
    /// <summary>
    /// We use CDNJS from Cloudflare it is fast and reliable.
    /// </summary>
    class Cdnjs
    {
        public static string GetUrl(string packageName, string packageVersion, string path)
        {
            return "https://cdnjs.cloudflare.com/ajax/libs/" + packageName + "/" + packageVersion + "/" + path;
        }

        public static readonly string Require_js = "require.js";
        public static readonly string Require_jsVersion = "2.3.6";
        public static readonly string Require_jsUrl = GetUrl(Require_js, Require_jsVersion, "require.js");

        public static readonly string Monaco = "monaco-editor";
        public static readonly string MonacoVersion = "0.34.1";
        public static readonly string MonacoRoot = GetUrl(Monaco, MonacoVersion, "min/"); // with trailing slash!
        public static readonly string MonacoMainCssUrl = GetUrl(Monaco, MonacoVersion, "min/vs/editor/editor.main.min.css");
    }
}
