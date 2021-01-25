using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Freestylecoding.MockableSqlDatabase;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	public class SqlSponsorServiceDatabase : SqlDatabase, ISponsorServiceDatabase {
		public SqlSponsorServiceDatabase()
			: this(
				Environment.GetEnvironmentVariable( SQL_CONNSTR_VARIABLE_NAME, EnvironmentVariableTarget.Process )
				?? ConfigurationManager.ConnectionStrings[SQL_CONNSTR_VARIABLE_NAME]?.ConnectionString
				?? "Server=localhost;Database=DevSpace;Trusted_Connection=True;"
			) { }		
		public SqlSponsorServiceDatabase( string connectionString )
			: base( connectionString ) {
			UpdateDatabase();
		}

		public IEventsRepository EventsRepository => new EventsRepository( this );

		public ISponsorLevelsRepository SponsorLevelsRepository => new SponsorLevelsRepository( this );

		private void UpdateDatabase() {
			using( SqlConnection connection = new SqlConnection( ConnectionString ) ) {
				connection.Open();
				bool updatesTableExists = false;
 
				using( SqlCommand command = connection.CreateCommand() ) {
					command.CommandText = "SELECT COUNT( 1 ) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Updates';";
					updatesTableExists = 1 == Convert.ToInt32( command.ExecuteScalar() );
				}

				List<string> appliedUpdates = new List<string>();
				if( updatesTableExists ) {
					using( SqlCommand command = connection.CreateCommand() ) {
						command.CommandText = "SELECT * FROM Updates;";

						using( SqlDataReader reader = command.ExecuteReader() ) {
							while( reader.Read() )
								appliedUpdates.Add( reader.GetString( 0 ) );
						}
					}
				}

				IEnumerable<string> availableUpdates = Assembly
					.GetExecutingAssembly()
					.GetManifestResourceNames()
					.Except( appliedUpdates );

				foreach( string updateResource in availableUpdates ) {
					using( SqlTransaction transaction = connection.BeginTransaction() ) {
						try {
							string[] updates = Regex.Split(
								new StreamReader(
									Assembly
										.GetExecutingAssembly()
										.GetManifestResourceStream( updateResource )
								)
									.ReadToEnd()
									.Replace( "\r", "" ),
								"^GO$",
								RegexOptions.Multiline
							);

							foreach( string update in updates ) {
								using( SqlCommand command = connection.CreateCommand() ) {
									command.CommandText = update;
									command.Transaction = transaction;
									command.ExecuteNonQuery();
								}
							}

							using( SqlCommand command = connection.CreateCommand() ) {
								command.CommandText = $"INSERT Updates VALUES ( '{updateResource}' )";
								command.Transaction = transaction;
								command.ExecuteNonQuery();
							}

							transaction.Commit();
						} catch {
							transaction.Rollback();
							throw;
						}
					}
				}
			}
		}

		private const string SQL_CONNSTR_VARIABLE_NAME = "SQL:Sponsors";
	}
}
