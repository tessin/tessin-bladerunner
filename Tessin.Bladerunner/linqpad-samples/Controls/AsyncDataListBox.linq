<Query Kind="Program">
  <Connection>
    <ID>267fadab-00a0-4cb2-8e87-82deb2eeba5c</ID>
    <Server>tessin-prod-q3j2jdhga4u26.database.windows.net</Server>
    <Database>tessin-prod</Database>
    <UserName>niels@tessin.com</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAJmcqpNpu20uU3NaGwEEj1QAAAAACAAAAAAAQZgAAAAEAACAAAAA6BjDHBE2B1YISI3OOv3kDRAseCRqE7u0QaIS8PEFc3QAAAAAOgAAAAAIAACAAAAC5dOt038oP0I0o/G23+LZlQpbW+uvQ2jsZuSD1KV2JYTAAAACNdZsqB/XbHlDhRasBXj4WCX0iQrxzpDw5trkImclBxlnkgDpp8hlRZ7PBAt3EcFNAAAAAf1Jx7SyDWnnbfg4m+ZqyS4pucO4eyuw86ckhcYnlJI2SBID5QGJZfsRsLS+xJiW7XF1JHKCWjkZRGbc6lXGy0g==</Password>
    <DbVersion>Azure</DbVersion>
    <IsProduction>true</IsProduction>
    <DisplayName>Tessin-Prod</DisplayName>
    <UniversalAuthentication>true</UniversalAuthentication>
  </Connection>
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		var ac = new AsyncDataListBox(QueryUsers, FindUser);
		
		var button = new Button("Set User", async (_) => {
			await ac.SetValueAsync("fb407699-4987-4fa6-a6ce-a68433c6f7cd");
		});
		
		var rc = new RefreshPanel(new[] { ac }, () =>
		{
			return ac.SelectedOption;
		}, addPadding:false);
		
		return Layout.Vertical(button, ac, rc);
	});
}

static async Task<IEnumerable<Option>> QueryUsers(string search)
{
	using(var db = new TypedDataContext())
	{
		return await Task.FromResult(
			db.AspNetUsers.Where(e => e.UserName.StartsWith(search)).Select(e => new Option(e.UserName, e.Id)).Take(5).ToList()
		);
	}
}

static async Task<Option> FindUser(object value)
{
	using (var db = new TypedDataContext())
	{
		return await Task.FromResult(
			db.AspNetUsers.Where(e => e.Id == value.ToString()).Select(e => new Option(e.UserName, e.Id)).FirstOrDefault()
		);
	}
}
