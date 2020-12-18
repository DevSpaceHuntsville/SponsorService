using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using DevSpaceHuntsville.SponsorService.Database;
using DevSpaceHuntsville.SponsorService.Database.Sql;
using Telerik.JustMock;

namespace SponsorService.Database.Sql.Test.Unit {
	public class TestBase<T> where T : class {
		protected readonly T Repository;
		protected readonly IDatabase Database;

		public TestBase() {
			this.Database = Mock.Create<IDatabase>();
			this.Repository = Mock.Create<T>();

			Mock
				.Arrange( () => this.Database.EventsRepository )
				.Returns( this.Repository );
		}
	}
}
