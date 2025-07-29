using CICD.Common.Node;
using CICD.Supervisor.Connection;
using CICD.Supervisor.RequestedTasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace CICD.Supervisor
{
	class Program
	{
		public static NodeInformation NodeInfo = new NodeInformation();

		static void Main(string[] args)
		{
			SupervisorTaskRunner.Initialize();

			Console.WriteLine("Configuration Loading");
			if (!File.Exists("config.json"))
			{
				Console.Error.WriteLine("Configuration file not found. Please create a config.json file.");
				throw new FileNotFoundException("Configuration file not found.");
			}
			JObject config = JObject.Parse(File.ReadAllText("config.json"));
			NodeInfo.Name = config["NodeName"].ToString();
			NodeInfo.ID = "To Be filled by Server";
			NodeInfo.IP = GetLocalIPAddress();
			NodeInfo.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
			ConnectionManager.Subscribe(config["ServerAddress"].ToString(), (int)config["ServerPort"]);
			Console.WriteLine(JsonConvert.SerializeObject(NodeInfo));
			Console.ReadKey(true);
		}
		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
	}
}
