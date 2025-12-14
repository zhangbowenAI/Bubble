
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileTool
{
	public static void CreatFilePath(string filepath)
	{
		string directoryName = Path.GetDirectoryName(filepath);
		CreatPath(directoryName);
	}

	public static void CreatPath(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	public static void DeleteDirectory(string path)
	{
		string[] directories = Directory.GetDirectories(path);
		foreach (string path2 in directories)
		{
			if (Directory.Exists(path2))
			{
				Directory.Delete(path2, recursive: true);
			}
		}
		string[] files = Directory.GetFiles(path);
		foreach (string path3 in files)
		{
			if (File.Exists(path3))
			{
				File.Delete(path3);
			}
		}
	}

	public static void CopyDirectory(string sourcePath, string destinationPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
		Directory.CreateDirectory(destinationPath);
		FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
		foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
		{
			string text = Path.Combine(destinationPath, fileSystemInfo.Name);
			if (fileSystemInfo is FileInfo)
			{
				File.Copy(fileSystemInfo.FullName, text);
				continue;
			}
			Directory.CreateDirectory(text);
			CopyDirectory(fileSystemInfo.FullName, text);
		}
	}

	public static void SafeDeleteDirectory(string path)
	{
		string[] directories = Directory.GetDirectories(path);
		foreach (string path2 in directories)
		{
			if (Directory.Exists(path2))
			{
				SafeDeleteDirectory(path2);
				try
				{
					Directory.Delete(path2, recursive: false);
				}
				catch
				{
				}
			}
		}
		string[] files = Directory.GetFiles(path);
		foreach (string path3 in files)
		{
			if (File.Exists(path3))
			{
				try
				{
					File.Delete(path3);
				}
				catch
				{
				}
			}
		}
	}

	public static void SafeCopyDirectory(string sourcePath, string destinationPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
		Directory.CreateDirectory(destinationPath);
		FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
		foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
		{
			string text = Path.Combine(destinationPath, fileSystemInfo.Name);
			if (fileSystemInfo is FileInfo)
			{
				try
				{
					File.Copy(fileSystemInfo.FullName, text);
				}
				catch
				{
				}
				continue;
			}
			Directory.CreateDirectory(text);
			SafeCopyDirectory(fileSystemInfo.FullName, text);
		}
	}

	public static string RemoveExpandName(string name)
	{
		int num = name.LastIndexOf(".");
		if (num != -1)
		{
			return name.Remove(num);
		}
		return name;
	}

	public static string GetExpandName(string name)
	{
		return name.Substring(name.LastIndexOf(".") + 1, name.Length - name.LastIndexOf(".") - 1);
	}

	public static string GetFileNameByPath(string path)
	{
		FileInfo fileInfo = new FileInfo(path);
		return fileInfo.Name;
	}

	public static string GetFileNameBySring(string path)
	{
		string[] array = path.Split('/');
		return array[array.Length - 1];
	}

	public static void ChangeFileName(string path, string newName)
	{
		if (File.Exists(path))
		{
			File.Move(path, newName);
		}
	}

	public static void ConvertFileEncoding(string sourceFile, string destFile, Encoding targetEncoding)
	{
		destFile = ((!string.IsNullOrEmpty(destFile)) ? destFile : sourceFile);
		Encoding encodingType = GetEncodingType(sourceFile);
		File.WriteAllText(destFile, File.ReadAllText(sourceFile, encodingType), targetEncoding);
	}

	public static Encoding GetEncodingType(string FILE_NAME)
	{
		FileStream fileStream = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
		Encoding encodingType = GetEncodingType(fileStream);
		fileStream.Close();
		return encodingType;
	}

	public static Encoding GetEncodingType(FileStream fs)
	{
		Encoding result = Encoding.Default;
		BinaryReader binaryReader = new BinaryReader(fs, Encoding.Default);
		int.TryParse(fs.Length.ToString(), out int result2);
		byte[] array = binaryReader.ReadBytes(result2);
		if (IsUTF8Bytes(array) || (array[0] == 239 && array[1] == 187 && array[2] == 191))
		{
			result = Encoding.UTF8;
		}
		else if (array[0] == 254 && array[1] == byte.MaxValue && array[2] == 0)
		{
			result = Encoding.BigEndianUnicode;
		}
		else if (array[0] == byte.MaxValue && array[1] == 254 && array[2] == 65)
		{
			result = Encoding.Unicode;
		}
		binaryReader.Close();
		return result;
	}

	private static bool IsUTF8Bytes(byte[] data)
	{
		int num = 1;
		for (int i = 0; i < data.Length; i++)
		{
			byte b = data[i];
			if (num == 1)
			{
				if (b >= 128)
				{
					while (((b = (byte)(b << 1)) & 0x80) != 0)
					{
						num++;
					}
					if (num == 1 || num > 6)
					{
						return false;
					}
				}
			}
			else
			{
				if ((b & 0xC0) != 128)
				{
					return false;
				}
				num--;
			}
		}
		if (num > 1)
		{
			throw new Exception("Unexpected byte format");
		}
		return true;
	}

	public static void RecursionFileExecute(string path, string expandName, FileExecuteHandle handle)
	{
		string[] files = Directory.GetFiles(path);
		string[] array = files;
		foreach (string text in array)
		{
			try
			{
				if (expandName != null)
				{
					if (text.EndsWith("." + expandName))
					{
						handle(text);
					}
				}
				else
				{
					handle(text);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("RecursionFileExecute Error :" + text + " Exception:" + ex.ToString());
			}
		}
		string[] directories = Directory.GetDirectories(path);
		for (int j = 0; j < directories.Length; j++)
		{
			RecursionFileExecute(directories[j], expandName, handle);
		}
	}

	public static List<string> GetAllFileNamesByPath(string path, string[] expandNames = null)
	{
		List<string> list = new List<string>();
		RecursionFind(list, path, expandNames);
		return list;
	}

	private static void RecursionFind(List<string> list, string path, string[] expandNames)
	{
		string[] files = Directory.GetFiles(path);
		string[] array = files;
		foreach (string text in array)
		{
			if (ExpandFilter(text, expandNames))
			{
				list.Add(text);
			}
		}
		string[] directories = Directory.GetDirectories(path);
		for (int j = 0; j < directories.Length; j++)
		{
			RecursionFind(list, directories[j], expandNames);
		}
	}

	private static bool ExpandFilter(string name, string[] expandNames)
	{
		if (expandNames == null)
		{
			return true;
		}
		if (expandNames.Length == 0)
		{
			return !name.Contains(".");
		}
		for (int i = 0; i < expandNames.Length; i++)
		{
			if (name.EndsWith("." + expandNames[i]))
			{
				return true;
			}
		}
		return false;
	}
}
