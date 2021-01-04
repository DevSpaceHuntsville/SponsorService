using System.Linq;

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
	}
}
