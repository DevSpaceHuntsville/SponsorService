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
using System.Collections.Generic;
using SponsorService.Tests.Helpers;
using System.Web.Http;

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
				.Select( i => new Event( i, $"DevSpace {i}", DateTime.Today.AddDays( -i ), DateTime.Today.AddDays( i ) ) );

			Mock
				.Arrange( () => this.EventsRepository.Get( default ) )
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
				.Arrange( () => this.EventsRepository.Get( default ) )
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
				.Arrange( () => this.EventsRepository.Get( default ) )
				.Throws<Exception>();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, log );

			Assert.IsType<InternalServerErrorResult>( actual );
		}

		[Fact]
		public async Task LogsIntroMessage() {
			Mock
				.Arrange( () => this.EventsRepository.Get( default ) )
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
				.Arrange( () => this.EventsRepository.Get( default ) )
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
				.Arrange( () => this.EventsRepository.Get( default ) )
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
				.Arrange( () => this.EventsRepository.Get( default ) )
				.Throws( ex );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetEvents finished a request.", actual.Message );
		}
	}
}
