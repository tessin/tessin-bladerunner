<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Alerts</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

static string body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean dapibus, arcu quis vehicula vehicula, tortor nisi sollicitudin massa, et fringilla risus eros et leo.";

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
		var dc = new DumpContainer();
		
		return Layout.Vertical(
			new Button("ShowOkCancel", (_) =>
			{
				new AlertBuilder(blade.Manager,body,"ShowOkCancel").ShowOkCancel();
			}),
			new Button("ShowOk", (_) =>
			{
				new AlertBuilder(blade.Manager,body,"ShowOk").ShowOk();
			}),
			new Button("ShowInput:String", (_) =>
			{
				new AlertBuilder(blade.Manager, body, "ShowInput:String").ShowInput<string>("String", (a, v) => {
					if(a.AlertResult == AlertResult.Ok)
					{
						dc.Content = v;
					}
				});
			}),
			new Button("ShowInput:Int", (_) =>
			{
				new AlertBuilder(blade.Manager, body, "ShowInput:Int").ShowInput<int?>("Int", (a, v) =>
				{
					if(a.IsOk())
					{
						dc.Content = v;
					}
				});
			}),
			new Button("ShowInput:Date", (_) =>
			{
				new AlertBuilder(blade.Manager, body, "ShowInput:Date").ShowInput(DateTime.UtcNow, "Date", (a, v) =>
				{
					if(a.IsOk())
					{
						dc.Content = v;
					}
				});
			}),
			new Button("ShowInput:Bool", (_) =>
			{
				new AlertBuilder(blade.Manager, body, "ShowInput:Bool").ShowInput(false, "Bool", (a, v) =>
				{
					if (a.IsOk())
					{
						dc.Content = v;
					}
				});
			}),
			dc
		);
	});
}