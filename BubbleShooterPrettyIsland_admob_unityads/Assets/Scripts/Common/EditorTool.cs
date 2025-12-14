
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EditorTool : MonoBehaviour
{
	public static Type GetType(string typeName)
	{
		return Type.GetType(typeName);
	}

	public static Type[] GetTypes()
	{
		return Assembly.Load("Assembly-CSharp").GetTypes();
	}

	public static string[] GetAllEnumType()
	{
		List<string> list = new List<string>();
		Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			if (types[i].IsSubclassOf(typeof(Enum)) && GetType(types[i].Name) != null)
			{
				list.Add(types[i].Name);
			}
		}
		return list.ToArray();
	}

	public static int GetAllEnumTypeIndex(string typeName)
	{
		string[] allEnumType = GetAllEnumType();
		for (int i = 0; i < allEnumType.Length; i++)
		{
			if (typeName == allEnumType[i])
			{
				return i;
			}
		}
		return -1;
	}
}
