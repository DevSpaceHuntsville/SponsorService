using DevSpace.Common.Entities;
using Freestylecoding.MockableDatabase;

namespace DevSpaceHuntsville.SponsorService.Database {
	public interface IEventsRepository : ISelectable<Event,int> {}
}
