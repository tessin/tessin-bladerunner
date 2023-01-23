using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tessin.Bladerunner.Alerts;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class BladeManager
    {
        static BladeManager()
        {
            Util.HtmlHead.AddScript(Javascript.Blades);
        }

        private readonly Stack<Blade> _stack;

        private readonly DumpContainer[] _panels;

        private Div[] _containers;

        private readonly StyleManager _styleManager;

        private readonly int _maxDepth;

        private readonly string _cssPath;

        private readonly bool _cssHotReloading;

        public bool ShowDebugButton { get; }

        private Div _divBladeManager;

        private Div _divSideBlade;

        private readonly DumpContainer _sideBladeContainer;

        private Action<object> _sideBladeOnClose;

        private Blade _sideBlade;

        private readonly Overlay _overlay;

        private readonly Popover _popover;

        private readonly Toaster _toaster;

        private readonly ProgressDisplay _progressDisplay;

        public BladeManager(int maxDepth = 10, bool showDebugButton = false, string cssPath = null, bool cssHotReloading = false)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((_, e) => ShowUnhandledException((Exception)e.ExceptionObject));
            LINQPad.Controls.Control.UnhandledException += new ThreadExceptionEventHandler((_, e) => ShowUnhandledException(e.Exception));
            TaskScheduler.UnobservedTaskException +=
                new EventHandler<UnobservedTaskExceptionEventArgs>((_, e) => ShowUnhandledException(e.Exception));

            _maxDepth = maxDepth;
            _cssPath = cssPath;
            _cssHotReloading = cssHotReloading;
            ShowDebugButton = showDebugButton;
            _stack = new Stack<Blade>();
            _panels = Enumerable.Range(0, _maxDepth).Select((e, i) => new DumpContainer()).ToArray();
            _sideBladeContainer = new DumpContainer();
            _styleManager = new StyleManager();
            _overlay = new Overlay();
            _popover = new Popover();
            _toaster = new Toaster();
            _progressDisplay = new ProgressDisplay(_overlay);
            _containers = _panels.Select(Blade).ToArray();
            
            Util.KeepRunning();
        }

        public void ShowUnhandledException(Exception ex)
        {
            if (ex is AggregateException) return; 
            new AlertBuilder(this).ShowException(ex);
        }

        public IDisposable ShowProgress(string title = null, Progress<int> progress = null,
            CancellationTokenSource cancellationTokenSource = null)
        {
            return _progressDisplay.Show(title, progress, cancellationTokenSource);
        }

        [Obsolete("Use Push().")]
        public void PushBlade(IBladeRenderer renderer, string title = "")
        {
            Push(renderer, title);
        }

        public void Push(IBladeRenderer renderer, string title = "")
        {
            var blade = new Blade(this, renderer, _stack.Count(), _panels[_stack.Count()], title, _containers[_stack.Count()]);
            _stack.Push(blade);
            blade.Refresh();
        }

        public void OpenSideBlade(IBladeRenderer renderer, Action<object> onClose = null, string title = "")
        {
            if (_sideBlade != null)
            {
                _sideBlade?.Clear();
                _sideBlade = null;
                _overlay.Hide();
            }

            _overlay.Show();
            _sideBladeOnClose = onClose;
            _divSideBlade.SetVisibility(true);
            _sideBlade = new Blade(this, renderer, -1, _sideBladeContainer, title, null);
            _sideBlade.Refresh();
        }

        public void CloseSideBlade(object result = null, bool refresh = false)
        {
            _divSideBlade.SetVisibility(false);
            _overlay.Hide();
            _sideBlade?.Clear();
            _sideBlade = null;
            if (_sideBladeOnClose != null)
            {
                _sideBladeOnClose(result);
            }
            else if (refresh)
            {
                _stack.Peek().Refresh();
            }
            _sideBladeOnClose = null;
        }

        public Popover ShowPopover(Control content, string parentId)
        {
            _popover.Show(content, parentId);
            return _popover;
        }

        public Toaster ShowToaster(Control content, int timeout = 3000, ToasterType type = ToasterType.Normal)
        {
            _toaster.Show(content, timeout, type);
            return _toaster;
        }

        public Toaster ShowToaster(object content, string icon = null, int timeout = 3000, ToasterType type = ToasterType.Normal)
        {
            if (icon == null)
            {
                _toaster.Show(Layout.Horizontal(content), timeout, type);
            }
            else
            {
                _toaster.Show(Layout.Middle().Horizontal(new Icon(icon), content, timeout, type));
            }

            return _toaster;
        }

        public Toaster ShowToaster(Exception e)
        {
            _toaster.Show(new LINQPad.Controls.Literal(e.Message), 3000, ToasterType.Error);
            return _toaster;
        }

        public void PopTo(int index, bool refresh)
        {
            while (_stack.Count() - 1 > index)
            {
                var blade = _stack.Pop();
                blade.Clear();
            }
            if (refresh)
            {
                _stack.Peek().Refresh();
            }
        }

        public void Pop(bool refresh = false)
        {
            if (_stack.Any())
            {
                var blade = _stack.Pop();
                blade.Clear();
            }

            if (refresh)
            {
                _stack.Peek().Refresh();
            }
        }

        object ToDump()
        {
            //Util.HtmlHead.AddScriptFromUri("https://unpkg.com/monaco-editor@latest/min/vs/loader.js");
            return BladeWrapper(_containers);
        }

        object BladeWrapper(params Control[] blades)
        {
            var div = new Div(blades).SetClass("blade-wrapper");
            _divSideBlade = SideBlade(_sideBladeContainer);

            _divBladeManager = new Div(_styleManager.Init(_cssPath, _cssHotReloading), div, _overlay, _divSideBlade, _progressDisplay, _popover, _toaster);

            return _divBladeManager;
        }

        Div Blade(DumpContainer dc)
        {
            var innerDiv = new Div(dc);
            innerDiv.HtmlElement.SetAttribute("class", "blade-container");

            var outerDiv = new Div(innerDiv);
            outerDiv.HtmlElement.SetAttribute("class", "blade blade-hidden");

            return outerDiv;
        }

        Div SideBlade(DumpContainer dc)
        {
            return new Div(
                new Div(
                    new Div(dc).SetClass("blade-container")
                ).SetClass("blade")
            ).SetClass("side-blade").SetVisibility(false);
        }

        public void DebugHtml()
        {
            var tempPath = Path.GetTempFileName() + ".html";
            File.WriteAllText(tempPath, _divBladeManager.HtmlElement.InnerHtml);
            var psi = new System.Diagnostics.ProcessStartInfo { UseShellExecute = true, FileName = tempPath };
            Process.Start(psi);
        }
    }
}