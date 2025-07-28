using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.Server.GlobalKVS
{
	public class KeyValueStore
	{
		private static readonly string filePath = "kvs.json"; // Path to the key-value store file
		private static Dictionary<string, string> store;
		public static void Initialize()
		{
			store = new Dictionary<string, string>();
			Load();
			Console.WriteLine("KeyValueStore initialized.");
		}
		public static void Load()
		{
			if (File.Exists(filePath))
			{
				store = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
			}
		}

		public static void Save()
		{
			File.WriteAllText(filePath, JsonConvert.SerializeObject(store, Formatting.Indented));
		}
		public static void Set(string key, string value)
		{
			store[key] = value;
			Save();
		}
		public static bool Remove(string key)
		{
			if (store.ContainsKey(key))
			{
				store.Remove(key);
				Save();
				return true;
			}
			Console.WriteLine($"Key '{key}' not found in the store.");
			return false;
		}
		public static string Get(string key)
		{
			if (store.TryGetValue(key, out string value))
			{
				return value;
			}
			return null; // or throw an exception if preferred
		}
	}
}
