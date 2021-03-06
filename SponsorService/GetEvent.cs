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
	public class GetEvent {
		private readonly IConfiguration Configuration;
		private readonly ISponsorServiceDatabase Database;

		public GetEvent( IConfiguration config, ISponsorServiceDatabase database ) {
			this.Database = database;
			this.Configuration = config;
		}

		[FunctionName( "GetEvent" )]
		public async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = "v1/events/{id}" )] HttpRequest req,
			int id,
			ILogger log
		) {
			log.LogInformation( "GetEvent processed a request." );

			try {
				Event result = await this.Database.EventsRepository.Select( id );
				return null == result
					? (IActionResult)new NotFoundResult()
					: (IActionResult)new OkObjectResult( result );
			} catch( Exception ex ) {
				log.LogError( ex, "GetEvent threw an exception." );
				return new InternalServerErrorResult();
			} finally {
				log.LogInformation( "GetEvent finished a request." );
			}
		}
	}
}
