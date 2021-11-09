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
	
	manager.PushBlade(Blade1(), "ToggleButton");
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		ToggleButton tb1 = new ToggleButton(false, async (_) => {
			await Task.Delay(2000);
		}, trueLabel:"Fun", falseLabel:"Boring");

		ToggleButton tb2 = new ToggleButton(false, 
			async (_) =>
			{
				await Task.Delay(1000);
				throw new Exception("Unable to unsubscribe from this account.");
			},
			(ex) => {
				blade.Manager.ShowToaster(ex.Message, Icons.BugOutline, type: ToasterType.Error);
			}
		);

		RefreshPanel rc = new RefreshPanel(new[] { tb1, tb2 }, () =>
		{
			return Layout.Vertical(tb1.State.ToString(), tb2.State.ToString());
		});

		return Layout.Vertical(tb1, tb2, rc);
	});
}