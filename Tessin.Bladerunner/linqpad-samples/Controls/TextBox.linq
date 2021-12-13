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
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "RadioButtons");

}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var textBox = new TextBox("HelloWorld");
		var textField = new Field("Foo", textBox);

		var selectBox = new SelectBox(new[] {"Male","Female"});
		var selectField = new Field("Sex", selectBox);

		var dateBox = new DateBox();
		var dateField = new Field("Birth Day", dateBox);
		
		var numberBox = new NumberBox(10_000_000.123, 3);
		var numberField = new Field("Worth", numberBox);

		var textArea = new TextArea();
		var textAreaField = new Field("Comment", textArea);

		return Layout.Vertical(textField, selectField, dateField, numberField, textAreaField);
	});
}