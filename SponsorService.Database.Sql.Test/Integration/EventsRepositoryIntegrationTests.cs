using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Integration {
	[Collection( "SqlDatabase" )]
	public class EventsRepositoryIntegrationTests : TestBase<EventsRepository> {
		public EventsRepositoryIntegrationTests( SqlDatabaseFixture fixture )
			: base( fixture ) { }

		#region Task<Event> Select( int key, CancellationToken token = default )
		[Fact]
		public async Task SelectOne() {
			Event actual = await Repository.Select( TestData.Event2015.Id );
			Assert.Equal( TestData.Event2015, actual );
		}

		[Fact]
		public async Task SelectOne_NotFound() {
			Assert.Null( await Repository.Select( -1 ) );
		}
		#endregion

		#region Task<IEnumerable<Event>> Select( CancellationToken cancelToken = default )
		[Fact]
		public async Task SelectAll() {
			IEnumerable<Event> actual = await Repository.Select();
			Assert.Equal( TestData.AllEvents, actual );
		}
		#endregion

		#region Task<IEnumerable<Event>> Select( IEnumerable<int> keys, CancellationToken token = default )
		[Fact]
		public async Task SelectSome() {
			IEnumerable<Event> expected = new[] {
				TestData.Event2017,
				TestData.Event2018,
				TestData.Event2019
			};

			IEnumerable<Event> actual = await Repository.Select( expected.Select( e => e.Id ) );
			Assert.Equal( expected, actual );
		}

		[Fact]
		public async Task SelectSome_NoDuplicates() {
			IEnumerable<Event> expected = new[] {
				TestData.Event2017,
				TestData.Event2018,
				TestData.Event2019
			};

			IEnumerable<Event> actual =
				await Repository.Select(
					expected
						.Select( e => e.Id )
						.Concat( expected.Select( e => e.Id ) )
				);

			Assert.Equal( expected, actual );
		}

		[Fact]
		public async Task SelectSome_SomeNotFound() {
			IEnumerable<Event> expected = new[] {
				TestData.Event2017,
				TestData.Event2018,
				TestData.Event2019
			};

			IEnumerable<Event> actual =
				await Repository.Select(
					expected
						.Select( e => e.Id )
						.Prepend( 0 )
						.Append( int.MaxValue )
				);

			Assert.Equal( expected, actual );
		}

		[Fact]
		public async Task SelectSome_NoneFound() {
			IEnumerable<Event> actual =
				await Repository.Select(
					Enumerable.Range( 1, 5 )
				);

			Assert.Empty( actual );
		}
		#endregion
	}
}
