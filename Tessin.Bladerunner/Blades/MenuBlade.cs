﻿using LINQPad.Controls;
using System;
using System.Linq;
using Tessin.Bladerunner;
using Tessin.Bladerunner.Blades;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class MenuBlade : IBladeRenderer
    {
        private readonly DirectoryNode _rootNode;

        public MenuBlade(DirectoryNode rootNode)
        {
            _rootNode = rootNode;
        }

        public object Render(Blade blade)
        {
            return new Menu(_rootNode.Children.Select(e => e.Render(blade)).ToArray());
        }
    }
}

public interface IMenuBladeNode
{
    Control Render(Blade blade);
}

public class ActionNode : IMenuBladeNode
{
    private readonly string _label;
    private readonly Action _onClick;
    private readonly string _svgIcon;

    public ActionNode(string label, Action onClick, string svgIcon = null)
    {
        _label = label;
        _onClick = onClick;
        _svgIcon = svgIcon;
    }

    public Control Render(Blade blade)
    {
        return new MenuButton(_label, (_) =>
        {
            _onClick();
        }, _svgIcon);
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

    public Control Render(Blade blade)
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

    public Control Render(Blade blade)
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

    public Control Render(Blade blade)
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

    public Control Render(Blade blade)
    {
        var header = new Div(new Span(_label));
        header.HtmlElement.SetAttribute("class", "menu-blade-group-header");

        return new Div(
            new Control[] { header }.Union(_children.Select(e => e.Render(blade)))
        );
    }
}