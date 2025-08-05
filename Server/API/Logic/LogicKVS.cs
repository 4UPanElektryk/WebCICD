using CICD.Server.GlobalKVS;
using NetBase.Communication;

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
				return Respond.Text(value, StatusCode.OK);
			}
			return Respond.RequestError("Key not found", StatusCode.Not_Found);
		}

		[Entry(HttpMethod.POST, "kvs/", EntryMatchType.Prefix)]
		public static HttpResponse Set(ApiEntryArgs args)
		{
			string key = args.Path;
			if (string.IsNullOrEmpty(args.Request.Body))
			{
				return Respond.RequestError("No value provided for the key.", StatusCode.Bad_Request);
			}
			KeyValueStore.Set(key, args.Request.Body);

			return Respond.Text("Key Set", StatusCode.OK);
		}

		[Entry(HttpMethod.DELETE, "kvs/", EntryMatchType.Prefix)]
		public static HttpResponse Delete(ApiEntryArgs args)
		{
			string key = args.Path;
			bool success = KeyValueStore.Remove(key);
			if (success)
			{
				return Respond.Text("Key Deleted", StatusCode.OK);
			}
			return Respond.RequestError("Key not found", StatusCode.Not_Found);
		}
	}
}
