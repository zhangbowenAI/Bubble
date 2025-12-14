
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HDJ.Framework.Utils
{
	public static class ReflectionUtils
	{
		public static readonly Assembly Assembly_CSharp = Assembly.Load("Assembly-CSharp");

		public static readonly Assembly Assembly_UnityEngine = Assembly.Load("UnityEngine");

		private const BindingFlags flagsInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private const BindingFlags flagsStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static Dictionary<string, object> GetClassOrStructData(object data, bool containsPropertyInfo = false)
		{
			Type type = data.GetType();
			FieldInfo[] fields = type.GetFields();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (int i = 0; i < fields.Length; i++)
			{
				object value = fields[i].GetValue(data);
				string name = fields[i].Name;
				if (value != null)
				{
					dictionary.Add(name, value);
				}
			}
			if (!containsPropertyInfo)
			{
				return dictionary;
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead)
				{
					try
					{
						string name2 = propertyInfo.Name;
						object value2 = propertyInfo.GetValue(data, null);
						dictionary.Add(name2, value2);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(ex.ToString());
					}
				}
			}
			return dictionary;
		}

		public static object SetClassOrStructData(Dictionary<string, object> dic, Type type, bool containsPropertyInfo = false, object instance = null)
		{
			object obj = instance;
			if (obj == null)
			{
				obj = Activator.CreateInstance(type);
			}
			FieldInfo[] fields = type.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				string name = fields[i].Name;
				if (dic.ContainsKey(name))
				{
					fields[i].SetValue(obj, dic[name]);
				}
			}
			if (!containsPropertyInfo)
			{
				return obj;
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int j = 0; j < properties.Length; j++)
			{
				PropertyInfo propertyInfo = properties[j];
				if (propertyInfo.CanWrite)
				{
					try
					{
						string name2 = properties[j].Name;
						if (dic.ContainsKey(name2))
						{
							properties[j].SetValue(obj, dic[name2], null);
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(ex.ToString());
					}
				}
			}
			return obj;
		}

		public static Type[] GetChildTypes(Type parentType, bool isContainsAllChild = true)
		{
			List<Type> list = new List<Type>();
			Assembly assembly = Assembly.GetAssembly(parentType);
			Type[] types = assembly.GetTypes();
			Type[] array = types;
			foreach (Type type in array)
			{
				if (type.BaseType != parentType)
				{
					continue;
				}
				list.Add(type);
				if (isContainsAllChild)
				{
					Type[] childTypes = GetChildTypes(type, isContainsAllChild);
					if (childTypes.Length > 0)
					{
						list.AddRange(childTypes);
					}
				}
			}
			return list.ToArray();
		}

		public static Type GetTypeByTypeFullName(string typeFullName, bool isShowErrorLog = true)
		{
			Type type = Type.GetType(typeFullName);
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			type = executingAssembly.GetType(typeFullName);
			if (type == null && Assembly_CSharp != null && executingAssembly != Assembly_CSharp)
			{
				type = Assembly_CSharp.GetType(typeFullName);
			}
			if (type == null && Assembly_UnityEngine != null && executingAssembly != Assembly_UnityEngine)
			{
				type = Assembly_UnityEngine.GetType(typeFullName);
			}
			if (type == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Assembly assembly = assemblies[i];
					if (assembly != executingAssembly && assembly != Assembly_CSharp && assembly != Assembly_UnityEngine)
					{
						type = assemblies[i].GetType(typeFullName);
						if (type != null)
						{
							break;
						}
					}
				}
			}
			if (type == null && isShowErrorLog)
			{
				UnityEngine.Debug.LogError("无法找到类型：" + typeFullName);
			}
			return type;
		}

		public static Type GetTypefromAssemblyFullName(string AssemblyName, string fullName)
		{
			Assembly assembly = Assembly.Load(AssemblyName);
			return assembly.GetType(fullName);
		}

		public static object CreateDefultInstanceAll(Type type, bool isDeepParameters = false)
		{
			object obj = null;
			string text = string.Empty;
			if (type.IsArray)
			{
				obj = Activator.CreateInstance(type, 0);
			}
			else if (type.IsValueType)
			{
				obj = Activator.CreateInstance(type);
			}
			else
			{
				ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (ConstructorInfo constructorInfo in constructors)
				{
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					object[] array = new object[parameters.Length];
					for (int j = 0; j < array.Length; j++)
					{
						ParameterInfo parameterInfo = parameters[j];
						if (isDeepParameters)
						{
							array[j] = CreateDefultInstance(parameterInfo.ParameterType);
						}
						else
						{
							array[j] = null;
						}
					}
					try
					{
						obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null);
						if (obj == null)
						{
							continue;
						}
					}
					catch (Exception ex)
					{
						text = text + ex.ToString() + "\n";
						continue;
					}
					break;
				}
			}
			if (obj == null)
			{
				UnityEngine.Debug.LogError("Type :" + type + "\n" + text);
			}
			return obj;
		}

		public static object CreateDefultInstance(Type type)
		{
			if (type == null)
			{
				UnityEngine.Debug.LogError("Type不可为：null");
				return null;
			}
			object obj = CreateDefultInstanceAll(type);
			if (obj == null)
			{
				obj = CreateDefultInstanceAll(type, isDeepParameters: true);
			}
			if (obj == null)
			{
				UnityEngine.Debug.LogError("创建默认实例失败！Type:" + type.FullName);
			}
			return obj;
		}

		public static void SetFieldInfo(Type t, object instance, string fieldName, object value)
		{
			BindingFlags bindingAttr = (instance != null) ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo field = t.GetField(fieldName, bindingAttr);
			if (field == null)
			{
				UnityEngine.Debug.LogError("获取失败：type:" + t + "  fieldName: " + fieldName);
			}
			else
			{
				try
				{
					field.SetValue(instance, value);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
		}

		public static object GetFieldInfo(Type t, object instance, string fieldName)
		{
			BindingFlags bindingAttr = (instance != null) ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo field = t.GetField(fieldName, bindingAttr);
			if (field == null)
			{
				UnityEngine.Debug.LogError("获取失败：type:" + t + "  fieldName: " + fieldName);
				return null;
			}
			try
			{
				return field.GetValue(instance);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			return null;
		}

		public static void SetPropertyInfo(Type t, object instance, string propertyName, object value)
		{
			BindingFlags bindingAttr = (instance != null) ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo property = t.GetProperty(propertyName, bindingAttr);
			if (property == null)
			{
				UnityEngine.Debug.LogError("获取失败：type:" + t + "  fieldName: " + propertyName);
			}
			else if (!property.CanWrite)
			{
				UnityEngine.Debug.LogError("属性不能写入：type:" + t + "  fieldName: " + propertyName);
			}
			else
			{
				try
				{
					property.SetValue(instance, value, null);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
		}

		public static object GetPropertyInfo(Type t, object instance, string propertyName)
		{
			BindingFlags bindingAttr = (instance != null) ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo property = t.GetProperty(propertyName, bindingAttr);
			if (property == null)
			{
				UnityEngine.Debug.LogError("获取失败：type:" + t + "  fieldName: " + propertyName);
				return null;
			}
			if (!property.CanRead)
			{
				UnityEngine.Debug.LogError("属性不能读取：type:" + t + "  fieldName: " + propertyName);
				return null;
			}
			try
			{
				return property.GetValue(instance, null);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			return null;
		}

		public static object InvokMethod(Type t, object instance, string methodName, ref object[] paras)
		{
			BindingFlags bindingAttr = (instance != null) ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo method = t.GetMethod(methodName, bindingAttr);
			if (method == null)
			{
				UnityEngine.Debug.LogError("获取方法失败：type:" + t + "  methodName: " + methodName);
				return null;
			}
			try
			{
				return method.Invoke(instance, paras);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			return null;
		}

		public static string GetTypeName4CodeFormat(Type t)
		{
			string text = t.Name;
			if (t.IsGenericType)
			{
				text = text.Remove(text.IndexOf('`'));
				Type[] genericArguments = t.GetGenericArguments();
				string text2 = string.Empty;
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type t2 = genericArguments[i];
					text2 += GetTypeName4CodeFormat(t2);
					if (i < genericArguments.Length - 1)
					{
						text2 += ",";
					}
				}
				text = text + "<" + text2 + ">";
			}
			return text;
		}

		public static bool IsDelegate(Type type)
		{
			return type.BaseType == typeof(MulticastDelegate);
		}

		public static object DeepCopySelf(this object obj)
		{
			if (obj == null)
			{
				return null;
			}
			Type type = obj.GetType();
			if (type.IsValueType)
			{
				return obj;
			}
			if (obj is string)
			{
				return obj;
			}
			object obj2 = CreateDefultInstance(type);
			MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.MemberType == MemberTypes.Field)
				{
					FieldInfo fieldInfo = (FieldInfo)memberInfo;
					object value = fieldInfo.GetValue(obj);
					if (value is ICloneable)
					{
						fieldInfo.SetValue(obj2, (value as ICloneable).Clone());
					}
					else
					{
						fieldInfo.SetValue(obj2, value.DeepCopySelf());
					}
				}
				else
				{
					if (memberInfo.MemberType != MemberTypes.Property)
					{
						continue;
					}
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					if (propertyInfo.CanRead && propertyInfo.CanWrite)
					{
						object value2 = propertyInfo.GetValue(obj, null);
						if (value2 is ICloneable)
						{
							propertyInfo.SetValue(obj2, (value2 as ICloneable).Clone(), null);
						}
						else
						{
							propertyInfo.SetValue(obj2, value2.DeepCopySelf(), null);
						}
					}
				}
			}
			return obj2;
		}
	}
}
