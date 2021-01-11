using System;
using System.Data.SqlClient;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test {
	public class SqlDatabaseFixture : IDisposable {
		public readonly string DatabaseName;
		public readonly string TargetConnectionString;
		private readonly string MasterConnectionString;

		public SqlDatabaseFixture() {
			SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder {
				InitialCatalog = "master",
				DataSource = "localhost",
				IntegratedSecurity = true
			};

			MasterConnectionString = scsb.ToString();
			DatabaseName = CreateDatabase();

			scsb.InitialCatalog = DatabaseName;
			TargetConnectionString = scsb.ToString();

			// YES, this looks strange
			// The ctor for SqlSponsorServiceDatabase runs the creation scripts
			// We need the DB fully created for we add the test data
			// However, we don't need the actual object it creates
			new SqlSponsorServiceDatabase( TargetConnectionString );
			CreateDefaultData();
		}

		public void Dispose() => DeleteDatabase();

		private string CreateDatabase() {
			string name = $"_{Guid.NewGuid():N}";

			using SqlConnection connection = new SqlConnection( MasterConnectionString );
			connection.Open();

			using SqlCommand command = connection.CreateCommand();
			command.CommandText = $"CREATE DATABASE {name};";
			command.ExecuteNonQuery();

			return name;
		}

		private void CreateDefaultData() {
			using SqlConnection connection = new SqlConnection( TargetConnectionString );
			connection.Open();

			using SqlCommand command = connection.CreateCommand();
			command.CommandText = $@"
INSERT Events ( Id, Name, StartDate, EndDate ) VALUES
	( 2015, 'DevSpace 2015', '2015-10-15 00:00:00', '2015-10-16 00:00:00' ),
	( 2016, 'DevSpace 2016', '2016-10-14 00:00:00', '2016-10-15 00:00:00' ),
	( 2017, 'DevSpace 2017', '2017-10-13 00:00:00', '2017-10-14 00:00:00' ),
	( 2018, 'DevSpace 2018', '2018-10-12 00:00:00', '2018-10-13 00:00:00' ),
	( 2019, 'DevSpace 2019', '2019-10-11 00:00:00', '2019-10-12 00:00:00' ),
	( 2020, 'DevSpace 2020', '2020-09-11 00:00:00', '2020-09-11 00:00:00' );
";
			command.ExecuteNonQuery();
		}

		private void DeleteDatabase() {
			using SqlConnection connection = new SqlConnection( MasterConnectionString );
			connection.Open();

			using SqlCommand command = connection.CreateCommand();
			command.CommandText = $@"
ALTER DATABASE {DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE {DatabaseName};";
			command.ExecuteNonQuery();
		}
	}
}
