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
	public class GetSponsorLevels {
		private readonly IConfiguration Configuration;
		private readonly ISponsorServiceDatabase Database;

		public GetSponsorLevels( IConfiguration config, ISponsorServiceDatabase database ) {
			this.Database = database;
			this.Configuration = config;
		}

		[FunctionName( "GetSponsorLevels" )]
		public async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = "v1/sponsorlevels" )] HttpRequest req,
			ILogger log
		) {
			log.LogInformation( "GetSponsorLevels processed a request." );

			try {
				return new OkObjectResult( await this.Database.SponsorLevelsRepository.Select() );
			} catch( Exception ex ) {
				log.LogError( ex, "GetSponsorLevels threw an exception." );
				return new InternalServerErrorResult();
			} finally {
				log.LogInformation( "GetSponsorLevels finished a request." );
			}
		}
	}
}
