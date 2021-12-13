<Query Kind="Program">
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <NuGetReference>Tessin.Fintech</NuGetReference>
  <NuGetReference>Tessin.Ledger.Data</NuGetReference>
  <NuGetReference>TogoBridge</NuGetReference>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Fintech</Namespace>
  <Namespace>Tessin.Ledger.Data</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	Togo.IsDev = false;
	
	manager.PushBlade(Blade1(), "Blade1");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var ac = new AsyncDataListBox(Shared.QueryProjects, Shared.FindProject);
		
		//var button = new Button("Set User", async (_) => {
		//	await ac.SetValueAsync("fb407699-4987-4fa6-a6ce-a68433c6f7cd");
		//});
		
		var rc = new RefreshPanel(new[] { ac }, () =>
		{
			return ac.SelectedOption;
		}, addPadding:false);
		
		return Layout.Vertical(ac, rc);
	});
}

public static class Shared
{
	public static async Task<IEnumerable<Option>> QueryProjects(string search)
	{
		search = search.Trim();
		return await Togo.Function((LedgerDbContext db) => db.Projects.Where(e => e.HId.StartsWith(search) || e.Title.StartsWith(search))
			.Select(e => new Option(LoanNumber.Format(e.LoanNumber) + ": " + e.Title.Trim(), e.Id)).Take(5).ToListAsync());
	}

	public static async Task<Option> FindProject(object value)
	{
		return await Togo.Function((LedgerDbContext db) => db.Projects.Where(e => e.Id == (Guid)value)
			.Select(e => new Option(LoanNumber.Format(e.LoanNumber) + ": " + e.Title.Trim(), e.Id)).FirstOrDefaultAsync());
	}

}