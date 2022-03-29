using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Tessin.Bladerunner
{
    public class Option
    {
        public Option()
        {

        }

        public Option(string label, object value, object tag)
        {
            Label = label;
            Value = value;
            Tag = tag;
        }

        public Option(string label, object value) : this(label, value, null)
        {
        }


        public Option(string label) : this(label, label, null)
        {
        }

        public string Label { get; set; }

        public object Value { get; set; }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }

    public static class OptionExtensions
    {
        public static Option[] ToOptions(this IEnumerable<string> options)
        {
            return options.Select(e => new Option(e)).ToArray();
        }
    }
}