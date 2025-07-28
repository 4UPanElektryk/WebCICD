using CICD.Common.Node;
using CICD.Server.GlobalKVS;
using CICD.Server.NodeSubsystem;
using NetBase.Communication;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace CICD.Server
{
	class Program
	{

		public static void Main(string[] args)
		{
			NetBase.Server server = new NetBase.Server();
			server.HandeRequest = Router;
			server.Start("http://+:8080/");
			Console.WriteLine("Press any key to exit...");
			KeyValueStore.Initialize();
			Console.ReadKey();
		}
		public static HttpResponse Router(HttpRequest request)
		{
			Console.WriteLine(request.Url);
			Console.WriteLine(request.Method.ToString());

			if (request.Url.StartsWith("api/"))
			{
				return Api(request);
			}

			HttpResponse response = new HttpResponse(StatusCode.OK, "Hello World!", null, Encoding.UTF8);
			return response;
		}
		public static HttpResponse Api(HttpRequest request)
		{
			string path = request.Url.Substring(4); // Remove "api/" prefix
			Console.WriteLine($"API Path: {path}");
			
			if (request.Method == HttpMethod.POST && path == "nodes/subscribe")
			{
				Console.WriteLine(request.body);
				NodeInformation info = JsonConvert.DeserializeObject<NodeInformation>(request.body);
				return new HttpResponse(StatusCode.OK, NodeManager.RegisterNode(info), null, Encoding.UTF8);
			}
			if (request.Method == HttpMethod.POST && path.StartsWith("kvs/"))
			{
				string key = path.Substring(4); // Remove "kvs/" prefix
				Console.WriteLine($"Setting key: {key} with value: {request.body}");
				KeyValueStore.Set(key, request.body);
				return new HttpResponse(StatusCode.OK, "Key Set", null, Encoding.UTF8);
			}
			if (request.Method == HttpMethod.GET && path.StartsWith("kvs/"))
			{
				string key = path.Substring(4); // Remove "kvs/" prefix
				string value = KeyValueStore.Get(key);
				if (value != null)
				{
					return new HttpResponse(StatusCode.OK, value, null, Encoding.UTF8);
				}
				return new HttpResponse(StatusCode.Not_Found, "Key not found", null, Encoding.UTF8);
			}
			if (request.Method == HttpMethod.DELETE && path.StartsWith("kvs/"))
			{
				string key = path.Substring(4);
				bool success = KeyValueStore.Remove(key);
				if (success)
				{
					return new HttpResponse(StatusCode.OK, "Key Deleted", null, Encoding.UTF8);
				}
				return new HttpResponse(StatusCode.Not_Found, "Key not found", null, Encoding.UTF8);
			}


			return new HttpResponse(StatusCode.OK, "API Endpoint Hit", null, Encoding.UTF8);
		}
	}
}
