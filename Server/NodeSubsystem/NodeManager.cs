using CICD.Common.Node;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.Server.NodeSubsystem
{
	public class NodeManager
	{
		private static string Path = "NodeData.json"; // Path to the node data file
		public static List<Node> RegisteredNodes { get; private set; }
		public static void Initialize()
		{
			RegisteredNodes = new List<Node>();
			LoadNodeData();
			Console.WriteLine("NodeManager initialized.");
		}
		public static void LoadNodeData()
		{
			RegisteredNodes.Clear();
			if (File.Exists(Path))
			{
				RegisteredNodes = JsonConvert.DeserializeObject<List<Node>>(System.IO.File.ReadAllText(Path)) ?? new List<Node>();
				RegisteredNodes.ForEach((node) => { node.Status = NodeStatus.Unreachable; });
			}
			Console.WriteLine($"Loaded {RegisteredNodes.Count} nodes from {Path}.");
		}
		public static void SaveNodeData()
		{
			File.WriteAllText(Path, JsonConvert.SerializeObject(RegisteredNodes, Formatting.Indented));
			Console.WriteLine($"Saved {RegisteredNodes.Count} nodes to {Path}.");
		}
		public static string RegisterNode(NodeInformation iNode)
		{
			// Logic to register a new node
			Console.WriteLine($"Registering node {iNode.Name} with version {iNode.Version} from IP {iNode.IP}.");
			string newName = "Worker-" + RegisteredNodes.Count;
			Node newNode = new Node
			{
				Id = newName,
				Name = iNode.Name,
				Version = iNode.Version,
				IP = iNode.IP,
				Status = iNode.Status,
				LastCheckin = DateTime.UtcNow
			};
			RegisteredNodes.Add(newNode);
			SaveNodeData();
			return newName;
		}
		public static void NodeCheckin(NodeInformation iNode)
		{
			// Logic to handle node check-in
			Console.WriteLine($"Node {iNode.ID} checked in with version {iNode.Version} from IP {iNode.IP}.");
			// Update node status or perform other actions as needed
			Node registered = RegisteredNodes.Find(Node => Node.Id == iNode.ID);
			RegisteredNodes.Remove(registered);
			registered.LastCheckin = DateTime.UtcNow;
			if (registered == null)
			{
				Console.WriteLine($"Node {iNode.ID} not found for check-in.");
				return;
			}
			registered.Status = iNode.Status;
			RegisteredNodes.Add(registered);
		}
		public static Node GetNode(string id)
		{
			// Logic to retrieve a node by its ID
			return RegisteredNodes.FirstOrDefault(node => node.Id == id);
		}
	}
}
