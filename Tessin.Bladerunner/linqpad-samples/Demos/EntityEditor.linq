<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
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
           
	//	             Nullable  Required  Styling  Buggy
	//File           ✔        ✔        !
	//Text           ✔        ✔
	//Select         ✔        ✔
	//Email          ✔        ✔
	//Url            ✔        ✔
	//Int            ✔        ✔
	//Double         ✔        ✔                 !   
	//AsyncDataList  ✔        ✔
	//Code      
	//TextArea
    //MultiSelect
	//Date           ✔        ✔

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.Push(Blade1(), "Blade1");
}

public class Product
{

	public string File { get; set; }

	public string FileRequired { get; set; }

	public string Literal { get; set; } = "HelloWorld";

	public int? Integer { get; set; }

	public int IntegerRequired { get; set; }

	public double? Double { get; set; }

	public double DoubleRequired { get; set; }

	public string Email { get; set; }

	public string EmailRequired { get; set; }

	public string Url { get; set; }

	public int? TagId { get; set; }

	public DateTime? DateTime { get; set; }

}


static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var record = new Product {
			Integer = 123456789,
			Double = 0.45,
			TagId = 2
		};

		return new EntityEditor<Product>(record, (_) => {
					
		})
		.Editor(e => e.Double, e => e.Number(2))
		.Editor(e => e.Literal, e => e.Literal())
		.Editor(e => e.File, e => e.File(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
		.Editor(e => e.FileRequired, e => e.File(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
		.Editor(e => e.Url, e => e.Url())
		.Editor(e => e.Email, e => e.Email())
		.Editor(e => e.Double, e => e.Number(2))
		.Editor(e => e.EmailRequired, e => e.Email())
		.Editor(e => e.TagId, e => e.Select(new [] { new Option("Foo",1),new Option("Bar",2) }))
		//.Required(e => e.DateTime)
		//.Required(e => e.FileRequired)
		//.Required(e => e.EmailRequired)
		//.Required(e => e.IntegerRequired)
		//.Required(e => e.DoubleRequired)
		.Render();
	});
}