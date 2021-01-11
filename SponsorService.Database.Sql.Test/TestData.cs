using System;
using System.Collections.Generic;
using DevSpace.Common.Entities;

namespace DevSpaceHuntsville.SponsorService.Database.Sql.Test {
	internal static class TestData {
		#region Events
		internal static readonly Event Event2015 = new Event( 2015, "DevSpace 2015", DateTime.Parse( "2015-10-15 00:00:00" ), DateTime.Parse( "2015-10-16 00:00:00" ) );
		internal static readonly Event Event2016 = new Event( 2016, "DevSpace 2016", DateTime.Parse( "2016-10-14 00:00:00" ), DateTime.Parse( "2016-10-15 00:00:00" ) );
		internal static readonly Event Event2017 = new Event( 2017, "DevSpace 2017", DateTime.Parse( "2017-10-13 00:00:00" ), DateTime.Parse( "2017-10-14 00:00:00" ) );
		internal static readonly Event Event2018 = new Event( 2018, "DevSpace 2018", DateTime.Parse( "2018-10-12 00:00:00" ), DateTime.Parse( "2018-10-13 00:00:00" ) );
		internal static readonly Event Event2019 = new Event( 2019, "DevSpace 2019", DateTime.Parse( "2019-10-11 00:00:00" ), DateTime.Parse( "2019-10-12 00:00:00" ) );
		internal static readonly Event Event2020 = new Event( 2020, "DevSpace 2020", DateTime.Parse( "2020-09-11 00:00:00" ), DateTime.Parse( "2020-09-11 00:00:00" ) );

		internal static readonly IEnumerable<Event> AllEvents = new[] {
			Event2015,
			Event2016,
			Event2017,
			Event2018,
			Event2019,
			Event2020
		};
		#endregion
	}
}
