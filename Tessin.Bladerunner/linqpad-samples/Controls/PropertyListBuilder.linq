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
	
	manager.PushBlade(Blade1(), "PropertyListBuilder");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var test = new { 
			HelloWorld = "Bar",
			Name = 123456,
			FatAsText = Typography.MaxWidth(300).P("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque et iaculis tortor.")
		};
		
		var x = PropertyListBuilder.Create(test)
		.MultiLine(e => e.FatAsText)
		.Render();

		return Layout.Vertical(new Card(x));
	});
}


public static class X
{
	public static List<T> Create<T>(T obj) where T : new()
	{
		var foo = new List<T>();
		
		foo.Add(obj);
		
		return foo;
	}
}