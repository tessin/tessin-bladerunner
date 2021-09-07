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
	//manager.PushBlade(Blade2());
	manager.PushBlade(Blade3());

	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout2
			.Right()
			.Fill()
			.Padding()
			.Vertical(new Button("Bar"), new Button("HelloWorld"), new Button("Hi"));
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout2
			.Padding()
			.Gap(false)
			.Middle()
			.Horizontal(new Button("Bar"), new Button("HelloWorld"), new Literal("Hello"));
	});
}

static IBladeRenderer Blade3()
{
	return BladeFactory.Make((blade) =>
	{
		var sb = new TextBox();

		var header = Layout2
			.Debug()
			.Middle()
			.Fill()
			.Add(sb, "100%")
			.Add(new IconButton(Icons.Plus))
			.Horizontal();

		return new HeaderPanel(header, new Div());
	});
}