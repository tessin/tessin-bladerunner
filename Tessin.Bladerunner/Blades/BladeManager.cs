using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using LINQPad;
using LINQPad.Controls;
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

        private DumpContainer _sideBladeContainer;

        private Action<object> _sideBladeOnClose;

        private Blade _sideBlade;

        private Overlay _overlay;

        private Popover _popover;

        private Toaster _toaster;

        private ProgressDisplay _progressDisplay;

        public BladeManager(int maxDepth = 10, bool showDebugButton = false, string cssPath = null, bool cssHotReloading = false)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((_, e) => ShowUnhandledException((Exception)e.ExceptionObject));
            LINQPad.Controls.Control.UnhandledException += new ThreadExceptionEventHandler((_, e) => ShowUnhandledException(e.Exception));

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

            Util.KeepRunning();
        }

        public void ShowUnhandledException(Exception ex)
        {
            new AlertBuilder(this).ShowException(ex);
        }

        public IDisposable ShowProgress(string title = null, Progress<int> progress = null,
            CancellationTokenSource cancellationTokenSource = null)
        {
            return _progressDisplay.Show(title, progress, cancellationTokenSource);
        }

        public void PushBlade(IBladeRenderer renderer, string title = "")
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

        public Toaster ShowToaster(string content, string icon = null, int timeout = 3000, ToasterType type = ToasterType.Normal)
        {
            if(icon == null)
            {
                _toaster.Show(new LINQPad.Controls.Literal(content), timeout, type);
            }
            else
            {
                _toaster.Show(Layout.Middle().Horizontal(new Icon(icon), new LINQPad.Controls.Literal(content)), timeout, type);
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
            while(_stack.Count()-1 > index)
            {
                var blade =_stack.Pop();
                blade.Clear();
            }
            if (refresh)
            {
                _stack.Peek().Refresh();
            }
        }

        object ToDump()
        {
            _containers = _panels.Select(Blade).ToArray();
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
            var psi = new System.Diagnostics.ProcessStartInfo {UseShellExecute = true, FileName = tempPath};
            Process.Start(psi);
        }
    }
}