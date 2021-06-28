using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner
{
    public class AnyTask
    {

        public static implicit operator AnyTask(Task task)
        {
            return new AnyTask(task);
        }

        private readonly Task _task;

        public object Result
        {
            get
            {
                if (!_task.IsCompleted)
                {
                    _task.Wait();
                }
                var result = _task.GetType().GetProperty("Result");
                return result?.GetValue(_task, null);
            }
        }

        public AnyTask(Task task)
        {
            this._task = task;
        }
    }
}
