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
	
	manager.PushBlade(Blade1(), "Matrix");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		List<MatrixCell> list = new();
		void Add(string col, string row, object value)
		{
			list.Add(new MatrixCell(col, row, value));
		}
		
		Add("A","1",new Tessin.Bladerunner.Controls.Icon(Icons.CoffeeOutline));
		Add("B","2","Foo");
		Add("C", "3", "Bar");
		Add("D", "4", "X");
		Add("E","5", "X");
		
		return Matrix<MatrixCell>.Create(list);
	});
}
