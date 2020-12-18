using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Database.Sql;

namespace SponsorService.Database.Sql.Test.Integration {
	public class TestBase<T> where T : class {
		protected readonly T Repository;
		protected readonly SqlDatabase Database;

		public TestBase() {
			SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder {
				InitialCatalog = "DevSpaceNew",
				DataSource = "localhost",
				IntegratedSecurity = true
			};

			Database = new SqlDatabase( scsb.ToString() );
			this.Repository = typeof( T )
				.GetConstructor( new[] { typeof( IDatabase ) } )
				.Invoke( new object[] { this.Database } )
				as T;
		}
	}
}
