<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Runtime</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
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
	
	public double Ratio { get; set; }

	
}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var record = new Product {
			ProductId = 1,
			Ratio = 0.75
		};

		double foo = 1000;

		var products = new[] { 
			new Option("Food", 1, 0.25),
			new Option("Books", 2, 0.5),
			new Option("Life", 3, 0.68)
		};

		object Helper(Control x)
		{
			if (x is NumberBox numberBox)
			{
				var v = numberBox.Value;
				return Layout.Horizontal(
					Typography.Span($"X: {v:P1}"),
					Typography.Span($"T: {v+0.5:P1}"),
					Typography.Span($"P: {v+1.5:P1}")
				);
			}
			return null;
		}

		return new EntityEditor<Product>(record, (_) => {
					
		})
		.Helper(e => e.Ratio, Helper)
		.Editor(e => e.ProductId, e => e.Select(products))
		
		.Render();
	});
}