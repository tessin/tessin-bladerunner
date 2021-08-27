<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);

	manager.PushBlade(Blade1(), "ContentFormatter");

	manager.Dump();
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
				span.Styles["background-color"] = "green";
				return span;
			}
		}
		
		
		return Layout.Vertical(true, 
			ContentFormatter.Format(0, Wrapper, "-"),
			ContentFormatter.Format(null, Wrapper, "-"),
			ContentFormatter.Format("", Wrapper, "-"),
			ContentFormatter.Format(1, Wrapper),
			ContentFormatter.Format(123456789, Wrapper),
			ContentFormatter.Format(123456789.24, Wrapper),
			ContentFormatter.Format(DateTime.Parse("1982-07-17"), Wrapper)
		);
	});
}