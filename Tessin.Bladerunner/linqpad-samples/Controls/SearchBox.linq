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
	
	manager.Push(Blade1(), "SearchBox");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		var contextMenu = new ContextMenu(
			blade.Manager, 
			new IconButton(Icons.Filter), 
			new ContextMenu.Item("Bookmarked", icon: Icons.Star, tag: "bookmarked:true")
		);

		SearchBox sb = new SearchBox(contextMenu: contextMenu);

		RefreshPanel rc = new RefreshPanel(new[] { sb }, () =>
		{
			return Layout.Vertical(sb.Text);
		});
		
		contextMenu.ItemSelected += (_, e) => {
			if(!string.IsNullOrEmpty(e.Tag))
			{
				if(!sb.Text.Contains(e.Tag))
				{
					sb.Text = (sb.Text + " " + e.Tag).Trim();
				}
			}
		};

		return Layout.Vertical(sb, rc);
	});
}