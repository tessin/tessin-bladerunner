<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
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

	manager.Dump();

	//manager.Push(Blade4());
	//manager.Push(Blade1());
	manager.Push(Blade2());
	//manager.Push(Blade3());
	
	//manager.Push(Blade5());
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		return Layout
			.Right()
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
			.Horizontal(new Button("Bar"), new Button("HelloWorld"), "Hello");
	});
}

static IBladeRenderer Blade3()
{
	return BladeFactory.Make((blade) =>
	{		
		var tb = new TextBox();
		
		var sb = new SelectBox(new string[] { "Hello", "Hej", "Bonjour" });

		var spacer = new Spacer("200px");

		var header = Layout
			.Gap(false)
			.Middle()
			.Fill()
			.Add(tb, "1fr")
			.Add(sb, "1fr")
			.Add(new IconButton(Icons.Plus))
			.Horizontal();

		return new HeaderPanel(header,
			Layout.Vertical(
				new Button("Hello", (_) =>
				{
					spacer.Styles["width"] = "400px";
				}),
				spacer
			)
		);
	});
}

static IBladeRenderer Blade4()
{
	var txtComment = new TextArea();
	var fieldComment = new Field("Comment", txtComment);

	var card = new Card("Hello");

	return BladeFactory.Make((blade) =>
	{
		return Layout
			.Gap(true)
			.Vertical(new Button("Bar"), new Button("HelloWorld"), "Hello", card, new Button("Bar"), fieldComment);
	});
}

static IBladeRenderer Blade5()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout.Vertical(new Field("Foo", new TextBox()));
	});
}
	


