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
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.Push(Blade1(), "TriStateCheckBox");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var box1 = new TriStateCheckBox("Unchecked", false);
		var box2 = new TriStateCheckBox("Checked", true);
		var box3 = new TriStateCheckBox("Indeterminate", null);

		RefreshPanel rc = new RefreshPanel(new[] { box1, box2, box3 }, () =>
		{
			return Layout.Vertical(box1.Checked.ToString(), box2.Checked.ToString(), box3.Checked.ToString());
		},addPadding:false);

		return Layout.Vertical(box1, box2, box3, rc);
	});
}