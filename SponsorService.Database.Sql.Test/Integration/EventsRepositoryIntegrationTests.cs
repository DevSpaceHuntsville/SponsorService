using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Integration {
	public class EventsRepositoryIntegrationTests : TestBase<EventsRepository> {
		#region Task<IEnumerable<Event>> Get( CancellationToken cancelToken = default )
		[Fact]
		public async Task Get() {
			IEnumerable<Event> expected = new[] {
				new Event( 2015, "DevSpace 2015", DateTime.Parse( "2015-10-15 00:00:00" ), DateTime.Parse( "2015-10-16 00:00:00" ) ),
				new Event( 2016, "DevSpace 2016", DateTime.Parse( "2015-10-14 00:00:00" ), DateTime.Parse( "2015-10-15 00:00:00" ) ),
				new Event( 2017, "DevSpace 2017", DateTime.Parse( "2015-10-13 00:00:00" ), DateTime.Parse( "2015-10-14 00:00:00" ) ),
				new Event( 2018, "DevSpace 2018", DateTime.Parse( "2015-10-12 00:00:00" ), DateTime.Parse( "2015-10-13 00:00:00" ) ),
				new Event( 2019, "DevSpace 2019", DateTime.Parse( "2015-10-11 00:00:00" ), DateTime.Parse( "2015-10-12 00:00:00" ) ),
				new Event( 2020, "DevSpace 2020", DateTime.Parse( "2020-09-11 00:00:00" ), DateTime.Parse( "2020-09-11 00:00:00" ) )
			};

			IEnumerable<Event> actual = await Repository.Select();

			Assert.Equal( expected, actual );
		}
		#endregion
	}
}
