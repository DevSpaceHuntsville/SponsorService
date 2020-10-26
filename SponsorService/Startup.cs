using System;
using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Database.Sql;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup( typeof( SponsorService.Startup ) )]

namespace SponsorService {
	public class Startup : FunctionsStartup {
		public override void Configure( IFunctionsHostBuilder builder ) {
			builder.Services.AddSingleton<IDatabase>( ( s ) => {
				return new SqlDatabase( Environment.GetEnvironmentVariable( "SQL" ) );
			} );

			//builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
		}
	}
}