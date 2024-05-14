using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JobChart.DemoSubway
{
	internal class JsonHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static bool SerializeToFile<T>(string filePath, T t, bool log = true) where T : class
		{
			var settings = new JsonSerializerSettings
			{
				//ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(), //设置序列化时key为驼峰样式
				Formatting = Formatting.Indented, // 带缩进
				TypeNameHandling = TypeNameHandling.Auto
			};
			return SerializeToFile(filePath, t, settings, log);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <param name="t"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		public static bool SerializeToFile<T>(string filePath, T t, JsonSerializerSettings settings, bool log = true) where T : class
		{
			bool isOk;
			try
			{
				using (StreamWriter stWriter = new StreamWriter(filePath))
				{
					var serializer = settings != null ? JsonSerializer.Create(settings) : JsonSerializer.Create();
					serializer.Converters.Add(new JavaScriptDateTimeConverter());
					serializer.NullValueHandling = NullValueHandling.Ignore;

					JsonWriter jsWriter = new JsonTextWriter(stWriter);
					serializer.Serialize(jsWriter, t);
					jsWriter.Close();
					stWriter.Close();
				}

				isOk = true;
			}
			catch (Exception ex)
			{
				isOk = false;
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
			}

			return isOk;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">DataClass Type</typeparam>
		/// <param name="t">DataClass Instance</param>
		/// <returns>Serialized String</returns>
		public static string SerializeToJson<T>(T t, Formatting formatting = Formatting.None, bool log = true) where T : class
		{
			try
			{
				return JsonConvert.SerializeObject(t, formatting);
			}
			catch (Exception ex)
			{
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t">DataClass Instance</param>
		/// <returns>Serialized String</returns>
		public static string SerializeToJson(object t, Formatting formatting = Formatting.None, bool log = true)
		{
			try
			{
				return JsonConvert.SerializeObject(t, formatting);
			}
			catch (Exception ex)
			{
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static T DeserializeFromFile<T>(string filePath, bool log = true) where T : class
		{
			T obj; //= new T();
			try
			{
				if (!File.Exists(filePath)) return default;

				using (StreamReader stReader = new StreamReader(filePath))
				{
					var settings = new JsonSerializerSettings
					{
						TypeNameHandling = TypeNameHandling.Auto,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};

					JsonSerializer js = new JsonSerializer();
					js.Converters.Add(new JavaScriptDateTimeConverter());
					js.NullValueHandling = NullValueHandling.Ignore;
					JsonReader jsReader = new JsonTextReader(stReader);
					var jobj = js.Deserialize(jsReader);
					obj = JsonConvert.DeserializeObject<T>(jobj?.ToString(), settings);
					//obj = js.Deserialize<T>(jsReader);
				}
			}
			catch (Exception ex)
			{
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
				obj = default;
			}

			return obj;
		}

		public static T DeserializeFromJson<T>(string json, bool log = true) where T : class
		{
			T obj;
			try
			{
				var settings = new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
					MissingMemberHandling = MissingMemberHandling.Ignore
				};
				obj = JsonConvert.DeserializeObject<T>(json, settings);
			}
			catch (Exception ex)
			{
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
				obj = default;
			}

			return obj;
		}

		public static object DeserializeFromJson(string json, Type type, bool log = true)
		{
			object obj;
			try
			{
				var settings = new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
					MissingMemberHandling = MissingMemberHandling.Ignore
				};
				obj = JsonConvert.DeserializeObject(json, type, settings);
			}
			catch (Exception ex)
			{
				if (log)
				{
					LogManager.Error(ex.Message + "\r\n" + ex.StackTrace);
					if (ex.InnerException != null)
						LogManager.Error(ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace);
				}
				obj = null;
			}

			return obj;
		}
	}
}