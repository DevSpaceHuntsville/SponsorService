using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using DevSpace.Common.Entities;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	public class EventsRepository : IEventsRepository {
		private readonly IDatabase Database;

		public EventsRepository()
			: this( new SqlDatabase() ) { }
		public EventsRepository( IDatabase database ) {
			this.Database = database;
		}

		public async Task<IEnumerable<Event>> Get( CancellationToken cancelToken = default ) {
			using DbConnection dbConnection = Database.GetConnection();
			await dbConnection.OpenAsync();

			using DbCommand dbCommand = dbConnection.CreateCommand();
			//dbCommand.CommandText = "SELECT Id, Name FROM Events FOR JSON PATH, INCLUDE_NULL_VALUES;";
			dbCommand.CommandText = "SELECT Id AS id, Name AS name, StartDate AS startDate, EndDate as endDate FROM Events FOR JSON PATH, INCLUDE_NULL_VALUES;";

			using DbDataReader reader = await dbCommand.ExecuteReaderAsync( cancelToken );
			return await reader.ReadJson<Event>();
		}
	}
}
