using LINQPad;
using System;
using System.Linq;
using Tessin.Bladerunner.Controls;

namespace Tessin.Bladerunner.Blades
{
    public class SelectBlade : IBladeRenderer
    {

        private Option[] _options;

        private Action<Option> _onSelect;

        public SelectBlade(Option[] options, Action<Option> onSelect)
        {
            _options = options;
            _onSelect = onSelect;
        }

        public object Render(Blade blade)
        {
            var searchBox = new SearchBox();

            var refreshContainer = new RefreshPanel(new[] { searchBox }, () =>
            {
                return new Menu(
                    _options.Where(e => e.Label.StartsWith(searchBox.Text)).Select(e => new MenuButton(e.Label, (_) =>
                    {
                        _onSelect(e);
                    })).ToArray()
                );
            });

            return new HeaderPanel(
                Layout.Fill().Middle().Add(searchBox, "1fr").Horizontal(),
                refreshContainer
            );
        }
    }
}
