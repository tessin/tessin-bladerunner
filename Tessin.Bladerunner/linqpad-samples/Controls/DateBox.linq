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
	
	manager.PushBlade(Blade1(), "DateBox");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		DateBox db = new DateBox();
		DateBox tb = new DateBox(showTime:true);

		RefreshPanel rc = new RefreshPanel(new[] { db, tb }, () =>
		{
			return Layout.Vertical(db.SelectedDate, tb.SelectedDate);
		},addPadding:false);

		return Layout.Vertical(db, tb, rc);
	});
}