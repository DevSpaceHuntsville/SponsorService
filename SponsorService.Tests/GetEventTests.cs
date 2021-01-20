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
	public class GetEventTests {
		private readonly ISponsorServiceDatabase Database;
		private readonly GetEvent Controller;
		private readonly IEventsRepository EventRepository;

		public GetEventTests() {
			this.Database = Mock.Create<ISponsorServiceDatabase>();
			this.EventRepository = Mock.Create<IEventsRepository>();

			Mock
				.Arrange( () => this.Database.EventsRepository )
				.Returns( this.EventRepository );

			IConfiguration config = Mock.Create<IConfiguration>();

			this.Controller = new GetEvent( config, this.Database );
		}

		[Fact]
		public async Task Run() {
			Event expected = new Event( 1, $"Test Event 1", DateTime.Today.AddDays( -1 ), DateTime.Today.AddDays( 1 ) );

			Mock
				.Arrange( () => this.EventRepository.Select( 1, default ) )
				.Returns( Task.FromResult( expected ) );

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, expected.Id, log );

			Assert.IsType<OkObjectResult>( actual );
			Assert.Equal(
				expected,
				( actual as OkObjectResult ).Value
			);
		}

		[Fact]
		public async Task NoResults() {
			Mock
				.Arrange( () => this.EventRepository.Select( 404, default ) )
				.Returns( Task.FromResult<Event>( null ) );

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, 404, log );

			Assert.IsType<NotFoundResult>( actual );
		}

		[Fact]
		public async Task Error() {
			Mock
				.Arrange( () => this.EventRepository.Select( 500, default ) )
				.Throws<Exception>();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, 500, log );

			Assert.IsType<InternalServerErrorResult>( actual );
		}

		[Fact]
		public async Task LogsIntroMessage() {
			Mock
				.Arrange( () => this.EventRepository.Select( 404, default ) )
				.Returns( Task.FromResult<Event>( null ) );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 404, log );

			LogData actual = log.Data.First();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvent processed a request.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessage() {
			Mock
				.Arrange( () => this.EventRepository.Select( 404, default ) )
				.Returns( Task.FromResult<Event>( null ) );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 404, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvent finished a request.", actual.Message );
		}

		[Fact]
		public async Task LogsErrorMessage() {
			Exception ex = new Exception();

			Mock
				.Arrange( () => this.EventRepository.Select( 500, default ) )
				.Throws( ex );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 500, log );

			Assert.Equal( 3, log.Data.Count() );

			LogData actual = log.Data.Skip( 1 ).First();
			Assert.Equal( ex, actual.Exception );
			Assert.Equal( LogLevel.Error, actual.LogLevel );
			Assert.Equal( "GetEvent threw an exception.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessageOnError() {
			Exception ex = new Exception();

			Mock
				.Arrange( () => this.EventRepository.Select( 500, default ) )
				.Throws( ex );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 500, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvent finished a request.", actual.Message );
		}
	}
}
