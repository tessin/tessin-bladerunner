<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Buttons");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return
			Layout.Vertical(
				Layout.Horizontal(
					new Button("Primary", theme: Theme.Primary),
					new Button("Secondary"),
					new Button("Disabled", theme: Theme.Primary, enabled: false)
				),
				Layout.Horizontal(
					new Button("Error", theme: Theme.Error),
					new Button("Success", theme: Theme.Success),
					new Button("Alert", theme: Theme.Alert)
				),
				Layout.Horizontal(
					new Button("Primary Alternate", theme: Theme.PrimaryAlternate),
					new Button("Primary Alternate Disabled", theme: Theme.PrimaryAlternate, enabled: false)
				),
				Layout.Horizontal(
					new Button("Secondary Alternate", theme: Theme.SecondaryAlternate),
					new Button("Secondary Alternate Disabled", theme: Theme.SecondaryAlternate, enabled: false)
				)
			);
	});
}				