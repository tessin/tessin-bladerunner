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
	
	manager.Push(Blade1(), "Blade1");
}

public class Product {
	
	public string Name { get; set; }
	
	public string Id { get; set; }
	
	public string Color { get; set; }
	
}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{
		var records = new Product[] {
			new() { Name = "Wrench", Id = "WRE345", Color = "Red" },
			new() { Name = "Hammer", Id = "HAM335", Color = "Blue" },
			new() { Name = "Screwdriver", Id = "SCR987", Color = "Green" },
			new() { Name = "Pliers", Id = "PLI456", Color = "Yellow" },
			new() { Name = "Socket Set", Id = "SOC123", Color = "Black" },
			new() { Name = "Ratchet", Id = "RAT567", Color = "Silver" },
			new() { Name = "Drill", Id = "DRI789", Color = "Orange" },
			new() { Name = "Saw", Id = "SAW321", Color = "Purple" }
		};

		return Scaffold.Grid(records)	
			.Empty("There are no records in the database. Create your first!")
			.Render();
	});
}