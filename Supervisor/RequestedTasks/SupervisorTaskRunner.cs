using CICD.Common.Task;
using CICD.Supervisor.Connection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace CICD.Supervisor.RequestedTasks
{
    public class SupervisorTaskRunner
    {

		private static List<TaskInfo> acquired;
		private static List<TaskInfo> released;
		private static List<TaskRunner> Runners;
		public static Dictionary<ulong, Thread> runningTasks;

		public static void Initialize()
		{
			acquired = new List<TaskInfo>();
			released = new List<TaskInfo>();
			Runners = new List<TaskRunner> {
				new TaskRunner("Test")
			};
			// Initialization logic here
		}
		public static void RunAcquiredTasks()
		{
			for (int i = 0; i < acquired.Count; i++)
			{
				TaskInfo task = acquired[i];
				if (task.Status != TaskStatus.NotStarted)
				{
					Console.WriteLine($"[@{task.Id}] Task {task.Name} is already started or completed, skipping.");
					continue;
				}
				Thread t = new Thread(() => RunSingleTask(task));
				runningTasks[task.Id] = t;
				t.Start();
				if (!task.RunAsync)
				{
					t.Join();
				}
			}

		}
		private static void RunSingleTask(TaskInfo task)
		{
			TaskRunner runner = Runners.FirstOrDefault(r => r.Name == task.Name);
			if (runner == null)
			{
				Console.WriteLine($"No runner found for task: {task.Name}");
				return;
			}
			Console.WriteLine($"[@{task.Id}] Running task: {task.Name}");
			Thread t = new Thread(() => runner.Run(ref task));
			t.Start();
			Thread.Sleep(20);
			UpdateTaskOnServer(task);
			while(task.Status == TaskStatus.Running)
			{
				// wait for task to finish
			}
			UpdateTaskOnServer(task);
			released.Add(task);
		}
		private static void UpdateTaskOnServer(TaskInfo task)
		{
			HttpClient client = new HttpClient()
			{
				BaseAddress = new Uri(ConnectionManager.serverInfo.Uri())
			};
			try
			{
				var response = client.PostAsync($"/api/tasks/update", new StringContent(JsonConvert.SerializeObject(task),Encoding.UTF8, "application/json")).Result;
				if (response.IsSuccessStatusCode)
				{
					Console.WriteLine($"[@{task.Id}] Task {task.Name} updated successfully on server.");
				}
				else
				{
					Console.WriteLine($"[@{task.Id}] Failed to update task {task.Name} on server: {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"[@{task.Id}] Error updating task {task.Name} on server: {ex.Message}");
			}
		}
		public static void AddTasks(TaskInfo[] tasks)
		{
			foreach (var task in tasks)
			{
				if (string.IsNullOrEmpty(task.Name))
				{
					Console.WriteLine("Invalid task provided, skipping.");
					continue;
				}
				if (acquired.Any(t => t.Id == task.Id))
				{
					Console.WriteLine($"Task with ID {task.Id} already exists, skipping.");
					continue;
				}
				acquired.Add(task);
			}
		}
	}
}
