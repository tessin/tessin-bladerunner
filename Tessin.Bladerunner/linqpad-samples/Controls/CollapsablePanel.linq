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
	
	manager.PushBlade(Blade1(), "CollapsablePanel");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		//return new DefaultContentFormatter().Format(new Field("Title", new TextBox(), ""));
		
		return 
			Layout.Vertical(
			new Field("Title", new TextBox(), ""),
			new CollapsablePanel("Contact", Layout.Vertical(
				new Field("Name", new TextBox(), ""),
				new Field("Email", new TextBox(), "")
			)),
			new CollapsablePanel("Contact", Layout.Vertical(
				new Field("Name", new TextBox(), ""),
				new Field("Email", new TextBox(), "")
			)),
			new Field("Company", new TextBox(), "")
		);
	});
}