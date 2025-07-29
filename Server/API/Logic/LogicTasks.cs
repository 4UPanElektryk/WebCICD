using NetBase.Communication;
using System;
using System.Text;
using Newtonsoft.Json;
using CICD.Common.Task;

namespace CICD.Server.API.Logic
{
	public class LogicTasks
	{
		[Entry(HttpMethod.GET, "tasks",EntryMatchType.Prefix)]
		public static HttpResponse Get(ApiEntryArgs args)
		{
			string node = args.Path;
			if (string.IsNullOrEmpty(node))
			{
				return new HttpResponse(
					StatusCode.Bad_Request,
					"Node name is required.",
					null,
					Encoding.UTF8,
					ContentType.text_plain
				);
			}
			TaskInfo[] tasks = new TaskInfo[] {
				new TaskInfo(0,"Test","Worker-0")
			};

			return new HttpResponse(
				StatusCode.OK,
				JsonConvert.SerializeObject(tasks),
				null,
				Encoding.UTF8,
				ContentType.application_json
			);
		}
	}
}
