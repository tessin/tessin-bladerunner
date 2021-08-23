<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	
	manager.PushBlade(Blade1(), "PropertyList");
	
	manager.PushBlade(Blade2(), "PropertyList+Card");
	
	manager.Dump();
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
			new Property("Actions", Layout.Horizontal(false,
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			))
		);

		return Layout.Vertical(true, pl);
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		PropertyList pl = new PropertyList(
			new Property("Antal investerare", 163),
			new Property("Investeringslag", "Lån"),
			new Property("Löptid", "Upp till 12 mån"),
			new Property("Årsränta", "8 %"),
			new Property("Lånenummer", new Hyperlink("#21139-1", (_) => { })),
			new Property("Actions", Layout.Horizontal(false,
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			))
		);

		return Layout.Vertical(true, new Card(pl));
	});
}
