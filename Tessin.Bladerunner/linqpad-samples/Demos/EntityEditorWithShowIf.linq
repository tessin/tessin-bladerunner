<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Runtime</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.Push(Blade1(), "Blade1");
}

public class Product {
	
	public int ProductId { get; set; }
	
	public double Vat { get; set; }
	
	public double Price { get; set; }
	
}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var record = new Product {
			ProductId = 1
		};

		double foo = 1000;

		var products = new[] { 
			new Option("Food", 1),
			new Option("Books", 2),
			new Option("Life", 3)
		};

		return new EntityEditor<Product>(record, (_) => {
					
		})
		.Editor(e => e.ProductId, e => e.Select(products))
		.ShowIf(e => e.Vat, e => e.ProductId == 3)
		.Render();
	});
}