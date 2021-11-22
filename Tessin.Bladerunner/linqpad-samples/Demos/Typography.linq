<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
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
	
	manager.PushBlade(Blade1(), "Typography");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		return Layout.Vertical( 
			Typography.H1("H1"),
			Typography.H2("H2"),
			Typography.Error("Error"),
			Typography.Small("Small"),
			Typography.P("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consectetur, quam non feugiat lobortis, quam elit elementum sapien, in interdum sem sem sit amet risus. In suscipit, lectus ac aliquet eleifend, dolor tortor molestie dolor, a finibus erat orci non dui. Etiam eget felis sapien. Cras risus mi, ornare porttitor varius ac, aliquet at lacus. Nullam egestas libero lectus, vitae venenatis tellus tincidunt et. Vestibulum eu ante tristique, porttitor quam vel, elementum lacus. Nam facilisis hendrerit libero molestie lacinia. Proin euismod dapibus magna. Nullam urna nibh, faucibus sed arcu ac, finibus rhoncus velit. Proin vitae urna turpis. Maecenas id pellentesque nulla. Fusce nec augue id enim feugiat condimentum.")
		);
	});
}