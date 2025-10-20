using CICD.Common.Task;
using CICD.Supervisor.RequestedTasks;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CICD.Supervisor.Connection
{
	public class ConnectionManager
	{
		public static ServerInformation serverInfo;
		public static ConnectionStatus status = ConnectionStatus.Disconnected;
		private static HttpClient client = new HttpClient();
		public static void Subscribe(string address, int port)
		{
			// Subscribe to the server
			Console.WriteLine($"Subscribing to server at {address}:{port}");
			serverInfo = new ServerInformation { Address = address, Port = port };
			Task<HttpResponseMessage> response = client.PostAsync($"{serverInfo.Uri()}/api/nodes/subscribe", new StringContent(JsonConvert.SerializeObject(Program.NodeInfo), Encoding.UTF8, "application/json"));
			try
			{
				if (response.Result.IsSuccessStatusCode)
				{
					Program.NodeInfo.ID = response.Result.Content.ReadAsStringAsync().Result;
					Console.WriteLine("Subscription successful.");
					status = ConnectionStatus.Connected;
					Thread d = new Thread(Loop)
					{
						IsBackground = true, // Set the thread as a background thread
						Name = "CheckinThread" // Optional: Set a name for the thread
					};
					d.Start();
					Console.WriteLine("Background thread started.");
				}
				else
				{
					Console.WriteLine($"Subscription failed: {response.Result.StatusCode}");
					status = ConnectionStatus.Failed;
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Exception occurred: {ex.Message}");
				status = ConnectionStatus.Failed;
			}
		}
		public static void Loop()
		{
			int failedresponses = 0;
			while (status == ConnectionStatus.Connected)
			{
				Task<HttpResponseMessage> response = client.PostAsync($"{serverInfo.Uri()}/api/nodes/checkin", new StringContent(JsonConvert.SerializeObject(Program.NodeInfo), Encoding.UTF8, "application/json"));
				try
				{
					if (response.Result.IsSuccessStatusCode)
					{
						Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
						OnCheckinSuccessfull(JsonConvert.DeserializeObject<bool>(response.Result.Content.ReadAsStringAsync().Result));

						Console.WriteLine("Check-in successful.");
						failedresponses = 0;
					}
					else
					{
						Console.WriteLine($"Check-in failed: {response.Result.StatusCode}");
						failedresponses++;
						if (failedresponses > 3)
						{
							Console.Error.WriteLine("Too many failed responses. Disconnecting.");
							status = ConnectionStatus.Disconnected;
						}
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine($"Exception occurred: {ex.Message}");
					Console.Error.WriteLine($"Source: {ex.Source}");
					Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
					status = ConnectionStatus.Failed;
				}
				Task.Delay(10000).Wait(); // Wait for 5 seconds before checking again
			}
		}
		private static void OnCheckinSuccessfull(bool TasksFound)
		{
			if (!TasksFound)
			{
				Console.WriteLine("No tasks found for this node.");
				return;
			}
			Task<HttpResponseMessage> response = client.GetAsync($"{serverInfo.Uri()}/api/tasks/{Program.NodeInfo.ID}");
			try
			{
				if (response.Result.IsSuccessStatusCode)
				{
					string content = response.Result.Content.ReadAsStringAsync().Result;
					TaskInfo[] tasks = JsonConvert.DeserializeObject<TaskInfo[]>(content);
					if (tasks.Length > 0)
					{
						Console.WriteLine($"Found {tasks.Length} tasks for this node.");
						SupervisorTaskRunner.AddTasks(tasks);
						SupervisorTaskRunner.RunAcquiredTasks();
					}
					else
					{
						Console.WriteLine("No tasks found for this node.");
					}
				}
				else
				{
					Console.WriteLine($"Failed to retrieve tasks: {response.Result.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Exception occurred while retrieving tasks: {ex.Message}");
			}
		}
	}
}
