<Query Kind="Program">
  <Reference Relative="..\..\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll">C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{
	Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", showDebugButton: true, cssHotReloading: true);
	
	manager.PushBlade(Blade1(), "Blade1");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		var menu = new Menu(
			new MenuButton("Header Panel", (_) => {
				blade.PushBlade(Blade2(), "Blade2");
			}),
			new MenuButton("Card", (_) =>
			{
				blade.PushBlade(Blade3(), "Blade3");
			}),
			new MenuButton("Hello World", (_) => { }, Icons.Atom),
			new MenuButton("Lorem ipsum some really pushing long text", (_) => { }, Icons.Atom, pillTask: Utils.CreateTask<object>(e => "HEJ")),
			new MenuButton("Hello World", (_) => { }, Icons.Atom, actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.AlertCircle, (_) => {
				})
			}),
			new MenuButton(Layout.Vertical(false, "Niels Bosma", Typography.Small("Product Owner"), "niels@tessin.se"), (_) => { }, Icons.Atom, pillTask: Utils.CreateTask<object>(e => "HELLOWORLD"), actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.Alert, (_) => {
				})
			}),
			new MenuButton("Hello World", (_) =>
			{
				blade.PushBlade(Blade2(), "Blade2");
			})
		);

		return menu;

	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		var searchBox = new SearchBox();
		
		var refreshContainer = new RefreshContainer(new[] { searchBox }, () => {
			return new Menu(
				GetColors().Where(e => e.StartsWith(searchBox.Text)).Select(e => new MenuButton(e.ToString(), (_) => { })).ToArray()
			);
		});

		
		return new HeaderPanel(
			Layout.Horizontal(true, searchBox, new IconButton(Icons.Plus)), 
			refreshContainer
		);
		
	});
}

static IBladeRenderer Blade3()
{
	return BladeFactory.Make((blade) =>
	{
		PropertyList pl = new PropertyList(
			new Property("Antal investerare", 163),
			new Property("Investeringslag", "Lån"),
			new Property("Löptid", "Upp till 12 mån"),
			new Property("Årsränta", "8 %"),
			new Property("Actions", Layout.Horizontal(false,
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			))
		);

		return Layout.Vertical(true, new Card(pl));
	});
}

static IEnumerable<string> GetColors()
{
	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
	{
		if (prop.PropertyType.FullName == "System.Drawing.Color")
			yield return prop.Name;
	}
}