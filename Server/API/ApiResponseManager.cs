using NetBase.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CICD.Server.API
{
	public class ApiResponseManager
	{
		private static ApiEntry[] entries;

		public static void Initialize()
		{
			var methods = Assembly.GetExecutingAssembly().GetTypes()
				.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
				.Where(m => m.GetCustomAttributes(typeof(EntryAttribute), false).Length > 0);

			List<ApiEntry> entryList = new List<ApiEntry>();
			methods.ToList().ForEach(m =>
			{
				var attr = (EntryAttribute)m.GetCustomAttribute(typeof(EntryAttribute));
				if (attr != null)
				{
					Console.WriteLine($"Added Entry {attr.Path} Method {attr.Method}");
					entryList.Add(
						new ApiEntry(
							attr.Path, 
							attr.Method, 
							attr.MatchType, 
							(Func<ApiEntryArgs, HttpResponse>)Delegate.CreateDelegate(typeof(Func<ApiEntryArgs, HttpResponse>), m)
						)
					);
				}
			});

			entries = entryList.ToArray();
			Console.WriteLine("API Response Manager Initialized");
		}

		public static HttpResponse RunRequest(HttpRequest request)
		{
			string path = request.Url.Substring(4); // Remove "api/" prefix
			Console.WriteLine($"API Path: {path}");

			// Find matching entry
			ApiEntry entry = entries?.FirstOrDefault(e =>
				e.Method == request.Method &&
				((e.MatchType == EntryMatchType.Exact && e.Path == path) ||
				 (e.MatchType == EntryMatchType.Prefix && path.StartsWith(e.Path)))
			);

			if (entry == null)
			{
				return new HttpResponse(StatusCode.Not_Found, "API endpoint not found");
			}

			ApiEntryArgs args = new ApiEntryArgs()
			{
				Request = request,
				Path = path.Substring(entry.Path.Length),
			};
			return entry.Handler(args);
		}
	}
}
