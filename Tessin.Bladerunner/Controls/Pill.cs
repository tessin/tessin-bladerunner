using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Pill : Control
    {
        public Pill(string text, Theme theme = Theme.Empty) : base("span", text)
        {
            this.SetClass($"pill theme-{Utils.SplitCamelCase(theme.ToString()).Replace(" ", "-").ToLower()}");
        }
    }
}
