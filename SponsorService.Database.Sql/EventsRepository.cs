using System.Collections.Generic;
using System.Data.Common;
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

		public Task<Event> Select( int key, CancellationToken token = default ) => throw new System.NotImplementedException();
		public Task<IEnumerable<Event>> Select( IEnumerable<int> keys, CancellationToken token = default ) => throw new System.NotImplementedException();
		public async Task<IEnumerable<Event>> Select( CancellationToken token = default )  {
			using DbConnection dbConnection = Database.GetConnection();
			await dbConnection.OpenAsync();

			using DbCommand dbCommand = dbConnection.CreateCommand();
			//dbCommand.CommandText = "SELECT Id, Name FROM Events FOR JSON PATH, INCLUDE_NULL_VALUES;";
			dbCommand.CommandText = "SELECT Id AS id, Name AS name, StartDate AS startDate, EndDate as endDate FROM Events FOR JSON PATH, INCLUDE_NULL_VALUES;";

			using DbDataReader reader = await dbCommand.ExecuteReaderAsync( token );
			return await reader.ReadJson<Event>();
		}
	}
}
