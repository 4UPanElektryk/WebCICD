using System;
using CICD.Common.Task;

namespace CICD.Supervisor.RequestedTasks
{
    public class TaskRunner
    {
        public string Name { get; set; }
        public TaskRunner(string taskName)
		{
			Name = taskName;
		}
		public void Run(ref TaskInfo task)
		{
			Console.WriteLine($"Running task: {task.Name}");
			task.Status = TaskStatus.Running;
			task.StartedAt = DateTime.UtcNow;
			try
			{
				Execute(task.Args);
			}
			catch (System.Threading.ThreadAbortException)
			{
				task.Status = TaskStatus.Cancelled;
				task.FinishedAt = DateTime.UtcNow;
				Console.WriteLine($"[@{task.Id}] Task {task.Name} was cancelled.");
				return;
			}
			catch (Exception ex)
			{
				task.Status = TaskStatus.Failed;
				task.FinishedAt = DateTime.UtcNow;
				Console.Error.WriteLine($"[@{task.Id}] Error executing task {task.Name}: {ex.Message}");
				Console.Error.WriteLine($"[@{task.Id}] Stack Trace: {ex.StackTrace}\n");
				return;
			}
			task.Status = TaskStatus.Completed;
			task.FinishedAt = DateTime.UtcNow;
			Console.WriteLine($"[@{task.Id}] Task {task.Name} completed successfully.");

		}
		public virtual void Execute(string[] args)
		{
			Console.WriteLine($"Executing task: {Name}");
		}
	}
}
