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
		var btnWindowsForms = new Button("Windows Forms", (_) =>
		{
			var saveDialog = new System.Windows.Forms.SaveFileDialog();
			saveDialog.Title = "Save Engagment Report";
			saveDialog.FileName = $"foo.pdf";
			saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			var dialogResult = saveDialog.ShowDialog();
		});
		
		return Layout.Vertical(btnWindowsForms);
	});
}