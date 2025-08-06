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
			return Respond.Text(NodeManager.RegisterNode(info));
		}

		[Entry(HttpMethod.POST, "nodes/checkin", EntryMatchType.Exact)]
		public static HttpResponse Checkin(ApiEntryArgs args)
		{
			NodeInformation info = JsonConvert.DeserializeObject<NodeInformation>(args.Request.Body);
			NodeManager.NodeCheckin(info);
			bool foundTasks = true;// TaskManager.GetTasksForNode(info.ID).Count > 0;
			return Respond.Json(foundTasks);
		}

		[Entry(HttpMethod.GET, "nodes", EntryMatchType.Exact)]
		public static HttpResponse GetNodes(ApiEntryArgs args)
		{
			return Respond.Json(NodeManager.RegisteredNodes);
		}
	}
}
