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
	
	manager.PushBlade(Blade1(), "MultiSelectBox");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		MultiSelectBox msb = new MultiSelectBox(GetColors().Select(e => new Option(e, Guid.NewGuid())).Take(3).ToArray()); 
		
		RefreshPanel rc = new RefreshPanel(new [] { msb }, () => {
			return msb.SelectedOptions;
		});
		
		return Layout.Vertical(msb, rc);
	});
}

static IEnumerable<string> GetColors()
{
	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
	{
		if (prop.PropertyType.FullName == "System.Drawing.Theme")
			yield return prop.Name;
	}
}