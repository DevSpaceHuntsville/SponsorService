﻿using System.Data;
using System.Data.Common;

namespace DevSpaceHuntsville.SponsorService.Database {
	public interface IDatabase {
		DbConnection GetConnection();
		DbParameter CreateParameter( string parameterName, DbType dbType, object value );
		DbParameter CreateParameter( string parameterName, DbType dbType, int size, object value );

		IEventsRepository EventsRepository { get; }
	}
}
