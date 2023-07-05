using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellSprite", menuName = "WaveFunctionCollapse/Tileset/CellSprite", order = 1)]
public class CellSprite :  ScriptableObject
{
    [System.Serializable]
    public class SpriteList
    {
        [SerializeField] Sprite sprite;
        public Sprite GetSprite { get{ return sprite; } }

        [SerializeField] List<string> spriteRules;
        public List<string> GetSpriteRules { get{ return spriteRules; } }
    }

    public SpriteList[] spriteLists;
}
