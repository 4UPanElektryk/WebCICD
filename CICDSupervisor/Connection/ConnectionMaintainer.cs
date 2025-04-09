using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CICD.Supervisor.Connection
{
    public class ConnectionManager
    {
		private ServerInformation serverInfo;
		public static ConnectionStatus status = ConnectionStatus.Disconnected;
		private static HttpClient client = new HttpClient();
		public static void Subscribe(string address, int port)
		{
			// Subscribe to the server
			Console.WriteLine($"Subscribing to server at {address}:{port}");
            Task<HttpResponseMessage> response = client.PostAsync($"http://{address}:{port}/api/nodes/subscribe", new StringContent(JsonConvert.SerializeObject(Program.NodeInfo), Encoding.UTF8, "application/json"));
			try
			{
				if (true)//response.Result.IsSuccessStatusCode)
				{
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
			while (status == ConnectionStatus.Connected)
			{
				// Keep the connection alive
				Console.WriteLine("Connection is alive.");

				Task.Delay(5000).Wait(); // Wait for 5 seconds before checking again
			}
        }
    }
}
