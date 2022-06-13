<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
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
	
	manager.Push(Blade1(), "HorizontalBarChart");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var chart = new HorizontalBarChart(
			new Tessin.Bladerunner.Controls.HorizontalBarChart.Point[] { 
				new(500, "134", "#b5b5b5"),
				new(134, "134", "#11e86b"),
				new(259, "134", "#ff0d0d"),
			},
			100,
			12
		);
		
		return Layout.Vertical(chart);
	});
}