using System.Data;
using System.Data.Common;
using Freestylecoding.MockableDatabase;

namespace DevSpaceHuntsville.SponsorService.Database {
	public interface ISponsorServiceDatabase : IDatabase {
		IEventsRepository EventsRepository { get; }
	}
}
