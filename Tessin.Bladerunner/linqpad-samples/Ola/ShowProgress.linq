<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference Relative="..\..\bin\Debug\netstandard2.0\Tessin.Bladerunner.dll">C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netstandard2.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//Debugger.Launch();
	
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", showDebugButton: true, cssHotReloading: true);
	manager.PushBlade(Blade1(), "Blade1");
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout.Vertical(true, 
			new Button("Progress", async (_) =>
			{
				using(IDisposable ff = blade.Manager.ShowProgress())
				{
					await Task.Delay(TimeSpan.FromSeconds(5));
				}
			}),
			new Button("Progress With Title", async (_) =>
			{
				using (IDisposable ff = blade.Manager.ShowProgress(title:"Working on it..."))
				{
					await Task.Delay(TimeSpan.FromSeconds(5));
				}
			}),
			new Button("Progress With Abort+Progress", async (_) =>
			{
				var cts = new CancellationTokenSource();
				Progress<int> progress = new Progress<int>();
				using (IDisposable ff = blade.Manager.ShowProgress(title: "Working on it...", progress:progress, cancellationTokenSource:cts))
				{
					try
					{
						for (var i = 0; i < 100; ++i)
						{
							((IProgress<int>)progress).Report(i);
							if (cts.Token.IsCancellationRequested)
							{
								break;
							}
							await Task.Delay(TimeSpan.FromMilliseconds(100), cts.Token);
						}
					} 
					catch(TaskCanceledException)
					{
						//ignore
					}
				}
			})
		);
	});
}

