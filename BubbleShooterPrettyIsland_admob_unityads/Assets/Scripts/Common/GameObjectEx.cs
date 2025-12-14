
using UnityEngine;

public static class GameObjectEx
{
	public static void setLayer(this GameObject self, int layer, bool includeChildren = true)
	{
		self.layer = layer;
		if (includeChildren)
		{
			Transform[] componentsInChildren = self.transform.GetComponentsInChildren<Transform>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}
	}
}
