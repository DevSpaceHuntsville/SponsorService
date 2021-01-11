using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Newtonsoft.Json;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Unit {
	public class EventsRepositoryUnitTests : TestBase<EventsRepository> {
		#region Task<Event> Select( int key, CancellationToken token = default )
		[Fact]
		public async Task SelectOne() {
			Event expected = new Event(
				2020,
				$"DevSpace {2020}",
				DateTime.Today.AddDays( -1 ),
				DateTime.Today.AddDays( 1 )
			);

			Database.AddJsonResult( JsonConvert.SerializeObject( new[] { expected } ) );

			Assert.Equal( expected, await Repository.Select( expected.Id ) );
		}

		[Fact]
		public async Task SelectOne_NoneFound_NullResults() {
			Database.AddEmptyResult();
			Assert.Null( await Repository.Select( -1 ) );
		}

		[Fact]
		public async Task SelectOne_Exceptional() {
			Database.AddSqlException();
			Assert.IsType<System.Data.SqlClient.SqlException>(
				await Record.ExceptionAsync( () =>
					Repository.Select( -1 )
				)
			);
		}

		[Fact]
		public async Task SelectOne_Cancelled() =>
			await base.CancellationTest( ( token ) => Repository.Select( 1, token ) );
		#endregion

		#region Task<IEnumerable<Event>> Select( CancellationToken token = default )
		[Fact]
		public async Task SelectAll() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event(
					id,
					$"DevSpace {id}",
					DateTime.Today.AddDays( -id ),
					DateTime.Today.AddDays( id )
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

		#region Task<IEnumerable<Event>> Select( IEnumerable<int> keys, CancellationToken token = default )
		[Fact]
		public async Task SelectSome() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event(
					id,
					$"DevSpace {id}",
					DateTime.Today.AddDays( -id ),
					DateTime.Today.AddDays( id )
				) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

			Assert.Equal(
				expected,
				await Repository.Select(
					expected.Select( e => e.Id )
				)
			);
		}

		[Fact]
		public async Task SelectSome_CorrectFilter() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event(
					id,
					$"DevSpace {id}",
					DateTime.Today.AddDays( -id ),
					DateTime.Today.AddDays( id )
				) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );
			await Repository.Select( expected.Select( e => e.Id ) );

			Assert.Contains(
				string.Join( ", ", expected.Select( e => e.Id ) ),
				Database.Commands.First().CommandText
			);
		}

		[Fact]
		public async Task SelectSome_NoDuplicates() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event(
					id,
					$"DevSpace {id}",
					DateTime.Today.AddDays( -id ),
					DateTime.Today.AddDays( id )
				) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

			Assert.Equal(
				expected,
				await Repository.Select(
					expected
						.Select( e => e.Id )
						.Concat( expected.Select( e => e.Id ) )
				)
			);
		}

		[Fact]
		public async Task SelectSome_SomeNotFound() {
			IEnumerable<Event> expected = Enumerable.Range( 2018, 3 )
				.Select( id => new Event(
					id,
					$"DevSpace {id}",
					DateTime.Today.AddDays( -id ),
					DateTime.Today.AddDays( id )
				) );

			Database.AddJsonResult( JsonConvert.SerializeObject( expected ) );

			Assert.Equal(
				expected,
				await Repository.Select(
					expected
						.Select( e => e.Id )
						.Prepend( int.MinValue )
						.Append( int.MaxValue )
				)
			);
		}

		[Fact]
		public async Task SelectSome_EmptyKeys() {
			Assert.Empty( await Repository.Select( Enumerable.Empty<int>() ) );
		}

		[Fact]
		public async Task SelectSome_NullKeys() {
			Assert.Empty( await Repository.Select( null ) );
		}

		[Fact]
		public async Task SelectSome_NoneFound_NullResults() {
			Database.AddEmptyResult();
			Assert.Empty( await Repository.Select( Enumerable.Range( 1, 3 ) ) );
		}

		[Fact]
		public async Task SelectSome_NoneFound_EmptyResults() {
			Database.AddJsonResult( "[]" );
			Assert.Empty( await Repository.Select( Enumerable.Range( 1, 3 ) ) );
		}

		[Fact]
		public async Task SelectSome_Exceptional() {
			Database.AddSqlException();
			Assert.IsType<System.Data.SqlClient.SqlException>(
				await Record.ExceptionAsync( () =>
					Repository.Select( Enumerable.Range( 1, 3 ) )
				)
			);
		}

		[Fact]
		public async Task SelectSome_Cancelled() =>
			await base.CancellationTest( ( token ) => Repository.Select( new[] { 1 }, token ) );
		#endregion
	}
}
