
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace HDJ.Framework.Tools
{
	[GeneratedCode("simple-json", "1.0.0")]
	public static class SimpleJsonTool
	{
		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		private static IJsonSerializerStrategy _currentJsonSerializerStrategy;

		private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;

		private static Queue<StringBuilder> stringBuilders = new Queue<StringBuilder>();

		public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
		{
			get
			{
				IJsonSerializerStrategy result;
				if ((result = _currentJsonSerializerStrategy) == null)
				{
					result = (_currentJsonSerializerStrategy = PocoJsonSerializerStrategy);
				}
				return result;
			}
			set
			{
				_currentJsonSerializerStrategy = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy
		{
			get
			{
				PocoJsonSerializerStrategy result;
				if ((result = _pocoJsonSerializerStrategy) == null)
				{
					result = (_pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());
				}
				return result;
			}
		}

		public static object DeserializeObject(string json)
		{
			if (TryDeserializeObject(json, out object obj))
			{
				return obj;
			}
			throw new SerializationException("Invalid JSON string");
		}

		public static bool TryDeserializeObject(string json, out object obj)
		{
			bool success = true;
			if (json != null)
			{
				char[] json2 = json.ToCharArray();
				int index = 0;
				obj = ParseValue(json2, ref index, ref success);
			}
			else
			{
				obj = null;
			}
			return success;
		}

		public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			object obj = DeserializeObject(json);
			return (type == null || (obj != null && ReflectionsUtils.IsAssignableFrom(obj.GetType(), type))) ? obj : (jsonSerializerStrategy ?? CurrentJsonSerializerStrategy).DeserializeObject(obj, type);
		}

		public static object DeserializeObject(string json, Type type)
		{
			return DeserializeObject(json, type, null);
		}

		public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			return (T)DeserializeObject(json, typeof(T), jsonSerializerStrategy);
		}

		public static T DeserializeObject<T>(string json)
		{
			return (T)DeserializeObject(json, typeof(T), null);
		}

		public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			StringBuilder stringBuilder = GetStringBuilder();
			string result = SerializeValue(jsonSerializerStrategy, json, stringBuilder) ? stringBuilder.ToString() : null;
			RecycleStringBuilder(stringBuilder);
			return result;
		}

		public static string SerializeObject(object json)
		{
			return SerializeObject(json, CurrentJsonSerializerStrategy);
		}

		public static string EscapeToJavascriptString(string jsonString)
		{
			string result;
			if (string.IsNullOrEmpty(jsonString))
			{
				result = jsonString;
			}
			else
			{
				StringBuilder stringBuilder = GetStringBuilder();
				int num = 0;
				while (num < jsonString.Length)
				{
					char c = jsonString[num++];
					if (c == '\\')
					{
						int num3 = jsonString.Length - num;
						if (num3 >= 2)
						{
							switch (jsonString[num])
							{
							case '\\':
								stringBuilder.Append('\\');
								num++;
								break;
							case '"':
								stringBuilder.Append("\"");
								num++;
								break;
							case 't':
								stringBuilder.Append('\t');
								num++;
								break;
							case 'b':
								stringBuilder.Append('\b');
								num++;
								break;
							case 'n':
								stringBuilder.Append('\n');
								num++;
								break;
							case 'r':
								stringBuilder.Append('\r');
								num++;
								break;
							}
						}
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				result = stringBuilder.ToString();
				RecycleStringBuilder(stringBuilder);
			}
			return result;
		}

		private static IDictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
		{
			IDictionary<string, object> dictionary = new JsonObject();
			NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (LookAhead(json, index))
				{
				case 6:
					NextToken(json, ref index);
					continue;
				case 2:
					NextToken(json, ref index);
					return dictionary;
				case 0:
					success = false;
					return null;
				}
				string key = ParseString(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				int num = NextToken(json, ref index);
				if (num != 5)
				{
					success = false;
					return null;
				}
				object value = ParseValue(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				dictionary[key] = value;
			}
			return dictionary;
		}

		private static JsonArray ParseArray(char[] json, ref int index, ref bool success)
		{
			JsonArray jsonArray = new JsonArray();
			NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (LookAhead(json, index))
				{
				case 6:
					NextToken(json, ref index);
					continue;
				case 4:
					break;
				default:
				{
					object item = ParseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonArray.Add(item);
					continue;
				}
				case 0:
					success = false;
					return null;
				}
				NextToken(json, ref index);
				break;
			}
			return jsonArray;
		}

		private static object ParseValue(char[] json, ref int index, ref bool success)
		{
			switch (LookAhead(json, index))
			{
			case 1:
				return ParseObject(json, ref index, ref success);
			case 3:
				return ParseArray(json, ref index, ref success);
			case 7:
				return ParseString(json, ref index, ref success);
			case 8:
				return ParseNumber(json, ref index, ref success);
			case 9:
				NextToken(json, ref index);
				return true;
			case 10:
				NextToken(json, ref index);
				return false;
			case 11:
				NextToken(json, ref index);
				return null;
			default:
				success = false;
				return null;
			}
		}

		private static string ParseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder stringBuilder = GetStringBuilder();
			EatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag && index != json.Length)
			{
				c = json[index++];
				switch (c)
				{
				case '"':
					flag = true;
					break;
				case '\\':
				{
					if (index == json.Length)
					{
						break;
					}
					switch (json[index++])
					{
					case '"':
						stringBuilder.Append('"');
						continue;
					case '\\':
						stringBuilder.Append('\\');
						continue;
					case '/':
						stringBuilder.Append('/');
						continue;
					case 'b':
						stringBuilder.Append('\b');
						continue;
					case 'f':
						stringBuilder.Append('\f');
						continue;
					case 'n':
						stringBuilder.Append('\n');
						continue;
					case 'r':
						stringBuilder.Append('\r');
						continue;
					case 't':
						stringBuilder.Append('\t');
						continue;
					case 'u':
						break;
					default:
						continue;
					}
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint result)))
					{
						return string.Empty;
					}
					if (55296 > result || result > 56319)
					{
						stringBuilder.Append(ConvertFromUtf32((int)result));
						index += 4;
						continue;
					}
					index += 4;
					num = json.Length - index;
					uint result2;
					if (num >= 6 && new string(json, index, 2) == "\\u" && uint.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result2) && 56320 <= result2 && result2 <= 57343)
					{
						stringBuilder.Append((char)result);
						stringBuilder.Append((char)result2);
						index += 6;
						continue;
					}
					success = false;
					return string.Empty;
				}
				default:
					stringBuilder.Append(c);
					continue;
				}
				break;
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			string result3 = stringBuilder.ToString();
			RecycleStringBuilder(stringBuilder);
			return result3;
		}

		private static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
			}
			if (55296 <= utf32 && utf32 <= 57343)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
			}
			if (utf32 < 65536)
			{
				return new string((char)utf32, 1);
			}
			utf32 -= 65536;
			return new string(new char[2]
			{
				(char)((utf32 >> 10) + 55296),
				(char)(utf32 % 1024 + 56320)
			});
		}

		private static object ParseNumber(char[] json, ref int index, ref bool success)
		{
			EatWhitespace(json, ref index);
			int lastIndexOfNumber = GetLastIndexOfNumber(json, index);
			int length = lastIndexOfNumber - index + 1;
			string text = new string(json, index, length);
			object result2;
			if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
			{
				success = double.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);
				result2 = result;
			}
			else
			{
				success = long.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out long result3);
				result2 = result3;
			}
			index = lastIndexOfNumber + 1;
			return result2;
		}

		private static int GetLastIndexOfNumber(char[] json, int index)
		{
			int i;
			for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
			{
			}
			return i - 1;
		}

		private static void EatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length && " \t\n\r\b\f".IndexOf(json[index]) != -1)
			{
				index++;
			}
		}

		private static int LookAhead(char[] json, int index)
		{
			int index2 = index;
			return NextToken(json, ref index2);
		}

		private static int NextToken(char[] json, ref int index)
		{
			EatWhitespace(json, ref index);
			if (index != json.Length)
			{
				char c = json[index];
				index++;
				switch (c)
				{
				case ',':
					return 6;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return 8;
				case '[':
					return 3;
				case '{':
					return 1;
				default:
				{
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return 10;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return 9;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return 11;
					}
					return 0;
				}
				case '"':
					return 7;
				case '}':
					return 2;
				case ']':
					return 4;
				case ':':
					return 5;
				}
			}
			return 0;
		}

		private static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
		{
			bool flag = true;
			string text = value as string;
			if (text != null)
			{
				flag = SerializeString(text, builder);
			}
			else
			{
				IDictionary<string, object> dictionary = value as IDictionary<string, object>;
				if (dictionary != null)
				{
					flag = SerializeObject(jsonSerializerStrategy, dictionary.Keys, dictionary.Values, builder);
				}
				else
				{
					IDictionary<string, string> dictionary2 = value as IDictionary<string, string>;
					if (dictionary2 != null)
					{
						flag = SerializeObject(jsonSerializerStrategy, dictionary2.Keys, dictionary2.Values, builder);
					}
					else
					{
						IEnumerable enumerable = value as IEnumerable;
						if (enumerable != null)
						{
							flag = SerializeArray(jsonSerializerStrategy, enumerable, builder);
						}
						else if (IsNumeric(value))
						{
							flag = SerializeNumber(value, builder);
						}
						else if (value is bool)
						{
							builder.Append(((bool)value) ? "true" : "false");
						}
						else if (value == null)
						{
							builder.Append("null");
						}
						else
						{
							flag = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out object output);
							if (flag)
							{
								SerializeValue(jsonSerializerStrategy, output, builder);
							}
						}
					}
				}
			}
			return flag;
		}

		private static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
		{
			builder.Append("{");
			IEnumerator enumerator = keys.GetEnumerator();
			IEnumerator enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object current = enumerator.Current;
				object current2 = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				string text = current as string;
				if (text != null)
				{
					SerializeString(text, builder);
				}
				else if (!SerializeValue(jsonSerializerStrategy, current2, builder))
				{
					return false;
				}
				builder.Append(":");
				if (SerializeValue(jsonSerializerStrategy, current2, builder))
				{
					flag = false;
					continue;
				}
				return false;
			}
			builder.Append("}");
			return true;
		}

		private static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			IEnumerator enumerator = anArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (!flag)
					{
						builder.Append(",");
					}
					if (!SerializeValue(jsonSerializerStrategy, current, builder))
					{
						return false;
					}
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
			builder.Append("]");
			return true;
		}

		private static bool SerializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");
			char[] array = aString.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					break;
				case '\\':
					builder.Append("\\\\");
					break;
				case '\b':
					builder.Append("\\b");
					break;
				case '\f':
					builder.Append("\\f");
					break;
				case '\n':
					builder.Append("\\n");
					break;
				case '\r':
					builder.Append("\\r");
					break;
				case '\t':
					builder.Append("\\t");
					break;
				default:
					builder.Append(c);
					break;
				}
			}
			builder.Append("\"");
			return true;
		}

		private static bool SerializeNumber(object number, StringBuilder builder)
		{
			if (number is long)
			{
				builder.Append(((long)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is ulong)
			{
				builder.Append(((ulong)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is int)
			{
				builder.Append(((int)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is uint)
			{
				builder.Append(((uint)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is decimal)
			{
				builder.Append(((decimal)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is float)
			{
				builder.Append(((float)number).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
			}
			return true;
		}

		private static bool IsNumeric(object value)
		{
			return value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
		}

		private static StringBuilder GetStringBuilder()
		{
			StringBuilder stringBuilder = null;
			if (stringBuilders.Count <= 0)
			{
				stringBuilder = new StringBuilder(2000);
			}
			else
			{
				stringBuilder = stringBuilders.Dequeue();
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Remove(0, stringBuilder.Length);
				}
			}
			return stringBuilder;
		}

		private static void RecycleStringBuilder(StringBuilder builder)
		{
			stringBuilders.Enqueue(builder);
		}
	}
}
