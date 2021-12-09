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
	
	manager.PushBlade(Blade1(), "RefreshPanel");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{
		await Task.Delay(1000);
	
		var txtInput = new TextBox();
		
		RefreshPanel rc = new RefreshPanel(new[] { txtInput }, async () =>
		{
			await Task.Delay(1000);
			return "Hej";
		}, addPadding:true);

		return new HeaderPanel(txtInput, rc);
	});
}