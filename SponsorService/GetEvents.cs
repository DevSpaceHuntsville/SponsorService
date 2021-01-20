using System;
using System.Threading.Tasks;
using System.Web.Http;
using DevSpaceHuntsville.SponsorService.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevSpaceHuntsville.SponsorService {
	public class GetEvents {
		private readonly IConfiguration Configuration;
		private readonly ISponsorServiceDatabase Database;

		public GetEvents( IConfiguration config, ISponsorServiceDatabase database ) {
			this.Database = database;
			this.Configuration = config;
		}

		[FunctionName( "GetEvents" )]
		public async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = "v1/events" )] HttpRequest request,
			ILogger log
		) {
			log.LogInformation( "GetEvents processed a request." );

			try {
				return new OkObjectResult( await this.Database.EventsRepository.Select() );
			} catch( Exception ex ) {
				log.LogError( ex, "GetEvents threw an exception." );
				return new InternalServerErrorResult();
			} finally {
				log.LogInformation( "GetEvents finished a request." );
			}
		}
	}
}
