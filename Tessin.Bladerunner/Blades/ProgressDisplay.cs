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

        private readonly Div _divWrap;

        private readonly Div _divTitle;
        private readonly Div _divAnimation;
        private readonly Div _divAnimationPart1;
        private readonly Div _divAnimationPart2;
        private readonly Div _divAnimationPart3;
        private readonly Div _divAnimationBg;

        private readonly Button _btnAbort;

        private readonly DumpContainer _progressContainer;
        private Util.ProgressBar _progressBar;

        private ProgressInstance _progressInstance;

        public ProgressDisplay(Overlay overlay)
        {
            _overlay = overlay;

            _divTitle = new Div();
            _divTitle.SetClass("progress--title");

            _divAnimationPart1 = new Div();
            _divAnimationPart1.SetClass("line");

            _divAnimationPart2 = new Div();
            _divAnimationPart2.SetClass("line");

            _divAnimationPart3 = new Div();
            _divAnimationPart3.SetClass("line");

            _divAnimationBg = new Div();
            _divAnimationBg.SetClass("animation-bg");

            _divAnimation = new Div(_divAnimationPart1, _divAnimationPart2, _divAnimationPart3, _divAnimationBg);
            _divAnimation.SetClass("progress--animation");

            _btnAbort = new Button("Abort", OnAbort);
            _btnAbort.Hide();

            _progressBar = new Util.ProgressBar(false);

            _progressContainer = new DumpContainer(_progressBar);
            _progressContainer.Hide();

            _divWrap = new Div(_divTitle, _divAnimation, _progressContainer, _btnAbort);
            _divWrap.SetClass("progress-container");

            _divContainer = new Div(_divWrap);
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
            _divContainer.Hide();
            _progressContainer.Hide();
            _btnAbort.Hide();
            _overlay.Hide();
            _progressInstance = null;
        }
    }
}
