<Query Kind="Expression">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);

	manager.PushBlade(Blade1(), "Layout");

	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		Layout
			.FullWidth()
			.FullHeight()
			.Align(MIDDLE | TOP)
			.Vertical()
			
		Layout
			.Gap()
			.FullWidth()
			.VercialAlign(MIDDLE)
			.Add(new SearchBox(), "1fr")
			.Add(new IconButton(), "auto")
			.Horizontal()
			.Render()
			
			
			
	});
}