﻿using LINQPad.Controls;
using System;
using System.Collections.Generic;
using Tessin.Bladerunner.Controls;
using Hyperlink = LINQPad.Controls.Hyperlink;

namespace Tessin.Bladerunner
{
    public class TypographyBuilder
    {
        private readonly Dictionary<string, string> _styles = new();
        private string _toolTip;

        public TypographyBuilder ToolTip(string toolTip)
        {
            _toolTip = toolTip;
            return this;
        }

        public TypographyBuilder Width(int value)
        {
            return Width(value + "px");
        }

        public TypographyBuilder Width(string value)
        {
            _styles["width"] = value;
            return this;
        }

        public TypographyBuilder MaxWidth(int value)
        {
            return MaxWidth(value + "px");
        }

        public TypographyBuilder MaxWidth(string value)
        {
            _styles["max-width"] = value;
            _styles["overflow"] = "hidden";
            _styles["text-overflow"] = "ellipsis";
            return this;
        }

        public TypographyBuilder NoWrap()
        {
            _styles["white-space"] = "nowrap";
            return this;
        }
        
        public TypographyBuilder Overflow(string overflow)
        {
            _styles["overflow"] = overflow;
            return this;
        }

        public Control P(string content)
        {
            return Render("p", content, "default");
        }

        public Control Html(string content)
        {
            return Render("div", content, "default", true);
        }

        public Control Span(string content)
        {
            return Render("span", content, "default");
        }

        public Control Code(string content)
        {
            return Render("pre", content, "code");
        }
        
        public Control InlineCode(string content)
        {
            return Render("span", content, "code");
        }

        public Control Small(string content)
        {
            return Render("span", content, "small");
        }

        public Control Error(string content)
        {
            return Render("span", content, "error");
        }

        public Control H1(string content)
        {
            return Render("h1", content, "default");
        }

        public Control H2(string content)
        {
            return Render("h2", content, "default");
        }

        public Control Link(string content, Action<Hyperlink> onClick)
        {
            var link = new Hyperlink(content, onClick);
            foreach (var key in _styles.Keys)
            {
                link.Styles[key] = _styles[key];
            }
            return link;
        }

        public Control Property(string content)
        {
            _styles["max-width"] = "150";
            return Title(content);
        }

        public Control Title(string content)
        {
            _styles["white-space"] = "nowrap";
            _styles["overflow"] = "hidden";
            _styles["text-overflow"] = "ellipsis";
            return Render("span", content, "default");
        }

        private Control Render(string htmlElementName, string content, string @class, bool isHtml = false)
        {
            if (string.IsNullOrEmpty(content)) return new EmptySpan("");

            Control control = isHtml ? new HtmlLiteral(htmlElementName, content)
                : new Control(htmlElementName, content);
            
            control.SetClass(@class);
            
            foreach (var key in _styles.Keys)
            {
                control.Styles[key] = _styles[key];
            }

            if (!string.IsNullOrEmpty(_toolTip))
            {
                control.HtmlElement.SetAttribute("title", _toolTip);
            }

            return control;
        }

        public Control Blockquote(string content)
        {
            return Render("blockquote", content, "default");
        }

        private class HtmlLiteral : Control
        {
            public HtmlLiteral(string htmlElementName, string html)
            {
                var inner = new Control
                {
                    HtmlElement =
                    {
                        InnerHtml = html
                    }
                };

                var container = new Control(htmlElementName, inner);
                
                this.VisualTree.Add(container);
            }
        }
    }

    public static class Typography
    {
        public static TypographyBuilder ToolTip(string toolTip)
        {
            return (new TypographyBuilder()).ToolTip(toolTip);
        }

        public static TypographyBuilder Width(int value)
        {
            return (new TypographyBuilder()).Width(value);
        }

        public static TypographyBuilder Width(string value)
        {
            return (new TypographyBuilder()).Width(value);
        }

        public static TypographyBuilder MaxWidth(int value)
        {
            return (new TypographyBuilder()).MaxWidth(value);
        }

        public static TypographyBuilder MaxWidth(string value)
        {
            return (new TypographyBuilder()).MaxWidth(value);
        }

        public static TypographyBuilder NoWrap()
        {
            return (new TypographyBuilder()).NoWrap();
        }
        
        public static TypographyBuilder Overflow(string overflow)
        {
            return (new TypographyBuilder()).Overflow(overflow);
        }

        public static Control P(string content)
        {
            return (new TypographyBuilder()).P(content);
        }

        public static Control Span(string content)
        {
            return (new TypographyBuilder()).Span(content);
        }

        public static Control Code(string content)
        {
            return (new TypographyBuilder()).Code(content);
        }
        
        public static Control InlineCode(string content)
        {
            return (new TypographyBuilder()).InlineCode(content);
        }

        public static Control Small(string content)
        {
            return (new TypographyBuilder()).Small(content);
        }

        public static Control Error(string content)
        {
            return (new TypographyBuilder()).Error(content);
        }

        public static Control H1(string content)
        {
            return (new TypographyBuilder()).H1(content);
        }

        public static Control H2(string content)
        {
            return (new TypographyBuilder()).H2(content);
        }

        public static Control Link(string content, Action<Hyperlink> onClick)
        {
            return (new TypographyBuilder()).Link(content, onClick);
        }

        public static Control Title(string content)
        {
            return (new TypographyBuilder()).Title(content);
        }
        
        public static Control Blockquote(string content)
        {
            return (new TypographyBuilder()).Blockquote(content);
        }

        public static Control Property(string content)
        {
            return (new TypographyBuilder()).Property(content);
        }
        
        public static Control Html(string content)
        {
            return (new TypographyBuilder()).Html(content);
        }
    }
}
