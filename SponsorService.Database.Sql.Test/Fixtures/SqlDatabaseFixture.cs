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
			DatabaseName = CreateDatabase( MasterConnectionString );

			scsb.InitialCatalog = DatabaseName;
			TargetConnectionString = scsb.ToString();
		}

		public void Dispose() => DeleteDatabase();

		private string CreateDatabase( string connectionString ) {
			string name = $"_{Guid.NewGuid():N}";

			using SqlConnection connection = new SqlConnection( connectionString );
			connection.Open();

			using SqlCommand command = connection.CreateCommand();
			command.CommandText = $"CREATE DATABASE {name};";
			command.ExecuteNonQuery();

			return name;
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
