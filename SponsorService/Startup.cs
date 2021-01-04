using System;
using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Database.Sql;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup( typeof( DevSpaceHuntsville.SponsorService.Startup ) )]

namespace DevSpaceHuntsville.SponsorService {
	public class Startup : FunctionsStartup {
		public override void Configure( IFunctionsHostBuilder builder ) {
			builder.Services.AddSingleton<ISponsorServiceDatabase>( ( s ) => {
				return new SqlSponsorServiceDatabase( Environment.GetEnvironmentVariable( "SQL" ) );
			} );

			//builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
		}
	}
}