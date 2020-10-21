using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DevSpaceHuntsville.SponsorService.Database.Sql;
using System.Collections.Generic;
using DevSpace.Common.Entities;

namespace SponsorService {
	public static class GetEvents {
		[FunctionName( "GetEvents" )]
		public static async Task<IActionResult> Run(
			[HttpTrigger( AuthorizationLevel.Anonymous, "get", Route = null )] HttpRequest request,
			ILogger log
		) {
			log.LogInformation( "GetEvents processed a request." );

			SqlDatabase database = new SqlDatabase( Environment.GetEnvironmentVariable( "SQL" ) );
			IEnumerable<Event> events = await database.EventsRepository.Get();

			return new OkObjectResult( JsonConvert.SerializeObject( events ) );
		}
	}
}
