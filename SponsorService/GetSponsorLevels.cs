using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using DevSpaceHuntsville.SponsorService.Database;
using System.Web.Http;

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
