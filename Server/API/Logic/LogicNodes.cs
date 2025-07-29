using CICD.Common.Node;
using CICD.Server.NodeSubsystem;
using CICD.Server.TaskSubystem;
using NetBase.Communication;
using Newtonsoft.Json;
using System.Text;

namespace CICD.Server.API.Logic
{
	public class LogicNodes
	{
		[Entry(HttpMethod.POST, "nodes/subscribe", EntryMatchType.Exact)]
		public static HttpResponse Subscribe(ApiEntryArgs args)
		{
			NodeInformation info = JsonConvert.DeserializeObject<NodeInformation>(args.Request.Body);
			return new HttpResponse(StatusCode.OK, NodeManager.RegisterNode(info), null, Encoding.UTF8);
		}

		[Entry(HttpMethod.POST, "nodes/checkin", EntryMatchType.Exact)]
		public static HttpResponse Checkin(ApiEntryArgs args)
		{
			NodeInformation info = JsonConvert.DeserializeObject<NodeInformation>(args.Request.Body);
			NodeManager.NodeCheckin(info);
			string response = JsonConvert.SerializeObject(TaskManager.GetTasksForNode(info.ID));

			return new HttpResponse(StatusCode.OK,"",null,);
		}

		[Entry(HttpMethod.GET, "nodes", EntryMatchType.Exact)]
		public static HttpResponse GetNodes(ApiEntryArgs args)
		{
			return new HttpResponse(StatusCode.OK, JsonConvert.SerializeObject(NodeManager.RegisteredNodes, Formatting.Indented), null, Encoding.UTF8, ContentType.application_json);
		}
	}
}
