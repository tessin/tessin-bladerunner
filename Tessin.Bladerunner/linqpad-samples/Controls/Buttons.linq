<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
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
	
	manager.Push(Blade1(), "Buttons");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return
			Layout.Vertical(
				Layout.Horizontal(
					new Button("Primary", theme: Theme.Primary),
					new Button("Primary Disabled", theme: Theme.Primary, enabled: false)
				),
				Layout.Horizontal(
					new Button("Primary Alternate", theme: Theme.PrimaryAlternate),
					new Button("Primary Alternate Disabled", theme: Theme.PrimaryAlternate, enabled: false)
				),
				Layout.Horizontal(
					new Button("Error", theme: Theme.Error),
					new Button("Success", theme: Theme.Success),
					new Button("Alert", theme: Theme.Alert)
				),
				Layout.Horizontal(
					new Button("Secondary", theme: Theme.Secondary),
					new Button("Secondary Disabled", theme: Theme.Secondary, enabled: false)
				),
				Layout.Horizontal(
					new Button("Secondary Alternate", theme: Theme.SecondaryAlternate),
					new Button("Secondary Alternate Disabled", theme: Theme.SecondaryAlternate, enabled: false)
				),
				Layout.Horizontal(
					new IconButton(Icons.Duck, theme:Theme.Primary),
					new IconButton(Icons.Duck, theme:Theme.Primary, enabled:false),
					new IconButton(Icons.Duck, theme:Theme.PrimaryAlternate),
					//new IconButton(Icons.Duck, theme:Theme.Secondary),	
					//new IconButton(Icons.Duck, theme:Theme.SecondaryAlternate),
					new IconButton(Icons.Duck, theme:Theme.Error),	
					new IconButton(Icons.Duck, theme:Theme.Success),	
					new IconButton(Icons.Duck, theme:Theme.Alert),
					new IconButton(Icons.Duck, theme:Theme.Empty)
				),
				Layout.Horizontal(
					new Icon(Icons.Duck, theme:Theme.Primary),
					new Icon(Icons.Duck, theme:Theme.PrimaryAlternate),
					//new Icon(Icons.Duck, theme:Theme.Secondary),
					//new Icon(Icons.Duck, theme:Theme.SecondaryAlternate),
					new Icon(Icons.Duck, theme:Theme.Error),
					new Icon(Icons.Duck, theme:Theme.Success),
					new Icon(Icons.Duck, theme:Theme.Alert)	
				)

			);
	});
}				