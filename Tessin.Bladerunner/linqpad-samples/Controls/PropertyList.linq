<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "PropertyList");
	
	manager.PushBlade(Blade2(), "PropertyList+Card");
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
			new Property("Lånenummer", new LINQPad.Controls.Hyperlink("#21139-1", (_) => { })),
			new Property("Actions", Layout.Gap(false).Horizontal(
				new IconButton(Icons.CoffeeOutline),
				new IconButton(Icons.Alert)
			))
		);
		return Layout.Vertical(pl);
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
			new Property("Lånenummer", new LINQPad.Controls.Hyperlink("#21139-1", (_) => { })),
			//new Property("Actions", Layout.Gap(false).Horizontal(
			//	new IconButton(Icons.CoffeeOutline),
			//	new IconButton(Icons.Alert)
			//)),
			new Property("Foo", 
				Typography.MaxWidth(300).P("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque et iaculis tortor."), isMultiLine: true)
		);

		return Layout.Vertical(new Card(pl, "FANCY TITLE"));
	});
}
