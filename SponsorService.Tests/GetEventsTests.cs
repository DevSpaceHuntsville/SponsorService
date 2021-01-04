using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DevSpace.Common.Entities;
using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telerik.JustMock;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Tests {
	public class GetEventsTests {
		private readonly ISponsorServiceDatabase Database;
		private readonly GetEvents Controller;
		private readonly IEventsRepository EventsRepository;

		public GetEventsTests() {
			this.Database = Mock.Create<ISponsorServiceDatabase>();
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
				.Select( i => new Event( i, $"DevSpace {i}", DateTime.Today.AddDays( -i ), DateTime.Today.AddDays( i ) ) );

			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Returns( Task.FromResult( expected ) );

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, log );

			Assert.IsType<OkObjectResult>( actual );
			Assert.Equal(
				expected,
				( actual as OkObjectResult ).Value
			);
		}

		[Fact]
		public async Task NoResults() {
			IEnumerable<Event> expected = Enumerable.Empty<Event>();

			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Returns( Task.FromResult( expected ) );

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, log );

			Assert.IsType<OkObjectResult>( actual );
			Assert.Equal(
				expected,
				( actual as OkObjectResult ).Value
			);
		}

		[Fact]
		public async Task Error() {
			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Throws<Exception>();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, log );

			Assert.IsType<InternalServerErrorResult>( actual );
		}

		[Fact]
		public async Task LogsIntroMessage() {
			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Returns( Task.FromResult( Enumerable.Empty<Event>() ) );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.First();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvents processed a request.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessage() {
			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Returns( Task.FromResult( Enumerable.Empty<Event>() ) );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvents finished a request.", actual.Message );
		}

		[Fact]
		public async Task LogsErrorMessage() {
			Exception ex = new Exception();

			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Throws( ex );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			Assert.Equal( 3, log.Data.Count() );

			LogData actual = log.Data.Skip( 1 ).First();
			Assert.Equal( ex, actual.Exception );
			Assert.Equal( LogLevel.Error, actual.LogLevel );
			Assert.Equal( "GetEvents threw an exception.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessageOnError() {
			Exception ex = new Exception();

			Mock
				.Arrange( () => this.EventsRepository.Select( default ) )
				.Throws( ex );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvents finished a request.", actual.Message );
		}
	}
}
