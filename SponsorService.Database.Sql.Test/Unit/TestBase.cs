using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test.Unit {
	public class TestBase<T> where T : class {
		protected readonly T Repository;
		protected readonly MockSponsorServiceDatabase Database;

		public TestBase() {
			this.Database = new MockSponsorServiceDatabase();
			this.Repository = typeof( T )
				.GetConstructors()
				.Single( ctor => ctor.GetParameters().Length == 1 )
				.Invoke( new object[] { Database } )
				as T;
		}

		public async Task CancellationTest( Func<CancellationToken,Task> f ) {
			CancellationTokenSource source = new CancellationTokenSource();
			source.Cancel();

			Database.AddSqlException();
			Assert.IsType<TaskCanceledException>(
				await Record.ExceptionAsync( () =>
					f( source.Token )
				)
			);
		}
	}
}
