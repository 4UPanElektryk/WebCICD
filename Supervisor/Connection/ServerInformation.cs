namespace CICD.Supervisor.Connection
{
	public class ServerInformation
	{
		public string Address { get; set; }
		public int Port { get; set; }

		public string Uri()
		{
			return $"http://{Address}:{Port}";
		}
	}
}
