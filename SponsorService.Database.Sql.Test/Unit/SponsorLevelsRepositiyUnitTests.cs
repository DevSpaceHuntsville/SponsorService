using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Newtonsoft.Json;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Unit {
	public class SponsorLevelsRepositiyUnitTests : TestBase<SponsorLevelsRepository> {
		#region Task<SponsorLevel> Select( int key, CancellationToken token = default )
		[Fact]
		public async Task SelectOne() {
			SponsorLevel expected = new SponsorLevel(
				1,
				2,
				$"Sponsor Level 3",
				400,
				true,
				false,
				true,
				5,
				60,
				70,
				false,
				true,
				false
			);

			Database.AddJsonResult( JsonConvert.SerializeObject( new[] { expected } ) );

			Assert.Equal( expected, await Repository.Select( 1 ) );
		}

		[Fact]
		public async Task SelectOne_NoneFound_NullResults() {
			Database.AddEmptyResult();
			Assert.Null( await Repository.Select( 1 ) );
		}

		[Fact]
		public async Task SelectOne_NoneFound_EmptyResults() {
			Database.AddJsonResult( "[]" );
			Assert.Null( await Repository.Select( 1 ) );
		}

		[Fact]
		public async Task SelectOne_Exceptional() {
			Database.AddSqlException();
			Assert.IsType<System.Data.SqlClient.SqlException>(
				await Record.ExceptionAsync( () =>
					Repository.Select( 1 )
				)
			);
		}

		[Fact]
		public async Task SelectOne_Cancelled() =>
			await base.CancellationTest( ( token ) => Repository.Select( 1, token ) );
		#endregion

		#region Task<IEnumerable<SponsorLevel>> Select( CancellationToken token = default )
		[Fact]
		public async Task SelectAll() {
			IEnumerable<SponsorLevel> expected = Enumerable.Range( 1, 5 )
				.Select( id => new SponsorLevel(
					id,
					id,
					$"Sponsor Level {id}",
					id * 100,
					true,
					id > 1,
					id > 2,
					id * 2,
					id * 10,
					( id - 1 ) * 30,
					true,
					id > 1,
					id > 2
				) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

			Assert.Equal( expected, await Repository.Select() );
		}

		[Fact]
		public async Task SelectAll_NoneFound_NullResults() {
			Database.AddEmptyResult();
			Assert.Empty( await Repository.Select() );
		}

		[Fact]
		public async Task SelectAll_NoneFound_EmptyResults() {
			Database.AddJsonResult( "[]" );
			Assert.Empty( await Repository.Select() );
		}

		[Fact]
		public async Task SelectAll_Exceptional() {
			Database.AddSqlException();
			Assert.IsType<System.Data.SqlClient.SqlException>(
				await Record.ExceptionAsync( () =>
					Repository.Select()
				)
			);
		}

		[Fact]
		public async Task SelectAll_Cancelled() =>
			await base.CancellationTest( ( token ) => Repository.Select( token ) );
		#endregion
	}
}
