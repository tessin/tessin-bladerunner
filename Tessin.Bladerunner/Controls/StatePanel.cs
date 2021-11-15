using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LINQPad;

namespace Tessin.Bladerunner.Controls
{
    public class StatePanel : DumpContainer
    {
        private readonly AnyTaskFactory _taskFactory;

        public StatePanel(Func<StatePanel, Task<object>> onRefreshAsync)
        {
            _taskFactory = AnyTask.Factory<object>(() => onRefreshAsync(this));
            Update();
        }

        public void Update()
        {
            Task.Run(() => _taskFactory.Run().Result).ContinueWith(e =>
            {
                this.Content = e.Result;
            });
        }
    }
}
