<Query Kind="Program">
  <Connection>
    <ID>ab4f23ff-9abb-40f7-9b23-863370e15152</ID>
    <Persist>true</Persist>
    <Server>tessin-prod-q3j2jdhga4u26.database.windows.net</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>tessin-prod</Database>
    <UserName>tessin</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAACe+cS1dyVUGiFERUvU8QlgAAAAACAAAAAAAQZgAAAAEAACAAAABb3OP8bsuJwIrNPKB+ILsgdnBXclTFDX/da7IRCu/2LAAAAAAOgAAAAAIAACAAAADFkA1f/WVzhaEv+ibPMYRKXfCBsamjC06esMNT9enBPjAAAADUeZTXRCzJw+bGTMQGfYU6YP5X3K2OAIEVT+fq1nE3nm7IZTA1sUAf3GwtQ7wzbuJAAAAAjtbeXAzOQ6Gzq/AQUVj5y5UFM8X3YBLdkhz+QgRCWyTsUvUJP8ZkK11e29HUdyeSdBUMF8x9Su9kbEG6KvLxcg==</Password>
    <DbVersion>Azure</DbVersion>
    <IsProduction>true</IsProduction>
    <DisplayName>Tessin-Production</DisplayName>
  </Connection>
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\net5.0\Tessin.Bladerunner.dll</Reference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//Debugger.Launch();

	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", cssHotReloading: true);
	
	manager.PushBlade(Blade1(), "CollapsablePanel");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{
		return 
			Layout.Vertical(true,
			new Field("Title", new TextBox(), ""),
			new CollapsablePanel("Contact", Layout.Vertical(true,
				new Field("Name", new TextBox(), ""),
				new Field("Email", new TextBox(), "")
			)),
			new Field("Company", new TextBox(), "")
		);
	});
}