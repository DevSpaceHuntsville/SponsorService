using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevSpace.Common.Entities;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	public class SponsorLevelsRepository : ISponsorLevelsRepository {
		private readonly ISponsorServiceDatabase Database;

		public SponsorLevelsRepository() :
			this( new SqlSponsorServiceDatabase() ) { }
		public SponsorLevelsRepository( ISponsorServiceDatabase database ) =>
			this.Database = database;

		public async Task<SponsorLevel> Select( int key, CancellationToken token = default ) {
			using( DbConnection dbConnection = Database.GetConnection() ) {
				await dbConnection.OpenAsync();

				using( DbCommand dbCommand = dbConnection.CreateCommand() ) {
					dbCommand.CommandText = string.Format( BaseSelectQuery, "WHERE Id = @Id" );
					dbCommand.Parameters.Add(
						Database.CreateParameter( "Id", System.Data.DbType.Int32, key )
					);

					using( DbDataReader reader = await dbCommand.ExecuteReaderAsync( token ) )
						return ( await reader.ReadJson<SponsorLevel>() )
							.FirstOrDefault();
				}
			}
		}
		public async Task<IEnumerable<SponsorLevel>> Select( IEnumerable<int> keys, CancellationToken token = default ) => throw new System.NotImplementedException();
		public async Task<IEnumerable<SponsorLevel>> Select( CancellationToken token = default ) {
			using( DbConnection dbConnection = Database.GetConnection() ) {
				await dbConnection.OpenAsync();

				using( DbCommand dbCommand = dbConnection.CreateCommand() ) {
					dbCommand.CommandText = string.Format( BaseSelectQuery, string.Empty );

					using( DbDataReader reader = await dbCommand.ExecuteReaderAsync( token ) )
						return await reader.ReadJson<SponsorLevel>();
				}
			}
		}

		public SponsorLevel BlockingSelect( int key ) => throw new System.NotImplementedException();
		public IEnumerable<SponsorLevel> BlockingSelect( IEnumerable<int> keys ) => throw new System.NotImplementedException();		public IEnumerable<SponsorLevel> BlockingSelect() => throw new System.NotImplementedException();

		private const string BaseSelectQuery = @"
SELECT
	Id,
	DisplayOrder,
	[Name],
	Cost,
	DisplayInEmails,
	DisplayInSidebar,
	DisplayLink,
	TimeOnScreen,
	Tickets,
	Discount,
	PreConEmail,
	MidConEmail,
	PostConEmail
FROM SponsorLevels
{0}
ORDER BY DisplayOrder
FOR JSON PATH, INCLUDE_NULL_VALUES;";
	}
}