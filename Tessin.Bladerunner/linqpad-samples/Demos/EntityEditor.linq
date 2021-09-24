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
  <Namespace>Tessin.Bladerunner.Editors</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

static IDbConnection connection;

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	manager.Dump();
	
	manager.PushBlade(Blade1(), "Blade1");
	
	connection = Connection;
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make(async (blade) =>
	{		
		var db = new TypedDataContext(connection);
		
		var segment = db.ChannelSegments.Where(e => e.Id == 28).FirstOrDefault();

		var segmentTypes = db.ChannelSegmentTypes.ToList();
		var dealTypes = db.ChannelSegmentDealTypes.ToList();
		var costModels = db.ChannelCostModels.ToList();

		return new EntityEditor<ChannelSegments>(segment, (_) => {
					
		})
		.Required(e => e.Title)
		.Validate(e => e.Title, (x) => (false, "Foo"))
		.Remove(e => e.Id)
		.Remove(e => e.ChannelId)
		.Editor(e => e.TypeId, e => e.Select(segmentTypes.Select(f => new Option(f.DescriptionText, f.Id))))
		.Label(e => e.TypeId, "Segment Type")
		.Editor(e => e.CostModelId, e => e.Select(costModels.Select(f => new Option(f.DescriptionText, f.Id))))
		.Label(e => e.CostModelId, "Cost Model")
		.Editor(e => e.DealTypeId, e => e.Select(dealTypes.Select(f => new Option(f.DescriptionText, f.Id))))
		.Label(e => e.DealTypeId, "Deal Type")
		.Description(e => e.Title, "HelloWorld")
		.Editor(e => e.Comment, e => e.Text(multiLine:true))
		.Group("Deal Conditions", e => e.DealPrice, e => e.DealTypeId)
		.Place(e => e.Title)
		.Place(true, e => e.ActiveFrom, e => e.ActiveTo)
		.Place(2, e => e.Comment, e => e.DealPrice, e => e.DealTypeId)
		.Helper(e => e.Comment, x => ((ITextControl)x).Text.Length)
		.Render();
	});
}