using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevSpace.Common.Entities;

namespace DevSpaceHuntsville.SponsorService.Database {
	public interface IEventsRepository {
		Task<IEnumerable<Event>> Get( CancellationToken cancelToken = default );
	}
}
