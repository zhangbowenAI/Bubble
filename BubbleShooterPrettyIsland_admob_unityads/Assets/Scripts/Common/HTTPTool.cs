
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class HTTPTool
{
	public class UpLoadThread
	{
		public string url;

		public string[] files;

		public NameValueCollection data;

		public Encoding encoding;

		public CallBack<string> callBack;

		public void Upload_Request()
		{
			UnityEngine.Debug.Log("Upload_Request " + url);
			try
			{
				string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
				byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
				byte[] bytes2 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
				httpWebRequest.Method = "POST";
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
				httpWebRequest.Expect = null;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
					if (data != null)
					{
						IEnumerator enumerator = data.Keys.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								string text = (string)enumerator.Current;
								stream.Write(bytes, 0, bytes.Length);
								string s = string.Format(format, text, data[text]);
								byte[] bytes3 = encoding.GetBytes(s);
								stream.Write(bytes3, 0, bytes3.Length);
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
					}
					string format2 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: text/plain\r\n\r\n";
					byte[] array = new byte[4096];
					int num = 0;
					for (int i = 0; i < files.Length; i++)
					{
						stream.Write(bytes, 0, bytes.Length);
						string s2 = string.Format(format2, "file", Path.GetFileName(files[i]));
						byte[] bytes4 = encoding.GetBytes(s2);
						stream.Write(bytes4, 0, bytes4.Length);
						using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
						{
							while ((num = fileStream.Read(array, 0, array.Length)) != 0)
							{
								stream.Write(array, 0, num);
							}
						}
					}
					stream.Write(bytes2, 0, bytes2.Length);
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
				{
					string text2 = streamReader.ReadToEnd();
					if (text2 == "ok")
					{
						UnityEngine.Debug.Log(files[0] + " upload completed " + text2);
						if (callBack != null)
						{
							callBack(files[0] + "upload completed " + text2);
						}
					}
					else
					{
						UnityEngine.Debug.Log(files[0] + "upload failed " + text2);
						if (callBack != null)
						{
							callBack(files[0] + "upload failed " + text2);
						}
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(files[0] + "upload failed \n" + ex.ToString());
				if (callBack != null)
				{
					callBack(files[0] + "upload failed \n" + ex.ToString());
				}
			}
		}
	}

	private static readonly Encoding DEFAULTENCODE = Encoding.UTF8;

	public static void Upload_Request_Thread(string url, string file, CallBack<string> callBack = null)
	{
		NameValueCollection data = new NameValueCollection();
		UpLoadThread upLoadThread = new UpLoadThread();
		upLoadThread.url = url;
		upLoadThread.files = new string[1]
		{
			file
		};
		upLoadThread.data = data;
		upLoadThread.encoding = DEFAULTENCODE;
		upLoadThread.callBack = callBack;
		Thread thread = new Thread(upLoadThread.Upload_Request);
		thread.Start();
	}
}
