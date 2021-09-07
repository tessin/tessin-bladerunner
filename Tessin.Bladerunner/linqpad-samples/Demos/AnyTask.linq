<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>Tessin.Bladerunner</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	BladeManager manager = new BladeManager();
	manager.PushBlade(Blade1(), "Blade1");
	manager.Dump();
}

static IBladeRenderer Blade1()
{	
	return BladeFactory.Make((blade) =>
	{
		return new Menu(
			new MenuButton("HelloWorld", (_) => {
				blade.PushBlade(Blade2());
			},pillTask:Task.FromResult(123))
		);
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make(async (blade) =>
	{
		await Task.Delay(TimeSpan.FromSeconds(5));
		
		return new Menu(
			Enumerable.Range(1,100).Select(e => new MenuButton(e.ToString(), (_) => {}).ToControl()).ToArray()
		);
	});
}