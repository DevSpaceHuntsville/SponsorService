using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DevSpaceHuntsville.SponsorService.Database.Sql {
	internal static class DbDataReaderExtensions {
		internal async static Task<IEnumerable<T>> ReadJson<T>( this DbDataReader reader ) {
			StringBuilder sb = new StringBuilder();
			while( await reader.ReadAsync() ) sb.Append( reader.GetString( 0 ) );
			return JsonConvert.DeserializeObject<IEnumerable<T>>( sb.ToString() );
		}
	}
}
