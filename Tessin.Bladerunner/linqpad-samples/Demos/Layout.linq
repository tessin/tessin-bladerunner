<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

static IDbConnection connection;

void Main()
{
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);

	manager.PushBlade(Blade1());
	manager.PushBlade(Blade2());
	manager.PushBlade(Blade3());

	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
	return Layout
		.Right()
		.Fill()
		.Vertical(new Button("Bar"), new Button("HelloWorld"), new Button("Hi"), DateTime.Now, true, new int[] { 1, 2, 3});
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout
			.Gap(false)
			.Middle()
			.Horizontal(new Button("Bar"), new Button("HelloWorld"), new Literal("Hello"));
	});
}

static IBladeRenderer Blade3()
{
	return BladeFactory.Make((blade) =>
	{
		var tb = new TextBox();
		
		var sb = new SelectBox(new string[] { "Hello", "Hej", "Bonjour" });

		var header = Layout
			//.Gap(false)
			.Middle()
			.Fill()
			.Add(tb, "1fr")
			.Add(sb, "1fr")
			.Add(new IconButton(Icons.Plus), "min-content")
			.Horizontal();

		return new HeaderPanel(header, 
			Layout.Vertical(
				new Button("Hello"),
				new Spacer("400px")
			)
		);
	});
}