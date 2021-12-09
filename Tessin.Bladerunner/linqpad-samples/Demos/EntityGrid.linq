<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <Namespace>Tessin.Bladerunner.Grid</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
}

public class Product {
	
	public string Foo { get; set; }
	
}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{
		var records = new Product[] {};

		return new EntityGrid<Product>(records)
		.Empty("There are no records in the database. Create your first!")
		.Render();
	});
}