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

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "MultiSelectBox");
}

public class FooRecord
{
	public ICollection<Guid> Colors { get; set; }
	public int SelectedCount { get; set; }
	public string Foo { get; set; } = "Bar";
	public int FooCount { get; set; }
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var options = new Option[] {
			new Option("Red", Guid.NewGuid()),
			new Option("Blue", Guid.NewGuid()),
			new Option("Green", Guid.NewGuid()),
			new Option("Yellow", Guid.NewGuid()),
			new Option("Black", Guid.NewGuid())
		};

		var record = new FooRecord()
		{
			Colors = new List<Guid>(new[] { (Guid)options[1].Value, (Guid)options[3].Value })
		};

		return new EntityEditor<FooRecord>(record)
			.Editor(e => e.Colors, e => e.MultiSelect(options))
			.Editor(e => e.SelectedCount, e => e.Literal())
			.Editor(e => e.FooCount, e => e.Literal())
			.Derived(e => e.SelectedCount, e => e.Colors, r => r.Colors.Count())
			.Derived(e => e.FooCount, e => e.Foo, r => r.Foo.Length) 
			.Render();
	});
}

//static IEnumerable<string> GetColors()
//{
//	foreach (System.Reflection.PropertyInfo prop in typeof(SystemColors).GetProperties())
//	{
//		if (prop.PropertyType.FullName == "System.Drawing.Color")
//			yield return prop.Name;
//	}
//}