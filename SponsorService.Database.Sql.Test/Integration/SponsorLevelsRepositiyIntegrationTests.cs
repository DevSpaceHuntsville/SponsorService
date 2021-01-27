using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Newtonsoft.Json;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Integration {
	[Collection( "SqlDatabase" )]
	public class SponsorLevelsRepositoryIntegrationTests : TestBase<SponsorLevelsRepository> {
		public SponsorLevelsRepositoryIntegrationTests( SqlDatabaseFixture fixture )
			: base( fixture ) { }

		#region Task<SponsorLevel> Select( int key, CancellationToken cancelToken = default )
		[Fact]
		public async Task SelectOne() {
			Assert.Equal(
				TestData.SponsorLevel01,
				await Repository.Select( TestData.SponsorLevel01.Id )
			);
		}

		[Fact]
		public async Task SelectOne_NotFound() {
			Assert.Null( await Repository.Select( -1 ) );
		}
		#endregion

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
