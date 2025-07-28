using NetBase.Communication;
using System;

namespace CICD.Server.API
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public class EntryAttribute : Attribute
	{
		public HttpMethod Method;
		public string Path;
		public EntryMatchType MatchType;
		public EntryAttribute(HttpMethod method, string path, EntryMatchType matchType = EntryMatchType.Exact)
		{
			Method = method;
			Path = path;
			MatchType = matchType;
		}
	}
}
