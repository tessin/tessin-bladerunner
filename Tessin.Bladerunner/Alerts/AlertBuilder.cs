using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;

namespace Tessin.Bladerunner.Alerts
{
    public enum AlertResult
    {
        Ok,
        Cancel
    }

    public class AlertBuilder
    {
        private readonly BladeManager _manager;
        private string _message;
        private string _title;
        private readonly List<AlertAction> _actions = new List<AlertAction>();

        public AlertBuilder(BladeManager manager, string message = null, string title = null)
        {
            _manager = manager;
            _message = message;
            _title = title;
        }

        public AlertBuilder Action(string label, bool isPrimary = true, AlertResult alertResult = AlertResult.Ok)
        {
            _actions.Add(new AlertAction()
            {
                Label = label,
                IsPrimary = isPrimary,
                AlertResult = alertResult
            });
            return this;
        }

        public void ShowOkCancel(Action<AlertAction> onClose = null)
        {
            _actions.Add(AlertAction.Ok);
            _actions.Add(AlertAction.Cancel);
            Show(onClose);
        }

        public void ShowOk(Action<AlertAction> onClose = null)
        {
            _actions.Add(AlertAction.Ok);
            Show(onClose);
        }

        public void ShowException(Exception ex, Action<AlertAction> onClose = null)
        {
            _title = ex.GetType().Name;
            _message = ex.Message;
            _actions.Add(AlertAction.Ok);
            Show(onClose);
        }

        public void Show(Action<AlertAction> onClose = null)
        {
            _manager.OpenSideBlade(new AlertBlade(this), (e) =>
            {
                switch (e)
                {
                    case AlertAction aa:
                        onClose?.Invoke(aa);
                        break;
                    case bool b:
                        onClose?.Invoke(b ? AlertAction.Ok :  AlertAction.Cancel);
                        break;
                }
            }, _title);
        }

        public class AlertAction
        {
            public static AlertAction Ok => new AlertAction()
            {
                AlertResult = AlertResult.Ok,
                IsPrimary = true,
                Label = "Ok"
            };

            public static AlertAction Cancel => new AlertAction()
            {
                AlertResult = AlertResult.Cancel,
                IsPrimary = false,
                Label = "Cancel"
            };

            public AlertResult AlertResult { get; set;  }

            public string Label { get; set; }

            public bool IsPrimary { get; set;  }

        }

        public class AlertBlade : IBladeRenderer
        {
            private readonly AlertBuilder _builder;

            public AlertBlade(AlertBuilder builder)
            {
                _builder = builder;
            }

            public object Render(Blade blade)
            {
                Button RenderButton(AlertAction action)
                {
                    return new Button(action.Label, (_) =>
                    {
                        blade.Manager.CloseSideBlade(action);
                    });
                }
                return Layout.Vertical(
                    _builder._message,
                    Layout.Horizontal(_builder._actions.Select(RenderButton))
                );
            }
        }

    }
}
