using CICD.Server.API;
using CICD.Server.GlobalKVS;
using CICD.Server.NodeSubsystem;
using NetBase.Communication;
using System;
using System.Text;

namespace CICD.Server
{
	class Program
	{

		public static void Main(string[] args)
		{
			NetBase.Server server = new NetBase.Server();
			server.HandeRequest = Router;

			KeyValueStore.Initialize();
			NodeManager.Initialize();
			ApiResponseManager.Initialize();

			server.Start("http://+:8080/");
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
		public static HttpResponse Router(HttpRequest request)
		{
			Console.WriteLine(request.Url);
			Console.WriteLine(request.Method.ToString());

			if (request.Url.StartsWith("api/"))
			{
				return ApiResponseManager.RunRequest(request);
			}


			HttpResponse response = new HttpResponse(StatusCode.OK, "Hello World!", null, Encoding.UTF8);
			return response;
		}
	}
}
