using NetBase.Communication;
using System.Text;

namespace CICD.Server
{
	public class Respond
	{
		public static HttpResponse OK = new HttpResponse(
			StatusCode.OK,
			"OK",
			null,
			Encoding.UTF8,
			ContentType.text_plain
		);
		public static HttpResponse Json(object data)
		{
			string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
			return new HttpResponse(
				StatusCode.OK,
				json,
				null,
				Encoding.UTF8,
				ContentType.application_json
			);
		}
		public static HttpResponse RequestError(string message, StatusCode code = StatusCode.Bad_Request)
		{
			return new HttpResponse(
				code,
				message,
				null,
				Encoding.UTF8,
				ContentType.text_plain
			);
		}
		public static HttpResponse Text(string message, StatusCode code = StatusCode.OK)
		{
			return new HttpResponse(
				code,
				message,
				null,
				Encoding.UTF8,
				ContentType.text_plain
			);
		}
	}
}
