<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{
	//Debugger.Launch();
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	manager.PushBlade(Blade1());
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var btnShow = new Button("Show", (_) => {
			blade.Manager.ShowToaster(Layout.Middle().Horizontal(new Icon(Icons.ContentCopy), "User id copied to clipboard."));
		});

		var btnShowUntilClosed = new Button("Show Until Close", (_) =>
		{
			blade.Manager.ShowToaster(Layout.Middle().Horizontal(new Icon(Icons.Alert), "Can't create new user. Server is unresponsive."), 0, ToasterType.Error);
		});

		return Layout.Vertical(btnShow, btnShowUntilClosed);
	});
}