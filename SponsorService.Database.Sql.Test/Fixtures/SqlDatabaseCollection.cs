using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test {
	[CollectionDefinition( "SqlDatabase" )]
	public class SqlDatabaseCollection
		: ICollectionFixture<SqlDatabaseFixture> {}
}
