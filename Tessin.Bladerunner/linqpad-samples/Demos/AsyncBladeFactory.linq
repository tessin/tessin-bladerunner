<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
</Query>

void Main()
{	
	BladeManager manager = new BladeManager();
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
	manager.PushBlade(Blade2(), "Blade2");
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return new Menu(
			new MenuButton("HelloWorld", (_) => {
				blade.PushBlade(Blade2());
			})
		);
		
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make(async (blade) =>
	{
		await Task.Delay(TimeSpan.FromSeconds(5));
		
		return new Menu(
			Enumerable.Range(1,100).Select(e => new MenuButton(e.ToString(), (_) => {})).ToArray()
		);
	});
}