using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevSpace.Common.Entities;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	public class EventsRepository : IEventsRepository {
		private readonly ISponsorServiceDatabase Database;

		public EventsRepository()
			: this( new SqlSponsorServiceDatabase() ) { }
		public EventsRepository( ISponsorServiceDatabase database ) {
			this.Database = database;
		}

		public Event BlockingSelect( int key ) => throw new System.NotImplementedException();
		public IEnumerable<Event> BlockingSelect( IEnumerable<int> keys ) => throw new System.NotImplementedException();
		public IEnumerable<Event> BlockingSelect() => throw new System.NotImplementedException();

		public async Task<Event> Select( int key, CancellationToken token = default ) {
			using DbConnection dbConnection = Database.GetConnection();
			await dbConnection.OpenAsync();

			using DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format(
				BaseSelectQuery,
				"WHERE Id = @Id"
			);

			dbCommand.Parameters.Add( Database.CreateParameter( "Id", DbType.Int32, key ) );

			using DbDataReader reader = await dbCommand.ExecuteReaderAsync( token );
			return ( await reader.ReadJson<Event>() ).FirstOrDefault();
		}
		public async Task<IEnumerable<Event>> Select( IEnumerable<int> keys, CancellationToken token = default ) {
			if( !keys?.Any() ?? true ) return Enumerable.Empty<Event>();

			using DbConnection dbConnection = Database.GetConnection();
			await dbConnection.OpenAsync();

			using DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format(
				BaseSelectQuery,
				$"WHERE Id IN ( {string.Join( ", ", keys )} )"
			);

			using DbDataReader reader = await dbCommand.ExecuteReaderAsync( token );
			return await reader.ReadJson<Event>();
		}
		public async Task<IEnumerable<Event>> Select( CancellationToken token = default ) {
			using DbConnection dbConnection = Database.GetConnection();
			await dbConnection.OpenAsync();

			using DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format( BaseSelectQuery, string.Empty );

			using DbDataReader reader = await dbCommand.ExecuteReaderAsync( token );
			return await reader.ReadJson<Event>();
		}

		private const string BaseSelectQuery = @"
SELECT
	Id AS id,
	Name AS name,
	StartDate AS startDate,
	EndDate as endDate
FROM Events
{0}
FOR JSON PATH, INCLUDE_NULL_VALUES;";
	}
}
