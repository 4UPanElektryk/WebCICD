using CICD.Common.Node;
using CICD.Server.NodeSubsystem;
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
	}
}
