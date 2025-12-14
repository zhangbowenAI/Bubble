
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace HDJ.Framework.Utils
{
	public class FileUtils
	{
		public static void CopyDirectory(string oldDirectory, string newDirectory, bool overwrite = true)
		{
			string[] directoryFilePath = PathUtils.GetDirectoryFilePath(oldDirectory);
			for (int i = 0; i < directoryFilePath.Length; i++)
			{
				string text = directoryFilePath[i].Replace(oldDirectory, newDirectory);
				string directoryName = Path.GetDirectoryName(text);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.Copy(directoryFilePath[i], text, overwrite);
			}
		}

		public static void MoveFile(string oldFilePath, string newFilePath, bool overwrite = true)
		{
			if (File.Exists(oldFilePath) && !(oldFilePath == newFilePath))
			{
				string directoryName = Path.GetDirectoryName(newFilePath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.Copy(oldFilePath, newFilePath, overwrite);
				DeleteFile(oldFilePath);
			}
		}

		public static string LoadTextFileByPath(string path)
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.Log("path dont exists ! : " + path);
				return string.Empty;
			}
			StreamReader streamReader = File.OpenText(path);
			StringBuilder stringBuilder = new StringBuilder();
			string empty = string.Empty;
			while ((empty = streamReader.ReadLine()) != null)
			{
				stringBuilder.Append(empty);
			}
			streamReader.Close();
			streamReader.Dispose();
			return stringBuilder.ToString();
		}

		public static byte[] LoadByteFileByPath(string path)
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.Log("path dont exists ! : " + path);
				return null;
			}
			FileStream fileStream = new FileStream(path, FileMode.Open);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			return array;
		}

		public static string[] LoadTextFileLineByPath(string path)
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.Log("path dont exists ! : " + path);
				return null;
			}
			StreamReader streamReader = File.OpenText(path);
			List<string> list = new List<string>();
			string empty = string.Empty;
			while ((empty = streamReader.ReadLine()) != null)
			{
				list.Add(empty);
			}
			streamReader.Close();
			streamReader.Dispose();
			return list.ToArray();
		}

		public static bool DeleteFile(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			try
			{
				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
				UnityEngine.Debug.Log("File Delete: " + path);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("File delete fail: " + path + "  ---:" + ex);
				return false;
			}
			return true;
		}

		public static bool CreateTextFile(string path, string _data)
		{
			byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(_data);
			return CreateFile(path, bytes);
		}

		public static bool CreateFile(string path, byte[] _data)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
				fileStream.Write(_data, 0, _data.Length);
				fileStream.Close();
				UnityEngine.Debug.Log("File written: " + path);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("File written fail: " + path + "  ---:" + ex);
				return false;
			}
			return true;
		}

		public static string GetFileMD5(string filePath)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filePath);
				if (fileInfo.Exists)
				{
					FileStream fileStream = new FileStream(filePath, FileMode.Open);
					int num = (int)fileStream.Length;
					byte[] data = new byte[num];
					fileStream.Close();
					return MD5Utils.GetMD5(data);
				}
				return string.Empty;
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
				return string.Empty;
			}
		}

		public static IEnumerator LoadTxtFileIEnumerator(string path, CallBack<string> callback)
		{
			WWW www = new WWW(path);
			yield return www;
			string data = string.Empty;
			if (string.IsNullOrEmpty(www.error))
			{
				data = www.text;
			}
			callback?.Invoke(data);
			yield return new WaitForEndOfFrame();
		}
	}
}
