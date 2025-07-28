namespace CICD.Common.Node
{
	public struct NodeInformation
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string IP { get; set; }
		public string Version { get; set; }
		public NodeStatus Status { get; set; }
	}
}
