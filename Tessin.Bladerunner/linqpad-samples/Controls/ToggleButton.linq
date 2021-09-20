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
	
	manager.PushBlade(Blade1(), "DateBox");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		ToggleButton tb = new ToggleButton(false, async (_) => {
			await Task.Delay(1000);
			return true;
		});

		RefreshPanel rc = new RefreshPanel(new[] { tb }, () =>
		{
			return Layout.Vertical(tb.State);
		});

		return Layout.Vertical(tb, rc);
	});
}