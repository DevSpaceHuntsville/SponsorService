using System;
using System.Threading.Tasks;
using System.Web.Http;
using DevSpace.Common.Entities;
using DevSpaceHuntsville.SponsorService.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevSpaceHuntsville.SponsorService {
	public class GetSponsorLevel {
		private readonly IConfiguration Configuration;
		private readonly ISponsorServiceDatabase Database;

		public GetSponsorLevel( IConfiguration config, ISponsorServiceDatabase database ) {
			this.Database = database;
			this.Configuration = config;
		}

		[FunctionName( "GetSponsorLevel" )]
		public async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = "v1/sponsorlevels/{id:int}" )]
			HttpRequest req,
			int id,
			ILogger log
		) {
			log.LogInformation( "GetSponsorLevel processed a request." );

			try {
				SponsorLevel result = await this.Database.SponsorLevelsRepository.Select( id );
				return null == result
					? (IActionResult)new NotFoundResult()
					: (IActionResult)new OkObjectResult( result );
			} catch( Exception ex ) {
				log.LogError( ex, "GetSponsorLevel threw an exception." );
				return new InternalServerErrorResult();
			} finally {
				log.LogInformation( "GetSponsorLevel finished a request." );
			}
		}
	}
}
