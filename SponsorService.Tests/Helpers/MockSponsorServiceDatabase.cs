using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Database.Sql;
using Freestylecoding.MockDatabase;

namespace DevSpaceHuntsville.SponsorService.Tests {
	public class MockSponsorServiceDatabase : MockDatabase, ISponsorServiceDatabase {
		public IEventsRepository EventsRepository => new EventsRepository( this );
		public ISponsorLevelsRepository SponsorLevelsRepository => new SponsorLevelsRepository( this );
	}
}
