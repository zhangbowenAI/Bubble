
using HDJ.Framework.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HDJ.Framework.Utils
{
	public static class JsonUtils
	{
		private static Type list_Type = typeof(List<>);

		private static Type dictionary_Type = typeof(Dictionary<, >);

		private static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

		private static Type notJsonSerialized_Type = typeof(NotJsonSerializedAttribute);

		public static string ToJson(object data)
		{
			object obj = ChangeObjectToJsonObject(data);
			if (obj == null)
			{
				return string.Empty;
			}
			return SimpleJsonTool.SerializeObject(obj);
		}

		public static T FromJson<T>(string json)
		{
			object obj = FromJson(typeof(T), json);
			return (obj != null) ? ((T)obj) : default(T);
		}

		public static object FromJson(Type type, string json)
		{
			object data = SimpleJsonTool.DeserializeObject(json);
			return ChangeJsonDataToObjectByType(type, data);
		}

		private static object JsonToList(string json, Type itemType)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			object obj = SimpleJsonTool.DeserializeObject(json);
			return JsonObjectToList(obj, itemType);
		}

		private static object JsonObjectToList(object obj, Type itemType)
		{
			IList<object> list = obj as IList<object>;
			Type type = list_Type.MakeGenericType(itemType);
			object obj2 = ReflectionUtils.CreateDefultInstance(type);
			if (list == null || list.Count == 0)
			{
				return obj2;
			}
			MethodInfo method = type.GetMethod("Add");
			for (int i = 0; i < list.Count; i++)
			{
				object data = list[i];
				data = ChangeJsonDataToObjectByType(itemType, data);
				if (data != null)
				{
					method.Invoke(obj2, new object[1]
					{
						data
					});
				}
			}
			return obj2;
		}

		private static string ListToJson(object datas)
		{
			object json = ListArrayToJsonObject(datas, isList: true);
			return SimpleJsonTool.SerializeObject(json);
		}

		private static object ListArrayToJsonObject(object datas, bool isList)
		{
			Type type = datas.GetType();
			PropertyInfo propertyInfo = null;
			propertyInfo = ((!isList) ? type.GetProperty("Length") : type.GetProperty("Count"));
			int num = (int)propertyInfo.GetValue(datas, null);
			MethodInfo methodInfo = null;
			methodInfo = ((!isList) ? type.GetMethod("GetValue", new Type[1]
			{
				typeof(int)
			}) : type.GetMethod("get_Item", flags));
			List<object> list = new List<object>();
			for (int i = 0; i < num; i++)
			{
				object data = methodInfo.Invoke(datas, new object[1]
				{
					i
				});
				data = ChangeObjectToJsonObject(data);
				if (data != null)
				{
					list.Add(data);
				}
			}
			return list;
		}

		private static object JsonToArray(string json, Type itemType)
		{
			object data = SimpleJsonTool.DeserializeObject(json);
			return JsonObjectToArray(data, itemType);
		}

		private static object JsonObjectToArray(object data, Type itemType)
		{
			object obj = JsonObjectToList(data, itemType);
			MethodInfo method = obj.GetType().GetMethod("ToArray");
			return method.Invoke(obj, null);
		}

		private static string ArrayToJson(object datas)
		{
			object json = ListArrayToJsonObject(datas, isList: false);
			return SimpleJsonTool.SerializeObject(json);
		}

		private static string ClassOrStructToJson(object data)
		{
			object json = ClassOrStructToJsonObject(data);
			return SimpleJsonTool.SerializeObject(json);
		}

		private static object ClassOrStructToJsonObject(object data)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = data.GetType();
			FieldInfo[] fields = type.GetFields(flags);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!ReflectionUtils.IsDelegate(fieldInfo.FieldType) && !CheckHaveNotJsonSerializedAttribute(fieldInfo))
				{
					try
					{
						object value = fieldInfo.GetValue(data);
						string name = fieldInfo.Name;
						if (value != null)
						{
							value = ChangeObjectToJsonObject(value);
							dictionary.Add(name, value);
						}
					}
					catch
					{
					}
				}
			}
			PropertyInfo[] properties = type.GetProperties(flags);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead && propertyInfo.CanWrite && !ReflectionUtils.IsDelegate(propertyInfo.PropertyType) && !CheckHaveNotJsonSerializedAttribute(propertyInfo))
				{
					try
					{
						object value2 = propertyInfo.GetValue(data, null);
						if (value2 != null)
						{
							value2 = ChangeObjectToJsonObject(value2);
							dictionary.Add(propertyInfo.Name, value2);
						}
					}
					catch
					{
					}
				}
			}
			return dictionary;
		}

		private static object JsonToClassOrStruct(string json, Type type)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			object jsonObj = SimpleJsonTool.DeserializeObject(json);
			return JsonObjectToClassOrStruct(jsonObj, type);
		}

		private static object JsonObjectToClassOrStruct(object jsonObj, Type type)
		{
			IDictionary<string, object> dictionary = (IDictionary<string, object>)jsonObj;
			object obj = ReflectionUtils.CreateDefultInstance(type);
			if (dictionary == null || obj == null)
			{
				return null;
			}
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				string key = item.Key;
				object value = item.Value;
				FieldInfo field = type.GetField(key, flags);
				if (field != null)
				{
					value = ChangeJsonDataToObjectByType(field.FieldType, value);
					field.SetValue(obj, value);
				}
				else
				{
					PropertyInfo property = type.GetProperty(key, flags);
					if (property != null && property.CanRead && property.CanWrite)
					{
						value = ChangeJsonDataToObjectByType(property.PropertyType, value);
						property.SetValue(obj, value, null);
					}
				}
			}
			return obj;
		}

		private static string DictionaryToJson(object data)
		{
			object json = DictionaryToJsonObject(data);
			return SimpleJsonTool.SerializeObject(json);
		}

		private static object DictionaryToJsonObject(object data)
		{
			Type type = data.GetType();
			PropertyInfo property = type.GetProperty("Count");
			int num = (int)property.GetValue(data, null);
			MethodInfo method = type.GetMethod("GetEnumerator");
			PropertyInfo property2 = method.ReturnParameter.ParameterType.GetProperty("Current");
			MethodInfo method2 = method.ReturnParameter.ParameterType.GetMethod("MoveNext");
			Type[] genericArguments = type.GetGenericArguments();
			Type type2 = typeof(KeyValuePair<, >).MakeGenericType(genericArguments);
			PropertyInfo property3 = type2.GetProperty("Key");
			PropertyInfo property4 = type2.GetProperty("Value");
			object obj = method.Invoke(data, null);
			Dictionary<object, object> dictionary = new Dictionary<object, object>();
			for (int i = 0; i < num; i++)
			{
				method2.Invoke(obj, null);
				object value = property2.GetValue(obj, null);
				object value2 = property3.GetValue(value, null);
				object value3 = property4.GetValue(value, null);
				value2 = ChangeObjectToJsonObject(value2);
				value3 = ChangeObjectToJsonObject(value3);
				if (value2 != null && value3 != null)
				{
					dictionary.Add(value2, value3);
				}
			}
			return dictionary;
		}

		private static object JsonToDictionary(string json, Type keyType, Type valueType)
		{
			object data = SimpleJsonTool.DeserializeObject(json);
			return JsonObjectToDictionary(data, keyType, valueType);
		}

		private static object JsonObjectToDictionary(object data, Type keyType, Type valueType)
		{
			IList<object> list = data as IList<object>;
			Type type = dictionary_Type.MakeGenericType(keyType, valueType);
			object obj = Activator.CreateInstance(type);
			MethodInfo method = type.GetMethod("Add", flags);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					IDictionary<string, object> dictionary = list[i] as IDictionary<string, object>;
					object data2 = dictionary["Key"];
					object data3 = dictionary["Value"];
					data2 = ChangeJsonDataToObjectByType(keyType, data2);
					data3 = ChangeJsonDataToObjectByType(valueType, data3);
					method.Invoke(obj, new object[2]
					{
						data2,
						data3
					});
				}
			}
			return obj;
		}

		private static bool IsSupportBaseValueParseJson(Type t)
		{
			if (t.IsPrimitive || t == typeof(string) || t.IsEnum)
			{
				return true;
			}
			return false;
		}

		private static object ChangeJsonDataToObjectByType(Type type, object data)
		{
			object obj = null;
			if (data == null)
			{
				return obj;
			}
			if (type.IsPrimitive || type == typeof(string))
			{
				obj = data;
			}
			else if (type.IsEnum)
			{
				obj = Enum.Parse(type, data.ToString());
			}
			else if (type.IsArray)
			{
				try
				{
					obj = JsonObjectToArray(data, type.GetElementType());
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError("Array无法转换类型， data：" + data.GetType().FullName + "  type.GetElementType(): " + type.GetElementType().FullName);
					UnityEngine.Debug.LogError(message);
				}
			}
			else if (type.IsGenericType)
			{
				if (list_Type.Name == type.Name)
				{
					obj = JsonObjectToList(data, type.GetGenericArguments()[0]);
				}
				else if (dictionary_Type.Name == type.Name)
				{
					Type[] genericArguments = type.GetGenericArguments();
					obj = JsonObjectToDictionary(data, genericArguments[0], genericArguments[1]);
				}
				else
				{
					obj = JsonObjectToClassOrStruct(data, type);
				}
			}
			else if (type.IsClass || type.IsValueType)
			{
				obj = JsonObjectToClassOrStruct(data, type);
			}
			if (obj == null)
			{
				return obj;
			}
			try
			{
				if (type.Equals(obj.GetType()))
				{
					return obj;
				}
				obj = Convert.ChangeType(obj, type);
				return obj;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("无法转换类型， type：" + type.FullName + "  valueType: " + obj.GetType().FullName + "\n " + ex);
				return obj;
			}
		}

		private static bool CheckHaveNotJsonSerializedAttribute(MemberInfo member)
		{
			object[] customAttributes = member.GetCustomAttributes(inherit: false);
			bool result = false;
			object[] array = customAttributes;
			foreach (object obj in array)
			{
				if (obj.GetType() == notJsonSerialized_Type)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private static object ChangeObjectToJsonObject(object data)
		{
			if (data == null)
			{
				return data;
			}
			Type type = data.GetType();
			if (ReflectionUtils.IsDelegate(type))
			{
				return null;
			}
			object result = data;
			if (!IsSupportBaseValueParseJson(type))
			{
				if (type.IsArray)
				{
					result = ListArrayToJsonObject(data, isList: false);
				}
				else if (type.IsClass || type.IsGenericType)
				{
					result = ((list_Type.Name == type.Name) ? ListArrayToJsonObject(data, isList: true) : ((!(dictionary_Type.Name == type.Name)) ? ClassOrStructToJsonObject(data) : DictionaryToJsonObject(data)));
				}
				else if (type.IsClass || type.IsValueType)
				{
					result = ClassOrStructToJsonObject(data);
				}
			}
			return result;
		}
	}
}
