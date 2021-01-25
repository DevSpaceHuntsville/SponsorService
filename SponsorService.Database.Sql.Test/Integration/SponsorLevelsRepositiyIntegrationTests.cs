using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Integration {
	[Collection( "SqlDatabase" )]
	public class SponsorLevelsRepositoryIntegrationTests : TestBase<SponsorLevelsRepository> {
		public SponsorLevelsRepositoryIntegrationTests( SqlDatabaseFixture fixture )
			: base( fixture ) { }

		#region Task<IEnumerable<SponsorLevel>> Select( CancellationToken cancelToken = default )
		[Fact]
		public async Task SelectAll() {
			IEnumerable<SponsorLevel> actual = await Repository.Select();
			Assert.True( TestData.AllSponsorLevels.SequenceEqual( actual ), $@"
Expected Json: {JsonConvert.SerializeObject( TestData.AllSponsorLevels, Formatting.Indented )}

Actual Json: {JsonConvert.SerializeObject( actual, Formatting.Indented )}" );
		}
		#endregion
	}
}
