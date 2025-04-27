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
		private static ServerInformation serverInfo;
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
					Thread d = new Thread(Loop);
					d.IsBackground = true; // Set the thread as a background thread
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
				Console.WriteLine($"Exception occurred: {ex.Message}");
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
						Console.WriteLine("Check-in successful.");
						failedresponses = 0;
					}
					else
					{
						Console.WriteLine($"Check-in failed: {response.Result.StatusCode}");
						failedresponses++;
						if (failedresponses > 3)
						{
							Console.WriteLine("Too many failed responses. Disconnecting.");
							status = ConnectionStatus.Disconnected;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Exception occurred: {ex.Message}");
					status = ConnectionStatus.Failed;
				}
				Task.Delay(5000).Wait(); // Wait for 5 seconds before checking again
			}
		}
		private void OnCheckinSuccessfull()
		{

		}
	}
}
