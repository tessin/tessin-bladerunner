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
	
	manager.PushBlade(Blade1(), "Blade1");
}

public class Product {
	
	public int ProductId { get; set; }
	
	public double VAT { get; set; }
	
	public double TotalInclVAT { get; set; }
	
}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var record = new Product {
			ProductId = 1,
			VAT = 0.25
		};

		double foo = 1000;

		var products = new[] { 
			new Option("Food", 1, 0.25),
			new Option("Books", 2, 0.5),
			new Option("Life", 3, 0.68)
		};

		return new EntityEditor<Product>(record, (_) => {
					
		})
		.Editor(e => e.ProductId, e => e.Select(products))
		.Derived(e => e.VAT, e => e.ProductId, record => ((double?)products.FirstOrDefault(e => e.Value.Equals(record.ProductId)).Tag) ?? 0)
		.Derived(e => e.TotalInclVAT, e => e.ProductId, record => record.ProductId==3 && record.TotalInclVAT == 0 ? foo : record.TotalInclVAT)
		.Render();
	});
}