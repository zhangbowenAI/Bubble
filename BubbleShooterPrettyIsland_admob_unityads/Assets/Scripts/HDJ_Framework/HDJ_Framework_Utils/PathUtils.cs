
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HDJ.Framework.Utils
{
	public class PathUtils
	{
		public static string CreatePlatformPath(string path)
		{
			string[] array = path.Split('/');
			string text = string.Empty;
			if (array.Length > 0)
			{
				text = array[0];
			}
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] != string.Empty)
				{
					text = Path.Combine(text, array[i]);
				}
			}
			return text;
		}

		public static string CutPath(string fullPath, string cutFolderName, bool returnLatterPart = true, bool includeCutFolderName = false)
		{
			fullPath = fullPath.Replace("\\", "/");
			if (!fullPath.Contains(cutFolderName))
			{
				return fullPath;
			}
			if (fullPath.Contains(cutFolderName + "/"))
			{
				if (returnLatterPart)
				{
					if (includeCutFolderName)
					{
						return cutFolderName + fullPath.Split(new string[1]
						{
							cutFolderName
						}, StringSplitOptions.None)[1];
					}
					return fullPath.Split(new string[1]
					{
						cutFolderName + "/"
					}, StringSplitOptions.None)[1];
				}
				if (includeCutFolderName)
				{
					return fullPath.Split(new string[1]
					{
						cutFolderName
					}, StringSplitOptions.None)[0] + cutFolderName;
				}
				return fullPath.Split(new string[1]
				{
					cutFolderName + "/"
				}, StringSplitOptions.None)[0];
			}
			if (returnLatterPart)
			{
				if (includeCutFolderName)
				{
					return cutFolderName + fullPath.Split(new string[1]
					{
						cutFolderName
					}, StringSplitOptions.None)[1];
				}
				return fullPath.Split(new string[1]
				{
					cutFolderName
				}, StringSplitOptions.None)[1];
			}
			if (includeCutFolderName)
			{
				return fullPath.Split(new string[1]
				{
					cutFolderName
				}, StringSplitOptions.None)[0] + cutFolderName;
			}
			return fullPath.Split(new string[1]
			{
				cutFolderName
			}, StringSplitOptions.None)[0];
		}

		public static string GetSpecialPath(string assetPath, SpecialPathType type)
		{
			string text = assetPath;
			switch (type)
			{
			case SpecialPathType.Resources:
				text = Application.dataPath + "/Resources/" + text;
				break;
			case SpecialPathType.Persistent:
				text = Application.persistentDataPath + "/" + text;
				break;
			case SpecialPathType.StreamingAssets:
				text = Application.streamingAssetsPath + "/" + text;
				break;
			}
			return text;
		}

		public static string RemoveExtension(string path)
		{
			string result = path;
			if (Path.HasExtension(path))
			{
				result = Path.ChangeExtension(path, null);
			}
			return result;
		}

		public static string[] RemovePathWithEnds(string[] paths, string[] endsWith)
		{
			if (endsWith == null && endsWith.Length == 0)
			{
				return paths;
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>(endsWith);
			for (int i = 0; i < paths.Length; i++)
			{
				string extension = Path.GetExtension(paths[i]);
				if (!list2.Contains(extension))
				{
					list.Add(paths[i]);
				}
			}
			return list.ToArray();
		}

		public static string[] GetDirectoryFilePath(string path, string[] endsWith = null, bool isIncludeChildFolder = true)
		{
			List<string> list = new List<string>();
			if (isIncludeChildFolder)
			{
				string[] directories = Directory.GetDirectories(path);
				foreach (string path2 in directories)
				{
					string[] directoryFilePath = GetDirectoryFilePath(path2, endsWith);
					list.AddRange(directoryFilePath);
				}
			}
			string[] files = Directory.GetFiles(path);
			foreach (string text in files)
			{
                //string text = text.Replace("\\", "/");
                //string extension = Path.GetExtension(text);
                //if (endsWith != null && endsWith.Length > 0)
                //{
                //	for (int k = 0; k < endsWith.Length; k++)
                //	{
                //		if (extension.Equals(endsWith[k]))
                //		{
                //			list.Add(text);
                //			break;
                //		}
                //	}
                //}
                //else
                //{
                //	list.Add(text);
                //}
                // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                string text2 = text.Replace("\\", "/");
                string extension = Path.GetExtension(text2);
                if (endsWith != null && endsWith.Length > 0) {
                    for (int k = 0; k < endsWith.Length; k++) {
                        if (extension.Equals(endsWith[k])) {
                            list.Add(text2);
                            break;
                        }
                    }
                } else {
                    list.Add(text2);
                }
            }
			return list.ToArray();
		}

		public static string[] GetDirectoryFileNames(string path, string[] endsWith, bool isNeedExtension = false, bool isIncludeChildFolder = true)
		{
			string[] directoryFilePath = GetDirectoryFilePath(path, endsWith, isIncludeChildFolder);
			List<string> list = new List<string>();
			for (int i = 0; i < directoryFilePath.Length; i++)
			{
				if (isNeedExtension)
				{
					list.Add(Path.GetFileName(directoryFilePath[i]));
				}
				else
				{
					list.Add(Path.GetFileNameWithoutExtension(directoryFilePath[i]));
				}
			}
			return list.ToArray();
		}

		public static string[] GetDirectorys(string path, bool isIncludeChildFolder = true)
		{
			List<string> list = new List<string>();
			if (Directory.Exists(path))
			{
				string[] directories = Directory.GetDirectories(path);
				list.AddRange(directories);
				if (isIncludeChildFolder)
				{
					foreach (string path2 in directories)
					{
						string[] directorys = GetDirectorys(path2);
						list.AddRange(directorys);
					}
				}
			}
			return list.ToArray();
		}

		public static string GetFileName(string path)
		{
			string result = string.Empty;
			try
			{
				result = Path.GetFileName(path);
				return result;
			}
			catch
			{
				path = path.Replace("\\", "/");
				if (!path.Contains("/"))
				{
					return path;
				}
				string[] array = path.Split(new string[1]
				{
					"/"
				}, StringSplitOptions.None);
				if (array.Length > 0)
				{
					return array[array.Length - 1];
				}
				return result;
			}
		}
	}
}
