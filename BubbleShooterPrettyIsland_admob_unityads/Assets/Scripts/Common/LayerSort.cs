
using UnityEngine;

public class LayerSort : MonoBehaviour
{
    public int order;

    private void Start()
    {
        Canvas component = GetComponent<Canvas>();
        if (component != null)
        {
            component.overrideSorting = true;
            component.sortingOrder = order;
            return;
        }
        Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
        Renderer[] array = componentsInChildren;
        foreach (Renderer renderer in array)
        {
            renderer.sortingOrder = order;
        }
    }
}
