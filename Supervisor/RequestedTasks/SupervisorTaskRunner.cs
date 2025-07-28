using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.Supervisor.RequestedTasks
{
    public class SupervisorTaskRunner
    {
		private List<SupervisorTask> acquired;
		public SupervisorTaskRunner()
		{
			acquired = new List<SupervisorTask>();
			// Constructor logic here
		}
		public void RunCheckinTask()
		{
			// Logic to run the check-in task
			Console.WriteLine("Running check-in task...");
			// Add your task execution code here
		}
	}
}
