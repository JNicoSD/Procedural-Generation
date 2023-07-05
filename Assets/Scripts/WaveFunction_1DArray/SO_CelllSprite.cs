using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SO_CelllSprite : ScriptableObject
{
    [System.Serializable]
    public class SpriteList
    {
        [SerializeField] Sprite sprite;
        [SerializeField] int index;
    }

    public SpriteList[] spriteLists;
}
