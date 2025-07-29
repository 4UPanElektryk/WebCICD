using CICD.Common.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CICD.Supervisor.RequestedTasks
{
    public class SupervisorTaskRunner
    {

		private static List<TaskInfo> acquired;
		private static List<TaskInfo> released;
		private static List<TaskRunner> Runners;
		private static List<Thread> threads;

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
				if (task.RunAsync)
				{
					Thread t = new Thread(() => RunSingleTask(task));
					t.Start();
					threads.Add(t);
				}
				else
				{
					RunSingleTask(task);
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
			runner.Run(ref task);
			if (task.Status != TaskStatus.Running)
			{
				released.Add(task);
			}
		}
		public static void AddTasks(TaskInfo[] tasks)
		{
			acquired.AddRange(tasks);
			if (acquired.Count == 0)
			{
				Console.WriteLine("No tasks to run.");
				return;
			}
			Console.WriteLine($"Running {acquired.Count} tasks...");
			foreach (var task in acquired)
			{
				
				// Here you would call the actual task execution logic
				// For example, if TaskInfo has an Execute method:
				// task.Execute();
			}
		}
	}
}
