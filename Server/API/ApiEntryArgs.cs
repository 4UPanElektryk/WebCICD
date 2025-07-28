using NetBase.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.Server.API
{
	public class ApiEntryArgs
	{
		public HttpRequest Request { get; set; }
		/// <summary>
		/// Path of the request, excluding the "api/" prefix and optional path in case MatchType is EntryMatchType.Prefix
		/// </summary>
		public string Path { get; set; }
	}
}
