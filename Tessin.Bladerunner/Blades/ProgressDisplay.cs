using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LINQPad;
using LINQPad.Controls;

namespace Tessin.Bladerunner.Blades
{
    internal class ProgressInstance : IDisposable
    {
        private ProgressDisplay _progressDisplay;

        public ProgressInstance(ProgressDisplay progressDisplay, string title, IProgress<int> progress, CancellationTokenSource cancellationTokenSource)
        {
            _progressDisplay = progressDisplay;
            Title = title;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
        }

        public string Title { get; set; }

        public IProgress<int> Progress { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void Dispose()
        {
            _progressDisplay.Close();
        }
    }

    internal class ProgressDisplay : Control
    {
        private readonly Overlay _overlay;

        private readonly Div _divContainer;
        private readonly Div _divTitle;
        private readonly Div _divAnimation;

        private readonly Button _btnAbort;

        private readonly DumpContainer _progressContainer;
        private Util.ProgressBar _progressBar;

        private ProgressInstance _progressInstance;

        public ProgressDisplay(Overlay overlay)
        {
            _overlay = overlay;

            _divTitle = new Div();
            _divTitle.SetClass("progress--title");

            _divAnimation = new Div();
            _divAnimation.SetClass("progress--animation");

            _btnAbort = new Button("Abort", OnAbort);
            _btnAbort.Hide();

            _progressBar = new Util.ProgressBar(false);

            _progressContainer = new DumpContainer(_progressBar);
            _progressContainer.Hide();

            _divContainer = new Div(_divTitle, _divAnimation, _progressContainer, _btnAbort);
            _divContainer.SetClass("progress");
            _divContainer.Hide();

            VisualTree.Add(_divContainer);
        }

        private void OnAbort(Button obj)
        {
            _progressInstance?.CancellationTokenSource.Cancel();
        }

        public IDisposable Show(string title, Progress<int> progress, CancellationTokenSource cancellationTokenSource)
        {
            _progressInstance = new ProgressInstance(this, title, progress, cancellationTokenSource);
            _overlay.Show();
            _divContainer.Show();

            SetTitle(title);

            if (cancellationTokenSource != null)
            {
                _btnAbort.Show();
            }

            if (progress != null)
            {
                _progressContainer.Show();
                progress.ProgressChanged += (sender, i) =>
                {
                    _progressBar.Percent = i;
                };
            }

            return _progressInstance;
        }

        internal void SetTitle(string title)
        {
            _divTitle.HtmlElement.InnerText = title??"";
        }

        internal void Close()
        {
            _overlay.Hide();
            _divContainer.Hide();
            _progressContainer.Hide();
            _btnAbort.Hide();
            _progressInstance = null;
        }
    }
}
