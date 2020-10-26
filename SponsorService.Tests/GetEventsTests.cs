using System;
using Xunit;
using Telerik.JustMock;
using DevSpaceHuntsville.SponsorService.Database;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using DevSpace.Common.Entities;
using Telerik.JustMock.Helpers;
using System.Collections.Generic;

namespace SponsorService.Tests {
	public class GetEventsTests {
		private readonly IDatabase Database;
		private readonly GetEvents Controller;
		private readonly IEventsRepository EventsRepository;

		public GetEventsTests() {
			this.Database = Mock.Create<IDatabase>();
			this.EventsRepository = Mock.Create<IEventsRepository>();

			Mock
				.Arrange( () => this.Database.EventsRepository )
				.Returns( this.EventsRepository );

			IConfiguration config = Mock.Create<IConfiguration>();

			this.Controller = new GetEvents( config, this.Database );
		}

		[Fact]
		public async Task Run() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 4 )
				.Select( i => new Event( i, $"DevSpace {i}" ) );

			Mock
				.Arrange( () => this.EventsRepository.Get( default ) )
				.Returns( Task.FromResult( expected ) );

			IActionResult actual = await Controller.Run( null, Mock.Create<ILogger>() );

			Assert.IsType<OkObjectResult>( actual );
			Assert.Equal(
				expected,
				( actual as OkObjectResult ).Value
			);
		}
	}
}
