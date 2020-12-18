using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using DevSpaceHuntsville.SponsorService.Database;

namespace SponsorService.Database.Sql.Test.Unit {
	internal class MockDatabase : IDatabase {
		public IEventsRepository EventsRepository => throw new NotImplementedException();

		public DbParameter CreateParameter( string parameterName, DbType dbType, object value ) => throw new NotImplementedException();
		public DbParameter CreateParameter( string parameterName, DbType dbType, int size, object value ) => throw new NotImplementedException();
		public DbConnection GetConnection() => throw new NotImplementedException();
	}
}
