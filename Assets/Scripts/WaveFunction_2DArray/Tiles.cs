using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tiles<T>
{
    [SerializeField] Sprite tileSprite;
    [SerializeField] List<T> rules = new List<T>();

    public Tiles(Sprite _tileSprite, List<T> _rules)
    {
        this.tileSprite = _tileSprite;
        
        foreach(T t in _rules)
        {
            rules.Add(t);
        }
    }

    public Sprite GetSprite() => this.tileSprite;
    public List<T> GetRules() => rules;
}
