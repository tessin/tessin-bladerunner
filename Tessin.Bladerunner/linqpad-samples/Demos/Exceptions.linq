<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

static class Ext
{
	public static Task<T> Try<T>(this Task<T> task)
	{
		return task.ContinueWith<T>(e =>
		{	
			if(e.Exception != null)
			{
				throw new Exception("Hello");
			}
			return e.Result;
		});
	}
}

void Main()
{
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);

	manager.Dump();
	
	manager.PushBlade(Blade1(), "");
	manager.PushBlade(Blade2(), "");
	manager.PushBlade(Blade3(), "");
	manager.PushBlade(Blade4(), "");
	
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		throw new Exception("Exception in blade rendering.");
		return "";
	});
}

static IBladeRenderer Blade2()
{
	return BladeFactory.Make((blade) =>
	{
		return new LINQPad.Controls.Button("Crash", blade.Catch((_) => {
			throw new Exception("Exception in control handler.");
		}));	
	});
}

static IBladeRenderer Blade3()
{
	return BladeFactory.Make(async (blade) =>
	{
		return await Foo();
	});
}

static async Task<string> Foo()
{
	await Task.Delay(300);
	throw new Exception("Exception in control handler.");
	return "Hello";
}

static IBladeRenderer Blade4()
{
	return BladeFactory.Make((blade) =>
	{
		return new Tessin.Bladerunner.Controls.Button("Crash", blade.Catch(async (_) =>
		{
			var str = await Foo();
		}));
	});
}



