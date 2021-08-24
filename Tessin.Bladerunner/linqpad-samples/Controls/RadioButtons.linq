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
	
	manager.PushBlade(Blade1(), "RadioButtons");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		Guid ola = Guid.NewGuid();
	
		RadioButtons rbs = new RadioButtons(ola, (e) => Layout.Horizontal(true, e),
			new Option("Niels",Guid.NewGuid()),
			new Option("John",Guid.NewGuid()),
			new Option("Ola", ola),
			new Option("Dennis",Guid.NewGuid())
		);
		
		var chk = new CheckBox("Test");
		
		RefreshContainer rc = new RefreshContainer(new [] {rbs}, () => {
			return rbs.SelectedOption;
		});
		
		return Layout.Vertical(true, chk, rbs, rc);
	});
}