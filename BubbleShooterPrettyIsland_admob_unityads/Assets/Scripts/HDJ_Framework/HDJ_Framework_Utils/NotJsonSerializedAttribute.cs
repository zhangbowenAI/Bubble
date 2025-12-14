
using System;

namespace HDJ.Framework.Utils
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NotJsonSerializedAttribute : Attribute
	{
	}
}
