<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "ContentFormatter");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		Control Wrapper(Control c, bool empty) {
			if(empty)
			{
				return c;
			}
			else
			{
				var span = new Span(c);
				span.Styles["background-color"] = "white";
				return span;
			}
		}
		
		return Layout.Vertical( 		
			new DefaultContentFormatter().Format(true, Wrapper, "-"),
			new DefaultContentFormatter().Format(0, Wrapper, "-"),
			new DefaultContentFormatter().Format(null, Wrapper, "-"),
			new DefaultContentFormatter().Format("", Wrapper, "-"),
			new DefaultContentFormatter().Format(1, Wrapper),
			new DefaultContentFormatter().Format(123456789, Wrapper),
			new DefaultContentFormatter().Format(123456789.24, Wrapper),
			new DefaultContentFormatter().Format(DateTime.Parse("1982-07-17"), Wrapper)
		);
	});
}