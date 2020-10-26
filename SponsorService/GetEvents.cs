using System.Collections.Generic;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using DevSpaceHuntsville.SponsorService.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SponsorService {
	public class GetEvents {
		private readonly IConfiguration Configuration;
		private readonly IDatabase Database;

		public GetEvents( IConfiguration config, IDatabase database ) {
			this.Database = database;
			this.Configuration = config;
		}

		[FunctionName( "GetEvents" )]
		public async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = null )] HttpRequest request,
			ILogger log
		) {
			log.LogInformation( "GetEvents processed a request." );

			IEnumerable<Event> events = await Database.EventsRepository.Get();
			return new OkObjectResult( events );
		}
	}
}
