<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
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
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
	
}

public class Form {
	
	public string FirstName { get; set; }
	
	public string LastName { get; set; }
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout.Right().Vertical(new ContextMenu(blade.Manager, new IconButton(Icons.DotsVertical),
			new ContextMenu.Item("Foo", (_) => {
				blade.PushBlade(Blade1());
			}, enabled:false, icon: Icons.AlienOutline),
			new ContextMenu.Item("Bar", (_) => { })
		), 
		new ContextMenu(blade.Manager, new IconButton(Icons.DotsVertical),
			new ContextMenu.Item("Foo", (_) =>
			{
				blade.PushBlade(Blade1());
			}, icon: Icons.AlienOutline),
			new ContextMenu.Item("Bar", (_) => { })
		));
	});
}

