
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace HDJ.Framework.Tools
{
	[GeneratedCode("simple-json", "1.0.0")]
	public class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		internal IDictionary<Type, ReflectionsUtils.ConstructorDelegate> ConstructorCache;

		internal IDictionary<Type, IDictionary<string, ReflectionsUtils.GetDelegate>> GetCache;

		internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>>> SetCache;

		internal static readonly Type[] EmptyTypes = new Type[0];

		internal static readonly Type[] ArrayConstructorParameterTypes = new Type[1]
		{
			typeof(int)
		};

		private static readonly string[] Iso8601Format = new string[3]
		{
			"yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z",
			"yyyy-MM-dd\\THH:mm:ss\\Z",
			"yyyy-MM-dd\\THH:mm:ssK"
		};

		public PocoJsonSerializerStrategy()
		{
			ConstructorCache = new ReflectionsUtils.ThreadSafeDictionary<Type, ReflectionsUtils.ConstructorDelegate>(ContructorDelegateFactory);
			GetCache = new ReflectionsUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionsUtils.GetDelegate>>(GetterValueFactory);
			SetCache = new ReflectionsUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>>>(SetterValueFactory);
		}

		protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
		{
			return clrPropertyName;
		}

		internal virtual ReflectionsUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
		{
			return ReflectionsUtils.GetContructor(key, key.IsArray ? ArrayConstructorParameterTypes : EmptyTypes);
		}

		internal virtual IDictionary<string, ReflectionsUtils.GetDelegate> GetterValueFactory(Type type)
		{
			IDictionary<string, ReflectionsUtils.GetDelegate> dictionary = new Dictionary<string, ReflectionsUtils.GetDelegate>();
			foreach (PropertyInfo property in ReflectionsUtils.GetProperties(type))
			{
				if (property.CanRead)
				{
					MethodInfo getterMethodInfo = ReflectionsUtils.GetGetterMethodInfo(property);
					if (!getterMethodInfo.IsStatic && getterMethodInfo.IsPublic)
					{
						dictionary[MapClrMemberNameToJsonFieldName(property.Name)] = ReflectionsUtils.GetGetMethod(property);
					}
				}
			}
			foreach (FieldInfo field in ReflectionsUtils.GetFields(type))
			{
				if (!field.IsStatic && field.IsPublic)
				{
					dictionary[MapClrMemberNameToJsonFieldName(field.Name)] = ReflectionsUtils.GetGetMethod(field);
				}
			}
			return dictionary;
		}

		internal virtual IDictionary<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>> SetterValueFactory(Type type)
		{
			IDictionary<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>> dictionary = new Dictionary<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>>();
			foreach (PropertyInfo property in ReflectionsUtils.GetProperties(type))
			{
				if (property.CanWrite)
				{
					MethodInfo setterMethodInfo = ReflectionsUtils.GetSetterMethodInfo(property);
					if (!setterMethodInfo.IsStatic && setterMethodInfo.IsPublic)
					{
						dictionary[MapClrMemberNameToJsonFieldName(property.Name)] = new KeyValuePair<Type, ReflectionsUtils.SetDelegate>(property.PropertyType, ReflectionsUtils.GetSetMethod(property));
					}
				}
			}
			foreach (FieldInfo field in ReflectionsUtils.GetFields(type))
			{
				if (!field.IsInitOnly && !field.IsStatic && field.IsPublic)
				{
					dictionary[MapClrMemberNameToJsonFieldName(field.Name)] = new KeyValuePair<Type, ReflectionsUtils.SetDelegate>(field.FieldType, ReflectionsUtils.GetSetMethod(field));
				}
			}
			return dictionary;
		}

		public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
		{
			return TrySerializeKnownTypes(input, out output) || TrySerializeUnknownTypes(input, out output);
		}

		public virtual object DeserializeObject(object value, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = value as string;
			if (type == typeof(Guid) && string.IsNullOrEmpty(text))
			{
				return default(Guid);
			}
			if (value == null)
			{
				return null;
			}
			object obj = null;
			if (text != null)
			{
				if (text.Length != 0)
				{
					if (type == typeof(DateTime) || (ReflectionsUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime)))
					{
						return DateTime.ParseExact(text, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(DateTimeOffset) || (ReflectionsUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset)))
					{
						return DateTimeOffset.ParseExact(text, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(Guid) || (ReflectionsUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid)))
					{
						return new Guid(text);
					}
					return text;
				}
				obj = ((type == typeof(Guid)) ? ((object)default(Guid)) : ((!ReflectionsUtils.IsNullableType(type) || Nullable.GetUnderlyingType(type) != typeof(Guid)) ? text : null));
				if (!ReflectionsUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
				{
					return text;
				}
			}
			else if (value is bool)
			{
				return value;
			}
			bool flag = value is long;
			bool flag2 = value is double;
			if ((flag && type == typeof(long)) || (flag2 && type == typeof(double)))
			{
				return value;
			}
			if ((flag2 && type != typeof(double)) || (flag && type != typeof(long)))
			{
				obj = (typeof(IConvertible).IsAssignableFrom(type) ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : value);
				if (ReflectionsUtils.IsNullableType(type))
				{
					return ReflectionsUtils.ToNullableType(obj, type);
				}
				return obj;
			}
			IDictionary<string, object> dictionary = value as IDictionary<string, object>;
			if (dictionary != null)
			{
				IDictionary<string, object> dictionary2 = dictionary;
				if (ReflectionsUtils.IsTypeDictionary(type))
				{
					Type[] genericTypeArguments = ReflectionsUtils.GetGenericTypeArguments(type);
					Type type2 = genericTypeArguments[0];
					Type type3 = genericTypeArguments[1];
					Type key = typeof(Dictionary<, >).MakeGenericType(type2, type3);
					IDictionary dictionary3 = (IDictionary)ConstructorCache[key](null);
					foreach (KeyValuePair<string, object> item in dictionary2)
					{
						dictionary3.Add(item.Key, DeserializeObject(item.Value, type3));
					}
					obj = dictionary3;
				}
				else if (type == typeof(object))
				{
					obj = value;
				}
				else
				{
					obj = ConstructorCache[type](null);
					foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionsUtils.SetDelegate>> item2 in SetCache[type])
					{
						if (dictionary2.TryGetValue(item2.Key, out object value2))
						{
							value2 = DeserializeObject(value2, item2.Value.Key);
							item2.Value.Value(obj, value2);
						}
					}
				}
			}
			else
			{
				IList<object> list = value as IList<object>;
				if (list != null)
				{
					IList<object> list2 = list;
					IList list3 = null;
					if (type.IsArray)
					{
						list3 = (IList)ConstructorCache[type](list2.Count);
						int num = 0;
						foreach (object item3 in list2)
						{
							list3[num++] = DeserializeObject(item3, type.GetElementType());
						}
					}
					else if (ReflectionsUtils.IsTypeGenericeCollectionInterface(type) || ReflectionsUtils.IsAssignableFrom(typeof(IList), type))
					{
						Type type4 = ReflectionsUtils.GetGenericTypeArguments(type)[0];
						Type key2 = typeof(List<>).MakeGenericType(type4);
						list3 = (IList)ConstructorCache[key2](list2.Count);
						foreach (object item4 in list2)
						{
							list3.Add(DeserializeObject(item4, type4));
						}
					}
					obj = list3;
				}
			}
			return obj;
		}

		protected virtual object SerializeEnum(Enum p)
		{
			return Convert.ToString(p, CultureInfo.InvariantCulture);
		}

		protected virtual bool TrySerializeKnownTypes(object input, out object output)
		{
			bool result = true;
			if (input is DateTime)
			{
				output = ((DateTime)input).ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is DateTimeOffset)
			{
				output = ((DateTimeOffset)input).ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is Guid)
			{
				output = ((Guid)input).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else
			{
				Enum @enum = input as Enum;
				if (@enum != null)
				{
					output = SerializeEnum(@enum);
				}
				else
				{
					result = false;
					output = null;
				}
			}
			return result;
		}

		protected virtual bool TrySerializeUnknownTypes(object input, out object output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			output = null;
			Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			IDictionary<string, object> dictionary = new JsonObject();
			IDictionary<string, ReflectionsUtils.GetDelegate> dictionary2 = GetCache[type];
			foreach (KeyValuePair<string, ReflectionsUtils.GetDelegate> item in dictionary2)
			{
				if (item.Value != null)
				{
					dictionary.Add(MapClrMemberNameToJsonFieldName(item.Key), item.Value(input));
				}
			}
			output = dictionary;
			return true;
		}
	}
}
