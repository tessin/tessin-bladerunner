<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Grid</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade2(), "FileBox");
}

class Foo {
	
	public string FileName { get; set; }
	
	public string Extension { get; set; }
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var fileBox = new FileBox();
		var fileField = new Field("Header Image", fileBox);

		RefreshPanel rc = new RefreshPanel(new[] { fileBox }, () =>
		{
			return Layout.Vertical(fileBox.Text);
		}, addPadding: false);

		return Layout.Vertical(fileField, rc);
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		var record = new Foo();
		return new EntityEditor<Foo>(record)
		.Editor(e => e.FileName, e => e.File())
		.Editor(e => e.Extension, e => e.Literal())
		.Derived(e => e.Extension, e => e.FileName, (r) => new FileInfo(r.FileName)?.Extension?.TrimStart('.'))
		.Render();
	});
}