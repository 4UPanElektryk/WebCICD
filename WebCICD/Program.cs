using NetBase.Communication;
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
			server.Start(IPAddress.Loopback, 8080);
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
		public static HttpResponse Router(HttpRequest request)
		{
			Console.WriteLine(request.Url);
			Console.WriteLine(request.body);
			HttpResponse response = new HttpResponse(StatusCode.OK, "Hello World!", null, Encoding.UTF8);
			return response;
		}
	}
}
