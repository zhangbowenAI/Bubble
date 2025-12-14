
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ByteTool
{
	public static byte[] Object2Bytes(object obj)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(memoryStream, obj);
			return memoryStream.GetBuffer();
		}
	}
}
