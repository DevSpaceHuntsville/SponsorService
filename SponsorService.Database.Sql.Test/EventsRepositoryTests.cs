using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevSpace.Common.Entities;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test {
	public class EventsRepositoryTests {
		internal readonly SqlDatabase Database;
		internal readonly EventsRepository EventsRepo;

		public EventsRepositoryTests() {
			ConstructorInfo[] ctors = typeof( Event ).GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance );
			SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder {
				InitialCatalog = "DevSpace",
				DataSource = "localhost",
				IntegratedSecurity = true
			};

			Database = new SqlDatabase( scsb.ToString() );
			EventsRepo = new EventsRepository( Database );
		}

		#region Task<IEnumerable<Event>> Get( CancellationToken cancelToken = default )
		[Fact]
		public async Task Get() {
			IEnumerable<Event> expected = Enumerable.Range( 2015, 6 )
				.Select( i => new Event( i, $"DevSpace {i}" ) );

			IEnumerable<Event> actual = await EventsRepo.Get();

			Assert.Equal( expected, actual );
		}
		#endregion
	}
}
