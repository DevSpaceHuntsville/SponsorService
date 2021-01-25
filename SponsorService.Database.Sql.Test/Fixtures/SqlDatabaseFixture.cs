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

			using( SqlConnection connection = new SqlConnection( MasterConnectionString ) ) {
				connection.Open();

				using( SqlCommand command = connection.CreateCommand() ) {
					command.CommandText = $"CREATE DATABASE {name};";
					command.ExecuteNonQuery();

					return name;
				}
			}
		}

		private void CreateDefaultData() {
			using( SqlConnection connection = new SqlConnection( TargetConnectionString ) ) {
				connection.Open();

				using( SqlCommand command = connection.CreateCommand() ) {
					command.CommandText = $@"
INSERT Events ( Id, Name, StartDate, EndDate ) VALUES
	( 2015, 'DevSpace 2015', '2015-10-15 00:00:00', '2015-10-16 00:00:00' ),
	( 2016, 'DevSpace 2016', '2016-10-14 00:00:00', '2016-10-15 00:00:00' ),
	( 2017, 'DevSpace 2017', '2017-10-13 00:00:00', '2017-10-14 00:00:00' ),
	( 2018, 'DevSpace 2018', '2018-10-12 00:00:00', '2018-10-13 00:00:00' ),
	( 2019, 'DevSpace 2019', '2019-10-11 00:00:00', '2019-10-12 00:00:00' ),
	( 2020, 'DevSpace 2020', '2020-09-11 00:00:00', '2020-09-11 00:00:00' );

INSERT SponsorLevels ( DisplayOrder, Name, Cost, DisplayInEmails, DisplayInSidebar, DisplayLink, TimeOnScreen, Tickets, Discount, PreConEmail, MidConEmail, PostConEmail )
VALUES
	(  1,	'Premere',			10000,	1,	1,	1,	0,	15,	25,	1,	1,	1 ),
	(  2,	'Diamond',			5000,	1,	1,	1,	0,	10,	15,	1,	1,	0 ),
	(  3,	'Meal',				5000,	1,	1,	1,	0,	10,	15,	1,	1,	0 ),
	(  4,	'USB',				3000,	1,	1,	1,	0,	10,	10,	1,	0,	0 ),
	(  5,	'Social',			2500,	1,	1,	1,	0,	8,	10,	1,	0,	0 ),
	(  7,	'Snack',			2500,	0,	0,	1,	0,	8,	10,	1,	0,	0 ),
	(  6,	'Gold',				2000,	0,	1,	1,	30,	8,	10,	1,	0,	0 ),
	(  8,	'Supplies',			1000,	0,	0,	1,	0,	5,	5,	0,	0,	0 ),
	(  9,	'Silver',			1000,	0,	0,	1,	0,	5,	5,	0,	0,	0 ),
	( 10,	'Bronze',			500,	0,	0,	1,	0,	3,	0,	0,	0,	0 ),
	( 11,	'In-Kind',			0,		0,	0,	1,	0,	0,	0,	0,	0,	0 ),
	( 12,	'Amazing Sponsors',	500,	0,	1,	1,	60,	0,	0,	0,	0,	0 ),
	( 13,	'Special Sponsors',	250,	0,	0,	1,	30,	0,	0,	0,	0,	0 ),
	( 14,	'Image Sponsors',	100,	0,	0,	1,	15,	0,	0,	0,	0,	0 ),
	( 15,	'Link Sponsors',	50,		0,	0,	1,	0,	0,	0,	0,	0,	0 ),
	( 16,	'Sponsors',			1,		0,	0,	0,	0,	0,	0,	0,	0,	0 );";
					command.ExecuteNonQuery();
				}
			}
		}

		private void DeleteDatabase() {
			using( SqlConnection connection = new SqlConnection( MasterConnectionString ) ) {
				connection.Open();

				using( SqlCommand command = connection.CreateCommand() ) {
					command.CommandText = $@"
ALTER DATABASE {DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE {DatabaseName};";
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
