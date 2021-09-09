<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//https://olegkarasik.wordpress.com/2019/04/16/code-tip-how-to-work-with-asynchronous-event-handlers-in-c/

void Main()
{
	LINQPad.Controls.Control.UnhandledException += (sender, args) =>
	{
		Util.ClearResults();
		args.Dump("UnhandledException");
	};

	new Button("Works", (_) => {
		Bar();
	}).Dump();

	new Button("Don't Work", async (_) =>
	{
		await Foo().ContinueWith(e =>
		{
			if (e.Exception != null)
			{
				throw e.Exception;
			}
			return e.Result;
		}).ConfigureAwait(false);
	}).Dump();

	new Button("Don't Work", async (_) =>
	{
		await Foo();
	}).Dump();
}

static async Task<string> Foo()
{
	await Task.Delay(300);
	throw new Exception("Exception in control handler.");
	return "Hello";
}

static string Bar()
{
	throw new Exception("Exception in control handler.");
	return "Hello";
}




