namespace Tessin.Bladerunner.Controls
{
    public class Literal : LINQPad.Controls.Literal
    {
        public Literal(string html) : base(html)
        {
        }

        public Literal(string htmlElementName, string innerHtml) : base(htmlElementName, innerHtml)
        {
        }
    }
}
