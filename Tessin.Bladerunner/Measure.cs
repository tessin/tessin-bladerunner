using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LINQPad;

namespace Tessin.Bladerunner
{
	public class Measure : IDisposable
    {
        private readonly string _label;
        private readonly Stopwatch _stopWatch;

        public Measure(string label)
        {
            _label = label;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void Dispose()
        {
            _stopWatch.Stop();
            $"-{_label}:{_stopWatch.Elapsed.Seconds}s".Dump();
        }
    }
}
