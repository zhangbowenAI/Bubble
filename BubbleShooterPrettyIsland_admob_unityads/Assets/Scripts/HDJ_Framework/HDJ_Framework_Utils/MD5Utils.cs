
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace HDJ.Framework.Utils
{
	public static class MD5Utils
	{
		public static string GetObjectMD5(object obj)
		{
			if (obj == null)
			{
				UnityEngine.Debug.LogError("obj is Null !");
				return string.Empty;
			}
			return GetMD5(Object2Bytes(obj));
		}

		public static int GetObjectMD5Hash(object obj)
		{
			return GetHashMD5(Object2Bytes(obj));
		}

		public static byte[] Object2Bytes(object obj)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(memoryStream, obj);
				return memoryStream.GetBuffer();
			}
		}

		public static int GetStringToHash(string content)
		{
			return GetHashMD5(Encoding.UTF8.GetBytes(content));
		}

		public static string GetMD5(byte[] data)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(data);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			foreach (byte value in array2)
			{
				stringBuilder.Append(Convert.ToString(value, 16));
			}
			return stringBuilder.ToString();
		}

		public static int GetHashMD5(byte[] data)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(data);
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				num += Convert.ToInt32(array[i]) + Convert.ToInt32(array[i + 1]) + Convert.ToInt32(array[i + 2]) + Convert.ToInt32(array[i]) << i * 8;
			}
			return num;
		}
	}
}
