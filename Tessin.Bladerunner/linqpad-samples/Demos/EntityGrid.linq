<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
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

	//BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	BladeManager manager = new BladeManager();
	manager.Dump();
	
	manager.Push(Blade1(), "Blade1");
}

public class Product {
	
	public string Name { get; set; }
	
	public string Id { get; set; }
	
	public string Color { get; set; }
	
	public string Hidden { get; set; }
	
	public bool Awesome  { get; set; }
	
	public double Price { get; set; }

	public object _ { get; set; }

}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{
		var records = new Product[] {
			new() { Name = "Wrench", Id = "WRE345", Color = "Red", Price = 3_456.12,
				_ = new IconButton(Icons.ArrowRight, (_) => { blade.Manager.ShowToaster("HelloWorld!"); }) },
			new() { Name = "Hammer", Id = "HAM335", Color = "Blue", Price = 56 },
			new() { Name = "Screwdriver", Id = "SCR112", Color = "Green", Price = 23 },
			new() { Name = "Pliers", Id = "PLI456", Color = "Yellow", Price = 78, Awesome = true },
			new() { Name = "Drill", Id = "DRI123", Color = "Black", Price = 190 },
			new() { Name = "Saw", Id = "SAW222", Color = "Orange", Price = 120 },
			new() { Name = "Paintbrush", Id = "PAI334", Color = "Purple", Price = 8 },
			new() { Name = "Tape measure", Id = "TAP122", Color = "Pink", Price = 15, Awesome = true },
			new() { Name = "Level", Id = "LEV432", Color = "Gray", Price = 35 },
			new() { Name = "Socket set", Id = "SOC123", Color = "Turquoise", Price = 75 },
			new() { Name = "Utility knife", Id = "UTI789", Color = "Red", Price = 20 }
		};

	var grid = Scaffold
		.Grid(records)
		//.Totals(e => e.Price)
		.RemoveEmptyColumns()
		//.HighlightRow(e => e.Price > 100)
		.Align(e => e.Color, CellAlignment.Center)
		.Empty("There are no records in the database. Create your first!")
		.UseJsGrid()
		.Render()
		;

	var textBox = new TextBox();

	var refreshPanel = new RefreshPanel(new[] { textBox },() => {
		return grid;
	});
	
	return new HeaderPanel(textBox, refreshPanel);
});
}