using System;
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
			Event expected =
				new Event(
					2015,
					"DevSpace 2015",
					DateTime.Parse( "2015-10-15 00:00:00" ),
					DateTime.Parse( "2015-10-16 00:00:00" )
				);

			Event actual = await Repository.Select( expected.Id );
			Assert.Equal( expected, actual );
		}

		[Fact]
		public async Task SelectOne_NotFound() {
			Assert.Null( await Repository.Select( -1 ) );
		}
		#endregion

		#region Task<IEnumerable<Event>> Select( CancellationToken cancelToken = default )
		[Fact]
		public async Task SelectAll() {
			IEnumerable<Event> expected = new[] {
				new Event( 2015, "DevSpace 2015", DateTime.Parse( "2015-10-15 00:00:00" ), DateTime.Parse( "2015-10-16 00:00:00" ) ),
				new Event( 2016, "DevSpace 2016", DateTime.Parse( "2015-10-14 00:00:00" ), DateTime.Parse( "2015-10-15 00:00:00" ) ),
				new Event( 2017, "DevSpace 2017", DateTime.Parse( "2015-10-13 00:00:00" ), DateTime.Parse( "2015-10-14 00:00:00" ) ),
				new Event( 2018, "DevSpace 2018", DateTime.Parse( "2015-10-12 00:00:00" ), DateTime.Parse( "2015-10-13 00:00:00" ) ),
				new Event( 2019, "DevSpace 2019", DateTime.Parse( "2015-10-11 00:00:00" ), DateTime.Parse( "2015-10-12 00:00:00" ) ),
				new Event( 2020, "DevSpace 2020", DateTime.Parse( "2020-09-11 00:00:00" ), DateTime.Parse( "2020-09-11 00:00:00" ) )
			};

			IEnumerable<Event> actual = await Repository.Select();

			Assert.Equal( expected, actual );
		}
		#endregion

		#region Task<IEnumerable<Event>> Select( IEnumerable<int> keys, CancellationToken token = default )
		[Fact]
		public async Task SelectSome() {
			IEnumerable<Event> expected = new[] {
				new Event( 2017, "DevSpace 2017", DateTime.Parse( "2015-10-13 00:00:00" ), DateTime.Parse( "2015-10-14 00:00:00" ) ),
				new Event( 2018, "DevSpace 2018", DateTime.Parse( "2015-10-12 00:00:00" ), DateTime.Parse( "2015-10-13 00:00:00" ) ),
				new Event( 2019, "DevSpace 2019", DateTime.Parse( "2015-10-11 00:00:00" ), DateTime.Parse( "2015-10-12 00:00:00" ) )
			};

			IEnumerable<Event> actual = await Repository.Select( expected.Select( e => e.Id ) );
			Assert.Equal( expected, actual );
		}

		[Fact]
		public async Task SelectSome_NoDuplicates() {
			IEnumerable<Event> expected = new[] {
				new Event( 2017, "DevSpace 2017", DateTime.Parse( "2015-10-13 00:00:00" ), DateTime.Parse( "2015-10-14 00:00:00" ) ),
				new Event( 2018, "DevSpace 2018", DateTime.Parse( "2015-10-12 00:00:00" ), DateTime.Parse( "2015-10-13 00:00:00" ) ),
				new Event( 2019, "DevSpace 2019", DateTime.Parse( "2015-10-11 00:00:00" ), DateTime.Parse( "2015-10-12 00:00:00" ) )
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
				new Event( 2017, "DevSpace 2017", DateTime.Parse( "2015-10-13 00:00:00" ), DateTime.Parse( "2015-10-14 00:00:00" ) ),
				new Event( 2018, "DevSpace 2018", DateTime.Parse( "2015-10-12 00:00:00" ), DateTime.Parse( "2015-10-13 00:00:00" ) ),
				new Event( 2019, "DevSpace 2019", DateTime.Parse( "2015-10-11 00:00:00" ), DateTime.Parse( "2015-10-12 00:00:00" ) )
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
