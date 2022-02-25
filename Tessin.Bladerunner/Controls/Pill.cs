using LINQPad.Controls;

namespace Tessin.Bladerunner.Controls
{
    public class Pill : Control
    {
        public Pill(string text) : base("span", text)
        {
            this.SetClass("pill");
        }
    }
}
