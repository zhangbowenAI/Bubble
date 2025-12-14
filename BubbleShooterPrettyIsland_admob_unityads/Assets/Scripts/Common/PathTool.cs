
using System.Text;
using UnityEngine;

public class PathTool
{
    public static string GetPath(ResLoadLocation loadType)
    {
        StringBuilder stringBuilder = new StringBuilder();
        switch (loadType)
        {
            case ResLoadLocation.Resource:
            case ResLoadLocation.Streaming:
                stringBuilder.Append(Application.dataPath);
                stringBuilder.Append("!assets/");
                break;
            case ResLoadLocation.Persistent:
                stringBuilder.Append(Application.persistentDataPath);
                stringBuilder.Append("/");
                break;
            case ResLoadLocation.Catch:
                stringBuilder.Append(Application.temporaryCachePath);
                stringBuilder.Append("/");
                break;
            default:
                UnityEngine.Debug.LogError("Type Error !" + loadType);
                break;
        }
        return stringBuilder.ToString();
    }

    public static string GetAbsolutePath(ResLoadLocation loadType, string relativelyPath)
    {
        return GetPath(loadType) + relativelyPath;
    }

    public static string GetRelativelyPath(string path, string fileName, string expandName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(path);
        stringBuilder.Append("/");
        stringBuilder.Append(fileName);
        stringBuilder.Append(".");
        stringBuilder.Append(expandName);
        return stringBuilder.ToString();
    }

    public static string GetDirectoryRelativePath(string DirectoryPath, string FullPath)
    {
        DirectoryPath = DirectoryPath.Replace("\\", "/");
        FullPath = FullPath.Replace("\\", "/");
        FullPath = FullPath.Replace(DirectoryPath, string.Empty);
        return FullPath;
    }
    
    public static string GetEditorPath(string directoryName, string fileName, string expandName)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Application.dataPath);
        stringBuilder.Append("/Editor");
        stringBuilder.Append(directoryName);
        stringBuilder.Append("/");
        stringBuilder.Append(fileName);
        stringBuilder.Append(".");
        stringBuilder.Append(expandName);
        return stringBuilder.ToString();
    }
}
