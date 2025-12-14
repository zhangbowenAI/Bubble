using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoSingletonBase<AtlasManager>
{

    public SpriteAtlas mapAtlas;
    public SpriteAtlas carAltas;

    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
        mapAtlas = Resources.Load<SpriteAtlas>("Atlas/AtlasMapimage");
    }

    public Sprite GetSprite(AtlasType atlasType, string spriteName)
    {
        Sprite sprite;
        switch (atlasType)
        {
            case AtlasType.MapAtlas:
                sprite = mapAtlas.GetSprite(spriteName);
                break;

            default:
                sprite = null;
                break;
        }
        if (sprite != null)
        {
            return sprite;
        }
        Debug.Log(string.Format("图集中没有 {0} 这个图片", spriteName));
        return null;
    }
}

public enum AtlasType
{
    MapAtlas,
}

