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
  <Namespace>LINQPad.Controls</Namespace>
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
	
	manager.PushBlade(Blade1(), "RadioButtons");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		Guid ola = Guid.NewGuid();
	
		RadioButtons rbs = new RadioButtons(ola, (e) => Layout.Horizontal(e),
			new Option("Niels",Guid.NewGuid()),
			new Option("John",Guid.NewGuid()),
			new Option("Ola", ola),
			new Option("Dennis",Guid.NewGuid())
		);
		
		var chk = new CheckBox("Test");
		
		RefreshPanel rc = new RefreshPanel(new [] {rbs}, () => {
			return rbs.SelectedOption;
		});
		
		return Layout.Vertical(chk, rbs, rc);
	});
}