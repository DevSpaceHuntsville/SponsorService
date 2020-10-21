using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	public class SqlDatabase : IDatabase {
		public SqlDatabase()
			: this(
				Environment.GetEnvironmentVariable( SQL_CONNSTR_VARIABLE_NAME, EnvironmentVariableTarget.Process )
				?? ConfigurationManager.ConnectionStrings[SQL_CONNSTR_VARIABLE_NAME]?.ConnectionString
				?? "Server=localhost;Database=DevSpace;Trusted_Connection=True;"
			) { }

		public SqlDatabase( string connectionString ) {
			this.ConnectionString = connectionString;
			UpdateDatabase();
		}

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

		#region IDatabase
		public DbConnection GetConnection() =>
			new SqlConnection( this.ConnectionString );
		public DbParameter CreateParameter( string parameterName, DbType dbType, object value ) =>
			new SqlParameter {
				Value = value ?? DBNull.Value,
				ParameterName = parameterName,
				DbType = dbType
			};
		public DbParameter CreateParameter( string parameterName, DbType dbType, int size, object value ) =>
			new SqlParameter {
				Value = value ?? DBNull.Value,
				ParameterName = parameterName,
				DbType = dbType,
				Size = size
			};

		public IEventsRepository EventsRepository => new EventsRepository( this );
		#endregion

		private const string SQL_CONNSTR_VARIABLE_NAME = "SQL:Sponsors";

		private readonly string ConnectionString;
	}
}
