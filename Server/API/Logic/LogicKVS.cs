using CICD.Server.GlobalKVS;
using NetBase.Communication;
using System.Text;

namespace CICD.Server.API.Logic
{
	public class LogicKVS
	{
		[Entry(HttpMethod.GET,"kvs/",EntryMatchType.Prefix)]
		public static HttpResponse Get(ApiEntryArgs args) 
		{
			string key = args.Path;
			string value = KeyValueStore.Get(key);
			if (value != null)
			{
				return new HttpResponse(StatusCode.OK, value, null, Encoding.UTF8);
			}
			return new HttpResponse(StatusCode.Not_Found, "Key not found", null, Encoding.UTF8);
		}

		[Entry(HttpMethod.POST, "kvs/", EntryMatchType.Prefix)]
		public static HttpResponse Set(ApiEntryArgs args)
		{
			string key = args.Path;
			if (string.IsNullOrEmpty(args.Request.Body))
			{
				return new HttpResponse(StatusCode.Bad_Request, "No value provided", null, Encoding.UTF8);
			}
			KeyValueStore.Set(key, args.Request.Body);
			return new HttpResponse(StatusCode.OK, "Key Set", null, Encoding.UTF8);
		}

		[Entry(HttpMethod.DELETE, "kvs/", EntryMatchType.Prefix)]
		public static HttpResponse Delete(ApiEntryArgs args)
		{
			string key = args.Path;
			bool success = KeyValueStore.Remove(key);
			if (success)
			{
				return new HttpResponse(StatusCode.OK, "Key Deleted", null, Encoding.UTF8);
			}
			return new HttpResponse(StatusCode.Not_Found, "Key not found", null, Encoding.UTF8);
		}
	}
}
