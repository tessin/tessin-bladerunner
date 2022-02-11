<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{
	//Debugger.Launch();
	
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", showDebugButton: true, cssHotReloading: true);
	manager.Push(Blade1(), "Blade1");
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return Layout.Vertical( 
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
			}),
			new Button("Progress With Sideblade", async (_) =>
			{
				blade.Manager.OpenSideBlade(new DisplayBlade("Hello"));
				
				using (blade.Manager.ShowProgress(title: "Working on it..."))
				{
					await Task.Delay(TimeSpan.FromSeconds(1000));
				}
			})
		);
	});
}

