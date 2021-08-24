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
	
	manager.PushBlade(Blade1(), "Typography");
	
	manager.Dump();
}

static IBladeRenderer Blade1()
{
	return BladeFactory.Make((blade) =>
	{		
		return Layout.Vertical(true, 
			Typography.H1("H1"),
			Typography.H2("H2"),
			Typography.Error("Error"),
			Typography.Small("Small"),
			Typography.P("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consectetur, quam non feugiat lobortis, quam elit elementum sapien, in interdum sem sem sit amet risus. In suscipit, lectus ac aliquet eleifend, dolor tortor molestie dolor, a finibus erat orci non dui. Etiam eget felis sapien. Cras risus mi, ornare porttitor varius ac, aliquet at lacus. Nullam egestas libero lectus, vitae venenatis tellus tincidunt et. Vestibulum eu ante tristique, porttitor quam vel, elementum lacus. Nam facilisis hendrerit libero molestie lacinia. Proin euismod dapibus magna. Nullam urna nibh, faucibus sed arcu ac, finibus rhoncus velit. Proin vitae urna turpis. Maecenas id pellentesque nulla. Fusce nec augue id enim feugiat condimentum.")
		);
	});
}