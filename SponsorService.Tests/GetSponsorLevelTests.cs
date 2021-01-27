using System;
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
	public class GetSponsorLevelTests {
		private readonly MockSponsorServiceDatabase Database;
		private readonly GetSponsorLevel Controller;

		public GetSponsorLevelTests() {
			this.Database = new MockSponsorServiceDatabase();

			IConfiguration config = Mock.Create<IConfiguration>();

			this.Controller = new GetSponsorLevel( config, this.Database );
		}

		[Fact]
		public async Task Run() {
			SponsorLevel expected = new SponsorLevel(
				1,
				2,
				$"Sponsor Level 3",
				400,
				true,
				false,
				true,
				5,
				60,
				70,
				false,
				true,
				false
			);

			this.Database.AddJsonResult(
				JsonConvert.SerializeObject( new[] { expected } )
			);

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
			this.Database.AddEmptyResult();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, 404, log );

			Assert.IsType<NotFoundResult>( actual );
		}

		[Fact]
		public async Task Error() {
			this.Database.AddSqlException();

			TestLogger log = new TestLogger();
			IActionResult actual = await this.Controller.Run( null, 500, log );

			Assert.IsType<InternalServerErrorResult>( actual );
		}

		[Fact]
		public async Task LogsIntroMessage() {
			this.Database.AddEmptyResult();

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 404, log );

			LogData actual = log.Data.First();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevel processed a request.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessage() {
			this.Database.AddEmptyResult();

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 404, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevel finished a request.", actual.Message );
		}

		[Fact]
		public async Task LogsErrorMessage() {
			const string errorMessage = "LogsErrorMessage Message";
			this.Database.AddSqlException( errorMessage );

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 500, log );

			Assert.Equal( 3, log.Data.Count() );

			LogData actual = log.Data.Skip( 1 ).First();
			Assert.Equal( errorMessage, actual.Exception.Message );
			Assert.Equal( LogLevel.Error, actual.LogLevel );
			Assert.Equal( "GetSponsorLevel threw an exception.", actual.Message );
		}

		[Fact]
		public async Task LogsOutroMessageOnError() {
			this.Database.AddSqlException();

			TestLogger log = new TestLogger();
			await this.Controller.Run( null, 500, log );

			LogData actual = log.Data.Last();
			Assert.Equal( LogLevel.Information, actual.LogLevel );
			Assert.Equal( "GetSponsorLevel finished a request.", actual.Message );
		}
	}
}
