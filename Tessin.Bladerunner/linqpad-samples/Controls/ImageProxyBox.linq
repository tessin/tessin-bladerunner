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

public static ImageProxySettings settings = new ImageProxySettings(new Uri("https://tessin-image-proxy.azurewebsites.net"), Util.GetPassword("FunctionKey"));

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.Push(Blade1(), "ImageProxyBox");
}

class Foo {
	
	public string Image { get; set; }
	
	public string Hello { get; set; }
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var record = new Foo();
		return new EntityEditor<Foo>(record)
			.Editor(e => e.Image, e => e.ImageProxy(settings))
			.Render();
	});
}