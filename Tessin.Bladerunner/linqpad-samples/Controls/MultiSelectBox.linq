<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
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
		
		RefreshContainer rc = new RefreshContainer(new [] { msb }, () => {
			return msb.SelectedOptions;
		});
		
		return Layout.Vertical(true, msb, rc);
	});
}

static IEnumerable<string> GetColors()
{
	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
	{
		if (prop.PropertyType.FullName == "System.Drawing.Color")
			yield return prop.Name;
	}
}