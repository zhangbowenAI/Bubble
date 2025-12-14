
using FrameWork;
using System;
using System.Collections.Generic;
using System.Reflection;

public class Deserializer
{
	public abstract class CustomCreator
	{
		public abstract object Create(Dictionary<string, object> src, Dictionary<string, object> parentSrc);

		public abstract Type TypeToCreate();
	}

	private Dictionary<Type, CustomCreator> m_creators;

	private BindingFlags m_fieldFlags;

	public Deserializer(bool includePrivateFields = false)
	{
		m_creators = new Dictionary<Type, CustomCreator>();
		m_fieldFlags = (BindingFlags)(0x14 | (includePrivateFields ? 32 : 0));
	}

	public static Type GetTypeByString(string typeStr)
	{
		return Type.GetType(typeStr);
	}

	public T Deserialize<T>(string json)
	{
		object o = Json.Deserialize(json);
		return Deserialize<T>(o);
	}

	public T Deserialize<T>(object o)
	{
		return (T)ConvertToType(o, typeof(T), null);
	}

	public object Deserialize(Type type, string json)
	{
		object value = Json.Deserialize(json);
		return ConvertToType(value, type, null);
	}

	public object Deserialize(string typeName, string json)
	{
		object value = Json.Deserialize(json);
		Type type = Type.GetType(typeName);
		return ConvertToType(value, type, null);
	}

	public void RegisterCreator(CustomCreator creator)
	{
		Type key = creator.TypeToCreate();
		m_creators[key] = creator;
	}

	private object DeserializeO(Type destType, Dictionary<string, object> src, Dictionary<string, object> parentSrc)
	{
		object obj = null;
		if (destType == typeof(object) || destType == typeof(Dictionary<string, object>))
		{
			return src;
		}
		if (m_creators.TryGetValue(destType, out CustomCreator value))
		{
			obj = value.Create(src, parentSrc);
		}
		if (obj == null)
		{
			if (src.TryGetValue("$dotNetType", out object value2))
			{
				destType = Type.GetType((string)value2);
			}
			obj = Activator.CreateInstance(destType);
		}
		DeserializeIt(obj, src);
		return obj;
	}

	private void DeserializeIt(object dest, Dictionary<string, object> src)
	{
		Type type = dest.GetType();
		FieldInfo[] fields = type.GetFields(m_fieldFlags);
		DeserializeClassFields(dest, fields, src);
	}

	private void DeserializeClassFields(object dest, FieldInfo[] fields, Dictionary<string, object> src)
	{
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!fieldInfo.IsStatic && src.TryGetValue(fieldInfo.Name, out object value))
			{
				DeserializeField(dest, fieldInfo, value, src);
			}
		}
	}

	private void DeserializeField(object dest, FieldInfo info, object value, Dictionary<string, object> src)
	{
		Type fieldType = info.FieldType;
		object obj = ConvertToType(value, fieldType, src);
		if (fieldType.IsAssignableFrom(obj.GetType()))
		{
			info.SetValue(dest, obj);
		}
	}

	public static bool IsGenericList(Type type)
	{
		return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
	}

	public static bool IsGenericDictionary(Type type)
	{
		return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >);
	}

	private object ConvertToType(object value, Type type, Dictionary<string, object> src)
	{
		if (type.IsArray)
		{
			return ConvertToArray(value, type, src);
		}
		if (IsGenericList(type))
		{
			return ConvertToList(value, type, src);
		}
		if (IsGenericDictionary(type))
		{
			return ConvertToDictionary(value, type, src);
		}
		if (type.IsEnum)
		{
			return Enum.Parse(type, Convert.ToString(value));
		}
		if (type == typeof(string))
		{
			return Convert.ToString(value);
		}
		if (type == typeof(byte))
		{
			return Convert.ToByte(value);
		}
		if (type == typeof(sbyte))
		{
			return Convert.ToSByte(value);
		}
		if (type == typeof(short))
		{
			return Convert.ToInt16(value);
		}
		if (type == typeof(ushort))
		{
			return Convert.ToUInt16(value);
		}
		if (type == typeof(int))
		{
			return Convert.ToInt32(value);
		}
		if (type == typeof(uint))
		{
			return Convert.ToUInt32(value);
		}
		if (type == typeof(long))
		{
			return Convert.ToInt64(value);
		}
		if (type == typeof(ulong))
		{
			return Convert.ToUInt64(value);
		}
		if (type == typeof(char))
		{
			return Convert.ToChar(value);
		}
		if (type == typeof(double))
		{
			return Convert.ToDouble(value);
		}
		if (type == typeof(float))
		{
			return Convert.ToSingle(value);
		}
		if (type == typeof(int))
		{
			return Convert.ToInt32(value);
		}
		if (type == typeof(float))
		{
			return Convert.ToSingle(value);
		}
		if (type == typeof(double))
		{
			return Convert.ToDouble(value);
		}
		if (type == typeof(bool))
		{
			return Convert.ToBoolean(value);
		}
		if (type == typeof(bool))
		{
			return Convert.ToBoolean(value);
		}
		if (type.IsValueType)
		{
			return DeserializeO(type, (Dictionary<string, object>)value, src);
		}
		if (type == typeof(object))
		{
			return value;
		}
		if (type.IsClass)
		{
			return DeserializeO(type, (Dictionary<string, object>)value, src);
		}
		return value;
	}

	private object ConvertToDictionary(object value, Type type, Dictionary<string, object> src)
	{
		Type genericTypeDefinition = type.GetGenericTypeDefinition();
		Type[] genericArguments = type.GetGenericArguments();
		Type type2 = genericTypeDefinition.MakeGenericType(genericArguments);
		object obj = Activator.CreateInstance(type2);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)value;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			object obj2 = ConvertToType(item.Key, genericArguments[0], src);
			object obj3 = ConvertToType(item.Value, genericArguments[1], src);
			obj.GetType().GetMethod("Add").Invoke(obj, new object[2]
			{
				obj2,
				obj3
			});
		}
		return obj;
	}

	private object ConvertToList(object value, Type type, Dictionary<string, object> src)
	{
		Type genericTypeDefinition = type.GetGenericTypeDefinition();
		Type[] genericArguments = type.GetGenericArguments();
		Type type2 = genericTypeDefinition.MakeGenericType(genericArguments);
		object obj = Activator.CreateInstance(type2);
		List<object> list = (List<object>)value;
		int num = 0;
		foreach (object item in list)
		{
			object obj2 = ConvertToType(item, genericArguments[0], src);
			obj.GetType().GetMethod("Add").Invoke(obj, new object[1]
			{
				obj2
			});
			num++;
		}
		return obj;
	}

	private object ConvertToArray(object value, Type type, Dictionary<string, object> src)
	{
		List<object> list = (List<object>)value;
		int count = list.Count;
		Type elementType = type.GetElementType();
		Array array = Array.CreateInstance(elementType, count);
		int num = 0;
		foreach (object item in list)
		{
			object value2 = ConvertToType(item, elementType, src);
			array.SetValue(value2, num);
			num++;
		}
		return array;
	}
}
