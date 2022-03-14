using LINQPad;
using LINQPad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tessin.Bladerunner.Controls
{
    public class RadioButtons : DumpContainer, IRefreshable
    {

        public RadioButtons(params Option[] options) : this(null, (e) => Layout.Gap(false).Vertical(e), options)
        {
        }

        public RadioButtons(object selectedValue, params Option[] options) : this(selectedValue, (e) => Layout.Gap(false).Vertical(e), options)
        {
        }

        public RadioButtons(object selectedValue, Func<IEnumerable<Control>, object> layout, params Option[] options)
        {
            if (selectedValue == null)
            {
                selectedValue = options[0].Value;
            }

            SelectedOption = options.FirstOrDefault(e => e.Value.Equals(selectedValue));

            string groupName = Guid.NewGuid().ToString();

            var btns = options.Select(e => new RadioButton(groupName, e.Label, e.Value.Equals(selectedValue), (_) =>
            {
                SelectedOption = e;
                Updated?.Invoke(SelectedOption);
            }));


            this.Content = layout(btns);
        }

        public Option SelectedOption { get; private set; }

        public object SelectedValue => SelectedOption?.Value;

        public object SelectedLabel => SelectedOption?.Label;

        public event RefreshEvent Updated;
    }
}
