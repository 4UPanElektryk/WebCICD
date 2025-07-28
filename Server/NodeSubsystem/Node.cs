using CICD.Common.Node;
using System;

namespace CICD.Server.NodeSubsystem
{
	public class Node
	{
		public string Id { get; set; } // Server Given id
		public string Name { get; set; } // Name given in the configuration
		public string Version { get; set; }
		public string IP { get; set; }
		public NodeStatus Status { get; set; }
		public DateTime LastCheckin { get; set; }
		public bool IsAvailable()
		{
			if (this.LastCheckin < DateTime.UtcNow.AddMinutes(-5))
			{
				this.Status = NodeStatus.Unreachable;
			}
			if (this.Status == NodeStatus.Healthy)
			{
				return true;
			}
			return false;
		}
	}
}
