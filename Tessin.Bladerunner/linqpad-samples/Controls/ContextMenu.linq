<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{		
	BladeManager manager = new BladeManager(showDebugButton:true);
	manager.PushBlade(Blade1(), "Blade1");
	manager.Dump();
}

public class Form {
	
	public string FirstName { get; set; }
	
	public string LastName { get; set; }
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout.Vertical(true, new ContextMenu(new IconButton(Icons.DotsVertical),
			new ContextMenu.Item("Foo", (_) => {
				blade.PushBlade(Blade1());
			}),
			new ContextMenu.Item("Bar", (_) => { })
		), 
		new ContextMenu(new IconButton(Icons.DotsVertical),
			new ContextMenu.Item("Foo", (_) =>
			{
				blade.PushBlade(Blade1());
			}),
			new ContextMenu.Item("Bar", (_) => { })
		));
	});
}

