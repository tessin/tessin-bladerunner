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
  <Reference>C:\Repos\tessin-bladerunner\Tessin.Bladerunner\bin\Debug\netcoreapp3.1\Tessin.Bladerunner.dll</Reference>
  <NuGetReference>LinqKit</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Tessin.Bladerunner</Namespace>
  <Namespace>Tessin.Bladerunner.Blades</Namespace>
  <Namespace>Tessin.Bladerunner.Controls</Namespace>
  <Namespace>Tessin.Bladerunner.Query</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

void Main()
{	
	//https://stackoverflow.com/questions/54870036/how-to-add-checkboxes-to-multi-select-in-ie-11
	
	//Validating filters
	
	//Saving and loading queries -> Import/Export?
	
	//Sort rules alphabetically
	
	//NullableRules?
	
	//DateRule
	
	//SelectBox => Allow to select more than one
	
	//Debugger.Launch();
	
	BladeManager manager = new BladeManager(cssPath: @"C:\Repos\tessin-bladerunner\Tessin.Bladerunner\Themes\Sass\default.css", showDebugButton:true, cssHotReloading:true);

	manager.PushBlade(SearchBlade(), "Projects");
	
	manager.Dump();
	
	//Util.KeepRunning();
}

static IBladeRenderer SearchBlade()
{
	return BladeFactory.Make((blade) =>
	{
		var projectExpr = new RefreshValue<Expression<Func<Projects,bool>>>(null);
		
		SearchBox txtSearch = null; 
		Action<object> onClose = null;
		
		onClose = (result) =>
		{
			if (result is QueryBuilder<Projects> queryBuilder)
			{
				projectExpr.Value = queryBuilder.ToExpression();
				//todo
				//txtSearch?.SetExternal("Query", (_) => {
				//	blade.Manager.OpenSideBlade(QueryBlade(queryBuilder), onClose);	
				//});
			}
		};

		var menu = new ContextMenu(
			blade.Manager,
			new IconButton(Icons.DotsHorizontal),
			new ContextMenu.Item("Query", (_) =>
			{
				blade.Manager.OpenSideBlade(QueryBlade(), onClose);
			}, icon: Icons.Plus)
		);

		txtSearch = new SearchBox(contextMenu: menu);

		MenuButton BuildMenuButton(Guid projectId, string title, long? loanNumber, string developer)
		{
			return new MenuButton(title, (_) =>
			{
				
			});
		}

		var refreshPanel = new RefreshPanel(new object[] { txtSearch, projectExpr }, () =>
		{
			using (var db = new TypedDataContext())
			{
				var linq = db.Projects.Where(e => !e.DeleteFlag);

				var searchText = txtSearch.Text.Trim();

				if(projectExpr.Value != null)
				{
					linq = linq.Where(projectExpr.Value);
				}
				else if (!string.IsNullOrEmpty(txtSearch.Text))
				{
					Guid projectId;
					if (Guid.TryParse(searchText, out projectId))
					{
						linq = linq.Where(e => e.Id.Equals(projectId));
					}
					else
					{
						linq = linq.Where(e => e.InternalRefr.Contains(searchText)
						   || e.Title.Contains(searchText)
						   || e.HId.Contains(searchText)
						   || e.Developer.EntityName.Contains(searchText)
						   || e.Developer.Presentation.Contains(searchText)
						   || e.Developer.GovId.Contains(searchText)
						   || e.Developer.ContactName.Contains(searchText)
						   || e.Developer.ContactEmail.Contains(searchText)
						   || e.Developer.Email.Contains(searchText)
						   || e.Developer.MotherEntityName.Contains(searchText)
						   || e.Developer.MotherEntityGovId.Contains(searchText)
						);
					}
				}

				var cards = linq.OrderBy(e => e.Title)
				.Select(e => new
				{
					e.Id,
					e.Title,
					e.LoanNumber,
					Developer = e.Developer.EntityName
				})
				.ToList()
				.Select(e => BuildMenuButton(e.Id, e.Title, e.LoanNumber, e.Developer)).ToList();
				return new Menu(cards.ToArray());
			}
		});

		return Util.VerticalRun(txtSearch, refreshPanel);
	});
}

static QueryBuilder<Projects> CreateQueryBuilder()
{
	var db = new TypedDataContext();

	var queryBuilder = new QueryBuilder<Projects>();
	queryBuilder.AddRule("Id", e => e.Id);
	queryBuilder.AddRule("Title", e => e.Title);
	queryBuilder.AddRule("Internal Refr", e => e.InternalRefr);
	queryBuilder.AddRule("Archived", e => e.Archived);
	queryBuilder.AddRule("Invested Amount", e => e.ProjectCommitments.Sum(f => f.InvestedAmount));
	queryBuilder.AddRule("Repayment Status", e => e.RepaymentStatus.DescriptionText, db.ProjectRepaymentStatuses.Select(e => e.DescriptionText).OrderBy(e => e).ToArray());
	queryBuilder.AddRule("Publish", e => e.Publish);
	queryBuilder.AddRule("Developer", e => e.Developer.EntityName);
	queryBuilder.AddRule("Developer GovId", e => e.Developer.GovId);
	return queryBuilder;
}


static IBladeRenderer QueryBlade(QueryBuilder<Projects> builder = null)
{
	return BladeFactory.Make((blade) =>
	{		
		var queryBuilder = builder ?? CreateQueryBuilder();
		
		var btnUse = new Button("Search", (_) =>
		{
			blade.Manager.CloseSideBlade(queryBuilder);
		});

		return Layout.Vertical(
			new Spacer("500px"),
			btnUse,
			queryBuilder.Render()
		);
	});
}