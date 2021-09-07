<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
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
	Util.CreateSynchronizationContext();
	
	Application.ThreadException += (sender, args) =>
	{
		Util.ClearResults();
		sender.Dump("sender");
		args.Dump("args");
	};

	System.AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
	{
		Util.ClearResults();
		sender.Dump("sender");
		args.Dump("args:System.AppDomain");
	};

	LINQPad.Controls.Control.UnhandledException += (sender, args) =>
	{
		Util.ClearResults();
		sender.Dump("sender");
		args.Dump("args:LINQPad.Controls");
	};

	BladeManager manager = new BladeManager();
	manager.PushBlade(Blade1(), "");
	manager.PushBlade(Blade2(), "");
	manager.PushBlade(Blade3(), "");
	manager.PushBlade(Blade4(), "");
	manager.Dump();
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
		return new LINQPad.Controls.Button("Crash", (_) => {
			throw new Exception("Exception in control handler.");
		});	
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
		return new LINQPad.Controls.Button("Crash", async (_) =>
		{
			var str = await Foo();
		});
	});
}




