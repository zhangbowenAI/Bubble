
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

public class Serializer
{
	private StringBuilder m_builder;

	private bool m_includeTypeInfoForDerivedTypes;

	private bool m_prettyPrint;

	private bool m_includePrivateFields;

	private BindingFlags m_fieldFlags;

	private string m_prefix;

	private Serializer(bool includeTypeInfoForDerivedTypes, bool prettyPrint, bool includePrivateFields)
	{
		m_builder = new StringBuilder();
		m_includeTypeInfoForDerivedTypes = includeTypeInfoForDerivedTypes;
		m_prettyPrint = prettyPrint;
		m_includePrivateFields = includePrivateFields;
		m_prefix = string.Empty;
		m_fieldFlags = (BindingFlags)(0x14 | (m_includePrivateFields ? 32 : 0));
	}

	public static string Serialize(object obj, bool includeTypeInfoForDerivedTypes = false, bool prettyPrint = false, bool includePrivateFields = false)
	{
		Serializer serializer = new Serializer(includeTypeInfoForDerivedTypes, prettyPrint, includePrivateFields);
		serializer.SerializeValue(obj);
		return serializer.GetJson();
	}

	private string GetJson()
	{
		return m_builder.ToString();
	}

	private void Indent()
	{
		if (m_prettyPrint)
		{
			m_prefix += "  ";
		}
	}

	private void Outdent()
	{
		if (m_prettyPrint)
		{
			m_prefix = m_prefix.Substring(2);
		}
	}

	private void AddIndent()
	{
		if (m_prettyPrint)
		{
			m_builder.Append(m_prefix);
		}
	}

	private void AddLine()
	{
		if (m_prettyPrint)
		{
			m_builder.Append("\n");
		}
	}

	private void AddSpace()
	{
		if (m_prettyPrint)
		{
			m_builder.Append(" ");
		}
	}

	private void SerializeValue(object obj)
	{
		if (obj == null)
		{
			m_builder.Append("undefined");
			return;
		}
		Type type = obj.GetType();
		if (type.IsArray)
		{
			SerializeArray(obj);
		}
		else if (Deserializer.IsGenericList(type))
		{
			Type type2 = type.GetGenericArguments()[0];
			MethodInfo methodInfo = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(type2);
			MethodInfo methodInfo2 = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(type2);
			object obj2 = methodInfo.Invoke(null, new object[1]
			{
				obj
			});
			object obj3 = methodInfo2.Invoke(null, new object[1]
			{
				obj2
			});
			SerializeArray(obj3);
		}
		else if (type.IsEnum)
		{
			SerializeString(obj.ToString());
		}
		else if (type == typeof(string))
		{
			SerializeString(obj as string);
		}
		else if (type == typeof(char))
		{
			SerializeString(obj.ToString());
		}
		else if (type == typeof(bool))
		{
			m_builder.Append((!(bool)obj) ? "false" : "true");
		}
		else if (type == typeof(bool))
		{
			m_builder.Append((!(bool)obj) ? "false" : "true");
			m_builder.Append(Convert.ChangeType(obj, typeof(string)));
		}
		else if (type == typeof(int))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(byte))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(sbyte))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(short))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(ushort))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(int))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(uint))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(long))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(ulong))
		{
			m_builder.Append(obj);
		}
		else if (type == typeof(float))
		{
			m_builder.Append(((float)obj).ToString("R", CultureInfo.InvariantCulture));
		}
		else if (type == typeof(double))
		{
			m_builder.Append(((double)obj).ToString("R", CultureInfo.InvariantCulture));
		}
		else if (type == typeof(float))
		{
			m_builder.Append(((float)obj).ToString("R", CultureInfo.InvariantCulture));
		}
		else if (type == typeof(double))
		{
			m_builder.Append(((double)obj).ToString("R", CultureInfo.InvariantCulture));
		}
		else if (type.IsValueType)
		{
			SerializeObject(obj);
		}
		else
		{
			if (!type.IsClass)
			{
				throw new InvalidOperationException("unsupport type: " + type.Name);
			}
			SerializeObject(obj);
		}
	}

	private void SerializeArray(object obj)
	{
		m_builder.Append("[");
		AddLine();
		Indent();
		Array array = obj as Array;
		bool flag = true;
		IEnumerator enumerator = array.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if (!flag)
				{
					m_builder.Append(",");
					AddLine();
				}
				AddIndent();
				SerializeValue(current);
				flag = false;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		AddLine();
		Outdent();
		AddIndent();
		m_builder.Append("]");
	}

	private void SerializeDictionary(IDictionary obj)
	{
		bool flag = true;
		IEnumerator enumerator = obj.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if (!flag)
				{
					m_builder.Append(',');
					AddLine();
				}
				AddIndent();
				SerializeString(current.ToString());
				m_builder.Append(':');
				AddSpace();
				SerializeValue(obj[current]);
				flag = false;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void SerializeObject(object obj)
	{
		m_builder.Append("{");
		AddLine();
		Indent();
		bool flag = true;
		if (m_includeTypeInfoForDerivedTypes)
		{
			Type type = obj.GetType();
			Type baseType = type.BaseType;
			if (baseType != null && baseType != typeof(object))
			{
				AddIndent();
				SerializeString("$dotNetType");
				m_builder.Append(":");
				AddSpace();
				SerializeString(type.AssemblyQualifiedName);
			}
		}
		IDictionary obj2;
		if ((obj2 = (obj as IDictionary)) != null)
		{
			SerializeDictionary(obj2);
		}
		else
		{
			FieldInfo[] fields = obj.GetType().GetFields(m_fieldFlags);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo.IsStatic)
				{
					continue;
				}
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (!flag)
					{
						m_builder.Append(",");
						AddLine();
					}
					AddIndent();
					SerializeString(fieldInfo.Name);
					m_builder.Append(":");
					AddSpace();
					SerializeValue(value);
					flag = false;
				}
			}
		}
		AddLine();
		Outdent();
		AddIndent();
		m_builder.Append("}");
	}

	private void SerializeString(string str)
	{
		m_builder.Append('"');
		char[] array = str.ToCharArray();
		char[] array2 = array;
		foreach (char c in array2)
		{
			switch (c)
			{
			case '"':
				m_builder.Append("\\\"");
				continue;
			case '\\':
				m_builder.Append("\\\\");
				continue;
			case '\b':
				m_builder.Append("\\b");
				continue;
			case '\f':
				m_builder.Append("\\f");
				continue;
			case '\n':
				m_builder.Append("\\n");
				continue;
			case '\r':
				m_builder.Append("\\r");
				continue;
			case '\t':
				m_builder.Append("\\t");
				continue;
			}
			int num = Convert.ToInt32(c);
			if (num >= 32 && num <= 126)
			{
				m_builder.Append(c);
				continue;
			}
			m_builder.Append("\\u");
			m_builder.Append(num.ToString("x4"));
		}
		m_builder.Append('"');
	}
}
