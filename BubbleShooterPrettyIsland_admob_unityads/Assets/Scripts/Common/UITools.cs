
using UnityEngine;

public class UITools : MonoBehaviour
{
    public static GameObject CreateUiItem(string l_gameObjectName, GameObject l_parent = null)
    {
        GameObject gameObject = (GameObject)ResourceManager.Load(l_gameObjectName);
        if (gameObject == null)
        {
            UnityEngine.Debug.LogError("CreateItem error dont find :" + l_gameObjectName);
            return null;
        }
        GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
        gameObject2.name = l_gameObjectName;
        if (l_parent != null)
        {
            gameObject2.transform.SetParent(l_parent.transform);
            gameObject2.transform.localScale = Vector3.one;
        }
        return gameObject2;
    }
}
