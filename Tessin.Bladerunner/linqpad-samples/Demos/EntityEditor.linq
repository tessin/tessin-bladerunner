<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
}

public class Product {
	
	public int Integer { get; set; }
	
	public double Double { get; set; }
	
	public string File { get; set; }

	public string LiteralOne { get; set; } = "HelloWorld";

	public string LiteralTwo { get; set; } = "HelloWorld";

}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var record = new Product {
			Integer = 123456789,
			Double = 123456.134,
		};

		return new EntityEditor<Product>(record, (_) => {
					
		})
		.Editor(e => e.Double, e => e.Number(2))
		.Editor(e => e.LiteralOne, e => e.Literal())
		.Editor(e => e.File, e => e.File(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
		.Render();
	});
}