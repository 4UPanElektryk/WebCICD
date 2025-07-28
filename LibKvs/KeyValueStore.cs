using System;
using System.Diagnostics;
using System.Net.Http;

namespace WebCICDKvs
{
	public class KeyValueStore
	{
		private static string kvsAddress;
		private static HttpClient client;
		public static void Initialize()
		{
			if (Environment.GetEnvironmentVariable("KVS_AVAILABLE") == null)
			{
				throw new InvalidOperationException("KeyValueStore is not available. Set KVS_AVAILABLE environment variable to true and Set KVS_ADDRESS");
			}
			kvsAddress = Environment.GetEnvironmentVariable("KVS_ADDRESS");
			client = new HttpClient();
			client.BaseAddress = new Uri(kvsAddress);
		}
		public static void Set(string key, string value)
		{
			if (string.IsNullOrEmpty(kvsAddress))
			{
				throw new InvalidOperationException("KeyValueStore is not initialized. Call Initialize() first.");
			}
			HttpResponseMessage res = client.PostAsync($"/kvs/{key}", new StringContent(value)).GetAwaiter().GetResult();
			if (!res.IsSuccessStatusCode)
			{
				throw new Exception($"Failed to set key: {key}. Status code: {res.StatusCode}");
			}
			Debug.WriteLine("KeyValueStore: Succesfully set: " + key + " with value: " + value);
		}
		public static string Get(string key)
		{
			if (string.IsNullOrEmpty(kvsAddress))
			{
				throw new InvalidOperationException("KeyValueStore is not initialized. Call Initialize() first.");
			}
			HttpResponseMessage res = client.GetAsync($"/kvs/{key}").GetAwaiter().GetResult();
			if (res.IsSuccessStatusCode)
			{
				return res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			}
			else
			{
				Debug.WriteLine($"KeyValueStore: Failed to get key: {key}. Status code: {res.StatusCode}");
				return null;
			}
		}
		public static void Delete(string key)
		{
			if (string.IsNullOrEmpty(kvsAddress))
			{
				throw new InvalidOperationException("KeyValueStore is not initialized. Call Initialize() first.");
			}
			HttpResponseMessage res = client.DeleteAsync($"/kvs/{key}").GetAwaiter().GetResult();
			if (!res.IsSuccessStatusCode)
			{
				throw new Exception($"Failed to delete key: {key}. Status code: {res.StatusCode}");
			}
			Debug.WriteLine("KeyValueStore: Successfully deleted key: " + key);
		}
	}
}
