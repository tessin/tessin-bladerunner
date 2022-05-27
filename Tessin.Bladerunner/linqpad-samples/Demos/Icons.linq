<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>System.Resources</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	//https://materialdesignicons.com/icon/svg


	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	manager.Push(Blade1());
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var icons = GetIcons().OrderBy(e => e.name).ToList();
		
		var searchBox = new SearchBox();

		var refreshContainer = new RefreshPanel(new[] { searchBox }, () =>
		{
			return new Menu(
				icons.Where(e => e.name.ToLower().Contains(searchBox.Text.Trim().ToLower())).Select(e => new MenuButton(e.name, (_) => { }, svgIcon:e.svgIcon)).ToArray()
			);
		}, addPadding: true);

		return new HeaderPanel(
			Layout.Fill().Gap(false).Middle().Add(searchBox, "1fr").Horizontal(new IconButton(Icons.Plus)),
			refreshContainer
		);
	});
}

static IEnumerable<(string name, string svgIcon)> GetIcons()
{
	ResourceManager IconsClass =
		new ResourceManager(typeof(Icons));

	ResourceSet resourceSet =
		IconsClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

	foreach (DictionaryEntry entry in resourceSet)
	{
		yield return (entry.Key.ToString(), (string)entry.Value);
	}
}
