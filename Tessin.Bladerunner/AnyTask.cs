using System;
using System.Threading.Tasks;

namespace Tessin.Bladerunner
{
    public class AnyTaskFactory
    {
        private Func<Task> taskFactory;

        public AnyTaskFactory(Func<Task> taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public AnyTask Run()
        {
            return taskFactory();
        }
    }

    public class AnyTask
    {
        public static AnyTaskFactory Factory<T>(Func<Task<T>> taskFactory)
        {
            return new AnyTaskFactory(() => (Task)taskFactory());
        }

        public static AnyTaskFactory Factory(Func<Task> taskFactory)
        {
            return new AnyTaskFactory(taskFactory);
        }

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

        public void OnResult(Action<object> cb)
        {
            this._task.ContinueWith(task => { cb(this.Result); });
        }
    }
}
