using CICD.Common.Task;
using System.Collections.Generic;
using System.Linq;

namespace CICD.Server.TaskSubystem
{
	public class TaskManager
	{
		private static List<TaskInfo> tasks;
		public static void Initialize()
		{
			tasks = new List<TaskInfo>();
		}
		public static void AddTask(TaskInfo task)
		{
			tasks.Add(task);
		}
		public static void UpdateTask(TaskInfo task)
		{
			var existingTask = tasks.FirstOrDefault(t => t.Id == task.Id);
			if (existingTask.Name != null)
			{
				existingTask.Status = task.Status;
			}
			else
			{
				tasks.Add(task);
			}
		}
		public static TaskInfo GetTask(ulong id)
		{
			return tasks.FirstOrDefault(t => t.Id == id);
		}
		public static TaskInfo[] GetNotStartedTasksForNode(string nodeId)
		{
			return tasks.Where(t => t.NodeId == nodeId && t.Status == TaskStatus.NotStarted).ToArray();
		}
	}
}
