using Freestylecoding.MockDatabase;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Unit {
	public class MockSponsorServiceDatabase : MockDatabase, ISponsorServiceDatabase {
		public IEventsRepository EventsRepository => new EventsRepository( this );
	}
}
