using System;
using System.Data.SqlClient;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Integration {
	public class TestBase<T> where T : class {
		protected readonly T Repository;
		protected readonly SqlSponsorServiceDatabase Database;

		public TestBase( SqlDatabaseFixture fixture ) {
			Database = new SqlSponsorServiceDatabase( fixture.TargetConnectionString );

			this.Repository = typeof( T )
				.GetConstructor( new[] { typeof( ISponsorServiceDatabase ) } )
				.Invoke( new object[] { this.Database } )
				as T;
		}
	}
}
