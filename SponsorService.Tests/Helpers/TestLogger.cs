using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DevSpaceHuntsville.SponsorService.Tests.Helpers {
	internal class LogData {
		public LogLevel LogLevel;
		public EventId EventId;
		public string Message;
		public object State;
		public Exception Exception;
	}

	internal class TestLogger : ILogger {
		private readonly List<LogData> data = new List<LogData>();
		public IEnumerable<LogData> Data => data;

		public IDisposable BeginScope<TState>( TState state ) =>
			throw new NotImplementedException();
		public bool IsEnabled( LogLevel logLevel ) =>
			true;
		public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter ) {
			data.Add( new LogData {
				LogLevel = logLevel,
				EventId = eventId,
				Message = formatter( state, exception ),
				State = state,
				Exception = exception
			} );
		}
	}
}
