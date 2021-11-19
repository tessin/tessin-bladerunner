using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Schema;
using LINQPad.Controls;

namespace Tessin.Bladerunner
{
    public class TypographyBuilder
    {
        private readonly Dictionary<string, string> _styles = new();

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
            return MaxWidth(value+"px");
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

        public Control P(string content)
        {
            return Render("p", content, "default");
        }

        public Control Span(string content)
        {
            return Render("span", content, "default");
        }

        public Control Code(string content)
        {
            return Render("pre", content, "code");
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

        private Control Render(string htmlElementName, string content, string @class)
        {
            var control = new Control(htmlElementName, content);
            control.SetClass(@class);
            foreach (var key in _styles.Keys)
            {
                control.Styles[key] = _styles[key];
            }
            return control;
        }
    }

    public static class Typography
    {
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
            return (new TypographyBuilder()).H1(content);
        }

        public static Control Link(string content, Action<Hyperlink> onClick)
        {
            return (new TypographyBuilder()).Link(content, onClick);
        }
    }
}
