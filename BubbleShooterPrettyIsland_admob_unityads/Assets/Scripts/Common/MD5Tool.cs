
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class MD5Tool
{
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
                return GetMD5(data);
            }
            return string.Empty;
        }
        catch (FileNotFoundException ex)
        {
            UnityEngine.Debug.Log(ex.Message);
            return string.Empty;
        }
    }

    public static string GetObjectMD5(object obj)
    {
        if (obj == null)
        {
            UnityEngine.Debug.LogError("obj is Null !");
            return string.Empty;
        }
        return GetMD5(ByteTool.Object2Bytes(obj));
    }

    public static int GetStringToHash(string content)
    {
        return GetHashMD5(Encoding.Default.GetBytes(content));
    }

    public static string GetMD5(byte[] data)
    {
        MD5 mD = new MD5CryptoServiceProvider();
        byte[] array = mD.ComputeHash(data);
        string text = string.Empty;
        byte[] array2 = array;
        foreach (byte value in array2)
        {
            text += Convert.ToString(value, 16);
        }
        if (!string.IsNullOrEmpty(text))
        {
            return text;
        }
        return string.Empty;
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

    public static int ToHash(this string content)
    {
        return GetStringToHash(content);
    }
}
