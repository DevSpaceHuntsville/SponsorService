using Freestylecoding.MockableDatabase;

namespace DevSpaceHuntsville.SponsorService.Database {
	public interface ISponsorServiceDatabase : IDatabase {
		IEventsRepository EventsRepository { get; }
		ISponsorLevelsRepository SponsorLevelsRepository { get; }
	}
}
