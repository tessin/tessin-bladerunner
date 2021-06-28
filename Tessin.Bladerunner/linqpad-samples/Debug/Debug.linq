<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference Relative="..\..\bin\Debug\netstandard2.0\Tessin.Bladerunner.dll">C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netstandard2.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
</Query>

void Main()
{
	//Debugger.Launch();
	
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", showDebugButton: true, cssHotReloading: true);
	manager.PushBlade(Blade1(), "Blade1");
	manager.PushBlade(Blade2(), "Blade2");
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return new Menu(
			new MenuButton("HelloWorld", (_) => {}),
			new MenuButton("HelloWorld", (_) => {}, Icons.Atom),
			new MenuButton("HelloWorld", (_) => {}, Icons.Atom, pillTask: Utils.CreateTask<object>(e => "HEJ")),
			new MenuButton("HelloWorld", (_) => {}, Icons.Atom, actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.AlertCircle, (_) => {
				})
			}),
			new MenuButton("HelloWorld", (_) => {}, Icons.Atom, pillTask: Utils.CreateTask<object>(e => "HELLOWORLD"), actions: new IconButton[] {
				new IconButton(Icons.Alert, (_) => {
				}),
				new IconButton(Icons.Alert, (_) => {
				})
			})
		);
		
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		return new Menu(
			Enumerable.Range(1,100).Select(e => new MenuButton(e.ToString(), (_) => {}).ToControl()).ToArray()
		);
	});
}