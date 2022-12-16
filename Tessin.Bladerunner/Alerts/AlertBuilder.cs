using System;
using System.Collections.Generic;
using System.Linq;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;
using Tessin.Bladerunner.Editors;

namespace Tessin.Bladerunner.Alerts
{
    public enum AlertResult
    {
        Ok,
        Cancel
    }

    public class InputValue<T>
    {
        public T Value { get; set; }
    }

    public interface IAlertBuilderEditor
    {
        public object Render(Controls.Button saveButton = null);
    }

    public class AlertBuilder
    {
        private readonly BladeManager _manager;
        private object _body;
        private string _title;
        private IAlertBuilderEditor _editor;
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

        // Closers: 

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
            _body = Layout.Vertical(Typography.P(ex.Message), new CollapsablePanel("Stack Trace", Typography.Code(ex.StackTrace)));
            _actions.Add(AlertAction.Ok);
            Show(onClose);
        }

        public void ShowEditor<T>(EntityEditor<T> editor, Action<AlertAction> onClose = null) where T : new()
        {
            _editor = editor;
            _actions.Add(AlertAction.Ok);
            _actions.Add(AlertAction.Cancel);
            Show(onClose);
        }

        public void ShowInput<T>(string label, Action<AlertAction, T> onClose = null)
        {
            ShowInput(default(T), label, onClose);
        }

        public void ShowInput<T>(T defaultValue, string label, Action<AlertAction, T> onClose = null)
        {
            var entity = new InputValue<T>()
            {
                Value = defaultValue
            };

            var editor = Scaffold.Editor(entity)
                .Label(e => e.Value, label)
                .Required(e => e.Value);

            ShowEditor(editor, (action) =>
            {
                onClose?.Invoke(action, entity.Value);
            });
        }

        public void ShowTextInput(string defaultValue, string label, Action<AlertAction, string> onClose = null, bool required = true)
        {
            var entity = new InputValue<string>()
            {
                Value = defaultValue
            };

            var editor = Scaffold.Editor(entity)
                .Label(e => e.Value, label)
                .Editor(e => e.Value, e => e.Text(multiLine: true));

            if (required)
            {
                editor.Required(e => e.Value);
            }

            ShowEditor(editor, (action) =>
            {
                onClose?.Invoke(action, entity.Value);
            });
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
                        onClose?.Invoke(b ? AlertAction.Ok : AlertAction.Cancel);
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

            public bool IsOk()
            {
                return this.AlertResult == AlertResult.Ok;
            }

            public AlertResult AlertResult { get; set; }

            public string Label { get; set; }

            public bool IsPrimary { get; set; }
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
                object PrepareBody(object body)
                {
                    if (body is string strBody)
                    {
                        return Typography.P(strBody);
                    }
                    return body;
                }

                Controls.Button RenderButton(AlertAction action)
                {
                    return new Controls.Button(action.Label, (_) =>
                    {
                        blade.Manager.CloseSideBlade(action);
                    }, theme: action.IsPrimary ? Theme.Primary : Theme.Secondary);
                }

                var primary = RenderButton(_builder._actions.Single(e => e.IsPrimary));
                var buttons = new[] { primary }.Union(_builder._actions.Where(e => !e.IsPrimary).Select(RenderButton));

                IContentFormatter formatter = new DefaultContentFormatter();

                return Layout.Vertical(
                    formatter.Format(PrepareBody(_builder._body)),
                    _builder._editor?.Render(primary),
                    Layout.Horizontal(buttons)
                );
            }
        }

    }
}
