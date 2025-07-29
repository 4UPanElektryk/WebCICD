using System;

namespace CICD.Common.Task
{
	public struct TaskInfo
	{
		public ulong Id;
		public string Name;
		public string[] Args;
		public string NodeId;
		public string Description;
		public bool RunAsync; // Default: false
		public DateTime CreatedAt;
		public DateTime? StartedAt;
		public DateTime? FinishedAt;
		public TaskStatus Status;

		public TaskInfo(ulong id, string name, string nodeId, bool runAsync = false, string description = "", string[] args = null)
		{
			Id = id;
			Name = name;
			Args = args ?? new string[] { };
			NodeId = nodeId;
			RunAsync = runAsync;
			Description = description;
			CreatedAt = DateTime.UtcNow;
			StartedAt = null;
			FinishedAt = null;
			Status = TaskStatus.NotStarted;
		}
	}
}
