using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Newtonsoft.Json;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Unit {
	public class EventsRepositoryUnitTests : TestBase<EventsRepository> {
		[Fact]
		public async Task Select() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event( id, $"DevSpace {id}", DateTime.Today.AddDays( -id ), DateTime.Today.AddDays( id ) ) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

			Assert.Equal( expected, await Repository.Select() );
		}
	}
}
