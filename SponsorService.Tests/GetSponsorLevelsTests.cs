using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DevSpace.Common.Entities;
using DevSpaceHuntsville.SponsorService.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telerik.JustMock;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Tests {
	public class GetSponsorLevelsTests {
		private readonly MockSponsorServiceDatabase Database;
		private readonly GetSponsorLevels Controller;

		public GetSponsorLevelsTests() {
			this.Database = new MockSponsorServiceDatabase();

			IConfiguration config = Mock.Create<IConfiguration>();

			this.Controller = new GetSponsorLevels( config, this.Database );
		}

		[Fact]
		public async Task Run() {
			IEnumerable<SponsorLevel> expected = Enumerable.Range( 1, 9 )
				.Select( i => new SponsorLevel(
					id: i,
					displayorder: i,
					name: $"Level {i}",
					cost: i * 100,
					displaylink: ( i & 0x1 ) == 0x1,
					displayinemails: ( i & 0x2 ) == 0x2,
					displayinsidebar: ( i & 0x3 ) == 0x3,
					tickets: i * 2,
					discount: i * 3,
					timeonscreen: i * 15,
					preconemail: ( i & 0x4 ) == 0x4,
					midconemail: ( i & 0x5 ) == 0x5,
					postconemail: ( i & 0x6 ) == 0x6
				) );

			this.Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

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
			IEnumerable<SponsorLevel> expected = Enumerable.Empty<SponsorLevel>();

			this.Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

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
			this.Database.AddSqlException();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, log );

			Assert.IsType<InternalServerErrorResult>( actual );
		}

		[Fact]
		public async Task LogsIntroMessage() {
			this.Database.AddJsonResult( "[]" );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.First();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevels processed a request.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessage() {
			this.Database.AddJsonResult( "[]" );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevels finished a request.", actual.Message );
		}

		[Fact]
		public async Task LogsErrorMessage() {
			const string errorMessage = "LogsErrorMessage Message";
			this.Database.AddSqlException( errorMessage );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			Assert.Equal( 3, log.Data.Count() );

			LogData actual = log.Data.Skip( 1 ).First();
			Assert.Equal( errorMessage, actual.Exception.Message );
			Assert.Equal( LogLevel.Error, actual.LogLevel );
			Assert.Equal( "GetSponsorLevels threw an exception.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessageOnError() {
			this.Database.AddSqlException();

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevels finished a request.", actual.Message );
		}
	}
}
