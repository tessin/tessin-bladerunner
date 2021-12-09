<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Grid</Namespace>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Paginator");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{
		var colors = GetColors();

		return PaginatorHelper.Create(colors, 10, (_colors) =>
		{	
			return EntityGridHelper.Create(_colors).Render();
		});
	});
}

public class Record
{
	public string Name { get; set; }
	
	public int Index { get; set; }
}

static IEnumerable<Record> GetColors()
{
	int i = 0;
	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
	{
		if (prop.PropertyType.FullName == "System.Drawing.Color")
			yield return new Record()
			{
				Name = prop.Name,
				Index = i++
			};
	}
}