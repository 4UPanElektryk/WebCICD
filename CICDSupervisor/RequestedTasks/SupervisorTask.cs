using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.Supervisor.RequestedTasks
{
    public class SupervisorTask
    {
        public string TaskName { get; set; }
        public SupervisorTask(string taskName)
		{
			TaskName = taskName;
		}
		public virtual void Execute()
		{
			Console.WriteLine($"Executing task: {TaskName}");
			// Add your task execution logic here
		}
	}
}
