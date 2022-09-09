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
	//Bool           ✔        n/a
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

	manager.Push(new MenuBlade(new DirectoryNode("",
		new ActionNode("Text", () => {
			var record = new TextRecord();

			var editor = new EntityEditor<TextRecord>(record, (_) => {})
				.Required(e => e.TextRequired)
				.Editor(e => e.Url, e => e.Url())
				.Editor(e => e.Email, e => e.Email())
				.Render();
			
			manager.OpenSideBlade(new DisplayBlade(editor), title:"Text");
		}),
		new ActionNode("Bool", () =>
		{
			var record = new BoolRecord();

			var editor = new EntityEditor<BoolRecord>(record, (_) => { })
				.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "Bool");
		}),
		new ActionNode("File", () =>
		{
			var record = new FileRecord();

			var editor = new EntityEditor<FileRecord>(record, (_) => { })
				.Required(e => e.FileRequired)
				.Editor(e => e.File, e => e.File(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
				.Editor(e => e.FileRequired, e => e.File(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
				.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "File");
		}),
		new ActionNode("Select", () =>
		{
			var record = new SelectRecord();

			var options = new [] { new Option("Foo",1),new Option("Bar",2) };

			var editor = new EntityEditor<SelectRecord>(record, (_) => { })
				.Required(e => e.SelectRequired)
				.Editor(e => e.Select, e => e.Select(options))
				.Editor(e => e.SelectRequired, e => e.Select(options))
				.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "Select");
		}),
		new ActionNode("Number", () =>
		{
			var record = new NumberRecord();

			var editor = new EntityEditor<NumberRecord>(record, (_) => { })
				.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "Number");
		}),
		new ActionNode("Date", () =>
		{
			var record = new DateRecord();

			var editor = new EntityEditor<DateRecord>(record, (_) => {
				
					manager.ShowToaster(record);
			
				})
				.Required(e => e.Date)
				.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "Date");
		}),
		new ActionNode("Code", () =>
		{
			var record = new CodeRecord();

			var editor = new EntityEditor<CodeRecord>(record, (_) =>
			{
				manager.ShowToaster(record);
			})
			.Editor(e => e.LotsOfCode, e => e.Code("xml"))
			.Render();

			manager.OpenSideBlade(new DisplayBlade(editor), title: "Date");
		})
	))
	, "Editors");
}


public class DateRecord
{
	public DateTime? Date { get; set; }
	public DateTime DateRequired { get; set; } = DateTime.UtcNow;
}

public class CodeRecord
{
	public string NoCode { get; set; }
	
	public string LotsOfCode { get; set; }
}

public class BoolRecord
{
	public bool Normal { get; set; }
	public bool? Nullable { get; set; }
}

public class TextRecord
{	
	public string Text { get; set; }
	public string TextRequired { get; set; }
	public string Url { get; set; }
	public string Email { get; set; }
}

public class FileRecord
{
	public string File { get; set; }
	public string FileRequired { get; set; }
}

public class SelectRecord
{
	public int? Select { get; set; } = 2;
	public int SelectRequired { get; set; } = 2;
}

public class NumberRecord
{
	public int Int { get; set; }
	public int? IntNullable { get; set; } = null;
	public double Double { get; set; }
	public double? DoubleNullable { get; set; } = null;
	public decimal Decimal { get; set; }
	public decimal? DecimalNullable { get; set; } = null;
	public decimal Long { get; set; }
	public decimal? LongNullable { get; set; } = null;
	public decimal Test1 { get; set; } = 1_234_567.89m;
	public decimal Test2 { get; set; } = 0.07m;
}

















