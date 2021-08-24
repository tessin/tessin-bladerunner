<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	BladeManager manager = new Tessin.Bladerunner.Blades.BladeManager();
	manager.PushBlade(Blade1(), "Blade1");
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var txtInput = new TextBox();

		Func<Task<object>> foo = async () =>
		{
			await Task.Delay(500);
			return "Works";
		};

		//Func<object> foo = () =>
		//{
		//	return "Also Works";
		//};

		var refreshContainer = new RefreshContainer(new[] { txtInput }, foo);
		
		return Layout.Vertical(true,
			txtInput,
			refreshContainer
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