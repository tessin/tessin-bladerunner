<Query Kind="Program">
  <Reference Relative="..\..\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll">C:\Users\JohnLeidegren\code\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	manager.PushBlade(Blade1(), "CodeEditor");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return new Button("Open", (_) => {
			blade.Manager.OpenSideBlade(CodeBlade());
		});
	});
}

public class Foo {
	
	public string Title { get; set; }
	
	public string Code { get; set; }
	
}


static IBladeRenderer CodeBlade()
{
	return BladeFactory.Make((blade) =>
	{
		var foo = new Foo() {
			Title = "Project Alpha", 
			Code = "<foo>1</foo>"
		};
		
		return new EntityEditor<Foo>(foo, (_) => {
			blade.PushBlade(new DisplayBlade(foo));
		})
		.Editor(e => e.Code, e => e.Code("xml"))
		.Render();
	});
}
