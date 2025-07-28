using System;
using NetBase.Communication;

namespace CICD.Server.API
{
	public class ApiEntry
	{
		public string Path { get; set; }
		public HttpMethod Method { get; set; }
		public EntryMatchType MatchType { get; set; } = EntryMatchType.Exact;
		public Func<ApiEntryArgs, HttpResponse> Handler { get; set; }
		
		public ApiEntry(string path, HttpMethod method, EntryMatchType matchType, Func<ApiEntryArgs, HttpResponse> handler)
		{
			Path = path;
			Method = method;
			MatchType = matchType;
			Handler = handler;
		}
	}
}
