using LINQPad.Controls;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Tessin.Bladerunner
{
    public class StyleManager
    {
        private FileSystemWatcher _watcher;

        public Div Init(string cssPath, bool cssHotReloading)
        {
            var divFonts = new Div();
            divFonts.HtmlElement.InnerHtml = $@"
                <link rel=""preconnect"" href=""https://fonts.gstatic.com"" />
                <link href=""https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400&display=swap"" rel=""stylesheet"" />
            ";

            Control style;
            if (!string.IsNullOrEmpty(cssPath))
            {
                style = new Control("link");
                style.HtmlElement.SetAttribute("href", cssPath);
                style.HtmlElement.SetAttribute("rel", "stylesheet");

                if (cssHotReloading)
                {
                    _watcher = new FileSystemWatcher
                    {
                        Path = Path.GetDirectoryName(cssPath),
                        NotifyFilter = NotifyFilters.LastWrite,
                        Filter = Path.GetFileName(cssPath)
                    };

                    var lastHash = FileHash(cssPath);

                    _watcher.Changed += (a, b) =>
                    {
                        try
                        {
                            var hash = FileHash(cssPath);
                            if (hash != lastHash)
                            {
                                var tempPath = Path.GetTempFileName() + ".css";
                                File.Copy(cssPath, tempPath);
                                style.HtmlElement.SetAttribute("href", tempPath);
                                lastHash = hash;
                            }
                        }
                        catch (Exception)
                        {
                           // ignore
                        }
                    };
                    _watcher.EnableRaisingEvents = true;
                }
            }
            else
            {
                style = new Control("style");
                style.HtmlElement.InnerHtml = Themes.Themes.Default;
            }

            return new Div(divFonts, style);
        }

        private static string FileHash(string file)
        {
            using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

    }
}