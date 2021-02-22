using System;
using System.Linq;
using LINQPad;
using LINQPad.Controls;
using Tessin.Bladerunner;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class MenuBlade : IBladeRenderer
    {
        private DirectoryNode _rootNode;

        public MenuBlade(DirectoryNode rootNode)
        {
            _rootNode = rootNode;
        }

        public object Render(Blade blade)
        {
            return Util.VerticalRun(_rootNode.Children.Select(e => e.Render(blade)));
        }
    }
}

public interface IMenuBladeNode
{
    object Render(Blade blade);
}

public class ActionNode : IMenuBladeNode
{
    private readonly string _label;
    private readonly Action _onClick;
    private readonly string _svgIcon;

    public ActionNode(string label, Action onClick, string svgIcon)
    {
        _label = label;
        _onClick = onClick;
        _svgIcon = svgIcon;
    }

    public object Render(Blade blade)
    {
        return new MenuButton(_label, (_) =>
        {
            _onClick();
        }, _svgIcon ?? Icons.Application);
    }
}

public class ScriptNode : IMenuBladeNode
{
    private readonly string _label;
    private readonly string _scriptPath;
    private readonly bool _run;
    private readonly string _svgIcon;

    public ScriptNode(string label, string scriptPath, bool run = true, string svgIcon = null)
    {
        _label = label;
        _scriptPath = scriptPath;
        _run = run;
        _svgIcon = svgIcon;
    }

    public object Render(Blade blade)
    {
        return new MenuButton(_label, (_) =>
        {
            LINQPad.Util.OpenQuery(_scriptPath, _run, _run);
        }, _svgIcon ?? (_run ? Icons.Application : Icons.File));
    }
}

public class UrlNode : IMenuBladeNode
{
    private readonly string _label;
    private readonly string _url;
    private readonly string _svgIcon;

    public UrlNode(string label, string url, string svgIcon = null)
    {
        _label = label;
        _url = url;
        _svgIcon = svgIcon;
    }

    public object Render(Blade blade)
    {
        return new MenuButton(_label, (_) =>
        {
            System.Diagnostics.Process.Start(_url);
        }, _svgIcon ?? Icons.Application);
    }
}

public class DirectoryNode : IMenuBladeNode
{
    private readonly string _label;
    public IMenuBladeNode[] Children { get; }

    public DirectoryNode(string label, params IMenuBladeNode[] children)
    {
        _label = label;
        Children = children;
    }

    public object Render(Blade blade)
    {
        return new MenuButton(_label, (_) =>
        {
            blade.PushBlade(new MenuBlade(this));
        }, Icons.ChevronRight);
    }
}

public class GroupNode : IMenuBladeNode
{
    private readonly string _label;
    private readonly IMenuBladeNode[] _children;

    public GroupNode(string label, params IMenuBladeNode[] children)
    {
        _label = label;
        _children = children;
    }

    public object Render(Blade blade)
    {
        var header = new Div(new Span(_label));
        header.HtmlElement.SetAttribute("class", "menu-blade-group-header");

        return Util.VerticalRun(
            new object[] { header }.Union(_children.Select(e => e.Render(blade)))
        );
    }
}