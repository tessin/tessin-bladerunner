<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	
	manager.Dump();
	
	//manager.PushBlade(Blade0(), "Blade0");
	//manager.PushBlade(Blade1(), "Blade1");
	//manager.PushBlade(Blade2(), "Blade2");
	manager.PushBlade(Blade4(), "Blade4");
	//manager.PushBlade(Blade5(), "Blade5");
}

static IBladeRenderer Blade0()
{
	return BladeFactory.Make((blade) =>
	{
		var menu = new Menu(
			new MenuButton("Foo", (_) => { }, Icons.Atom),
			new MenuButton("Bar", (_) => { }, Icons.Atom)
		);

		return menu;
	});
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		PropertyList pl = new PropertyList(
			new Property("Antal investerare", 163),
			new Property("Investeringslag", "Lån"),
			new Property("Löptid", "Upp till 12 mån"),
			new Property("Årsränta", "8 %"),
			new Property("Lånenummer", new Hyperlink("#21139-1", (_) => { })),
			new Property("Actions", Layout.Horizontal(
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			))
		);

		var menu = new Menu(
			new MenuButton("Header Panel", (_) => {
				blade.PushBlade(Blade2(), "Blade2");
			},pill:1_100_100),
			new MenuButton("Card", (_) =>
			{
				blade.PushBlade(Blade3(), "Blade3");
			}, pill:""),
			new MenuButton("Hello World", (_) => { }, Icons.Atom),
			new MenuButton("Lorem ipsum some really pushing long text", (_) => { }, Icons.Atom, pillTask: Utils.CreateTask<object>(e => DateTime.Now)),
			new MenuButton("Hello World", (_) => { }, Icons.Atom, actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.AlertCircle, (_) => {
				})
			}),
			new MenuButton(Layout.Gap(false).Vertical("Niels Bosma", Typography.Small("Product Owner"), "niels@tessin.se"), (_) => { }, Icons.Atom, pillTask: Utils.CreateTask<object>(e => "HELLOWORLD"), actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.Alert, (_) => {
				})
			})
		);

		return Layout.ContainerPadding(false).Horizontal(menu, Layout.Padding().Vertical(new Card(pl)));

	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make(async (blade) =>
	{
		await Task.Delay(500);
		
		var searchBox = new SearchBox();
		
		var refreshContainer = new RefreshPanel(new[] { searchBox }, async () => {
			await Task.Delay(500);
			return new Menu(
				GetColors().Where(e => e.StartsWith(searchBox.Text)).Select(e => new MenuButton(e.ToString(), (_) => { })).ToArray()
			);
		});

		return new HeaderPanel(
			Layout.Fill().Gap(false).Middle().Add(searchBox, "1fr").Horizontal(new IconButton(Icons.Plus)), 
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
			new Property("Actions", Layout.Gap(false).Horizontal(
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			)),
			new Property("Address", "Ola Taube<br />Häckvägen 12<br />132 43<br />Saltsjö-boo")
		);

		return Layout.Vertical(new Card(pl));
	});
}

static IBladeRenderer Blade4()
{
	return BladeFactory.Make((blade) =>
	{
		var refreshContainer = new RefreshPanel(null, () =>
		{
			return "";
		}, addPadding: true);

		return new HeaderPanel(
			Layout.Fill().Add(new SearchBox(), "1fr").Add(new TextBox(), "auto").Add(new Button("Hej", (_) => {}), "auto").Horizontal(new IconButton(Icons.Plus)),
			refreshContainer
		);
	});
}

static IBladeRenderer Blade5()
{
	return BladeFactory.Make((blade) =>
	{
		return new HeaderPanel(
			"Foo",
			"Bar"
		);
	});
}

static IEnumerable<string> GetColors()
{
	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
	{
		if (prop.PropertyType.FullName == "System.Drawing.Theme")
			yield return prop.Name;
	}
}