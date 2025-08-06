using NetBase.Communication;
using System.Text;
using Newtonsoft.Json;
using CICD.Common.Task;
using CICD.Server.TaskSubystem;

namespace CICD.Server.API.Logic
{
	public class LogicTasks
	{
		[Entry(HttpMethod.POST, "tasks/update", EntryMatchType.Exact)]
		public static HttpResponse UpadteTask(ApiEntryArgs args)
		{
			TaskInfo task = JsonConvert.DeserializeObject<TaskInfo>(args.Request.Body);
			if (task.Name == null)
			{
				return Respond.RequestError("Invalid task data provided.");
			}
			TaskManager.UpdateTask(task);
			// Here you would typically update the task in your database or in-memory store.
			// For this example, we will just return the updated task as a response.

			return Respond.OK;
		}

		[Entry(HttpMethod.GET, "tasks",EntryMatchType.Prefix)]
		public static HttpResponse Get(ApiEntryArgs args)
		{
			string node = args.Path;
			if (string.IsNullOrEmpty(node))
			{
				return Respond.RequestError("Node name is required.");
			}
			
			TaskInfo[] tasks = TaskManager.GetNotStartedTasksForNode(node);

			return Respond.Json(tasks);
		}
	}
}
