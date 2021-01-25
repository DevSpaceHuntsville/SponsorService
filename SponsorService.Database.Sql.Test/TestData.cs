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

		#region SponsorLevels
		internal static readonly SponsorLevel SponsorLevel01 = new SponsorLevel(  1,     1, "Premere",          10000,  true,   true,   true,   15, 25, 0,  true,   true,   true );
		internal static readonly SponsorLevel SponsorLevel02 = new SponsorLevel(  2,	 2,	"Diamond",			5000,	true,	true,	true,	10,	15,	0,	true,	true,	false );
		internal static readonly SponsorLevel SponsorLevel03 = new SponsorLevel(  3,	 3,	"Meal",				5000,	true,	true,	true,	10,	15,	0,	true,	true,	false );
		internal static readonly SponsorLevel SponsorLevel04 = new SponsorLevel(  4,	 4,	"USB",				3000,	true,	true,	true,	10,	10,	0,	true,	false,	false );
		internal static readonly SponsorLevel SponsorLevel05 = new SponsorLevel(  5,	 5,	"Social",			2500,	true,	true,	true,	8,	10,	0,	true,	false,	false );
		internal static readonly SponsorLevel SponsorLevel06 = new SponsorLevel(  6,	 7,	"Snack",			2500,	true,	false,	false,	8,	10,	0,	true,	false,	false );
		internal static readonly SponsorLevel SponsorLevel07 = new SponsorLevel(  7,	 6,	"Gold",				2000,	true,	false,	true,	8,	10,	30,	true,	false,	false );
		internal static readonly SponsorLevel SponsorLevel08 = new SponsorLevel(  8,	 8,	"Supplies",			1000,	true,	false,	false,	5,	5,	0,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel09 = new SponsorLevel(  9,	 9,	"Silver",			1000,	true,	false,	false,	5,	5,	0,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel10 = new SponsorLevel( 10,	10,	"Bronze",			500,	true,	false,	false,	3,	0,	0,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel11 = new SponsorLevel( 11,	11,	"In-Kind",			0,		true,	false,	false,	0,	0,	0,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel12 = new SponsorLevel( 12,	12,	"Amazing Sponsors",	500,	true,	false,	true,	0,	0,	60,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel13 = new SponsorLevel( 13,	13,	"Special Sponsors",	250,	true,	false,	false,	0,	0,	30,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel14 = new SponsorLevel( 14,	14,	"Image Sponsors",	100,	true,	false,	false,	0,	0,	15,	false,	false,	false );
		internal static readonly SponsorLevel SponsorLevel15 = new SponsorLevel( 15,    15, "Link Sponsors",    50,     true,   false,  false,  0,  0,  0,  false,  false,  false );
		internal static readonly SponsorLevel SponsorLevel16 = new SponsorLevel( 16,	16,	"Sponsors",			1,		false,	false,	false,	0,	0,	0,	false,	false,	false );

		internal static readonly IEnumerable<SponsorLevel> AllSponsorLevels = new[] {
			SponsorLevel01,
			SponsorLevel02,
			SponsorLevel03,
			SponsorLevel04,
			SponsorLevel05,
			SponsorLevel07,
			SponsorLevel06,
			SponsorLevel08,
			SponsorLevel09,
			SponsorLevel10,
			SponsorLevel11,
			SponsorLevel12,
			SponsorLevel13,
			SponsorLevel14,
			SponsorLevel15,
			SponsorLevel16
		};
		#endregion
	}
}
