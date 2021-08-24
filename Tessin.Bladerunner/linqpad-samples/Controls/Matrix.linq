<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	
	manager.PushBlade(Blade1(), "Matrix");
	
	manager.Dump();
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
		Add("D", "4", Layout.Vertical(false, new object[] { "23 %", new ProgressBar(23, "70px")}, HorizontalAlignment.Center));
		Add("E","5",Layout.Vertical(false, new Button("HelloWorld")));
		
		return Matrix<MatrixCell>.Create(list);
	});
}
