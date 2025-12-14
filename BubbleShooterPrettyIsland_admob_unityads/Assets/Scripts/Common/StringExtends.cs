
using System.Collections.Generic;

public static class StringExtends
{
	public static string[] SplitExtend(this string value, string startSign, string endSign)
	{
		List<string> list = new List<string>();
		string text = value;
		for (int num = text.IndexOf(startSign); num != -1; num = text.IndexOf(startSign))
		{
			int num2 = num + startSign.Length;
			int num3 = text.IndexOf(endSign, num2);
			if (num3 == -1)
			{
				break;
			}
			list.Add(text.Substring(num2, num3 - num2));
			text = text.Remove(0, num3 + endSign.Length);
		}
		return list.ToArray();
	}
}
