using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Xunit;

namespace SponsorService.Database.Sql.Test.Unit {
	public class EventsRepositoryUnitTests {
		[Fact]
		public async Task Get() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event( id, $"DevSpace {id}", DateTime.Today.AddDays( -id ), DateTime.Today.AddDays( id ) ) );


		}
	}
}
