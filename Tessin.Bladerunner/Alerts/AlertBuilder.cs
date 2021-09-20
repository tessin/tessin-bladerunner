using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQPad.Controls;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

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
        private object _body;
        private string _title;
        private readonly List<AlertAction> _actions = new List<AlertAction>();

        public AlertBuilder(BladeManager manager, object body = null, string title = null)
        {
            _manager = manager;
            _body = body;
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
            _body = Layout.Vertical(ex.Message, new CollapsablePanel("Stack Trace", Typography.Code(ex.StackTrace)));
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
                Controls.Button RenderButton(AlertAction action)
                {
                    return new Controls.Button(action.Label, (_) =>
                    {
                        blade.Manager.CloseSideBlade(action);
                    });
                }

                IContentFormatter formatter = new DefaultContentFormatter();

                return Layout.Vertical(
                    formatter.Format(_builder._body),
                    Layout.Horizontal(_builder._actions.Select(RenderButton))
                );
            }
        }

    }
}
