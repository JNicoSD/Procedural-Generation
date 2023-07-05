using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectHandler<T>
{
    GameObject obj;
    List<T> candidates;
    Tiles<int> spriteHandler;
    bool isFilled;
    public ObjectHandler(string _name, int _x, int _y, List<T> _candidates)
    {
        if(this.candidates == null)
        {
            this.candidates = new List<T>();
        }

        this.obj = new GameObject(_name);

        int i = 0;
        //not sure if needs to .Clear()
        foreach(T t in _candidates)
        {
            candidates.Add(t);
            i++;
        }
        this.isFilled = false;
    }
    public GameObject GetObj() => this.obj;
    public void AssignSprite(Tiles<int> _s) 
    {
        this.spriteHandler = _s;
        this.obj.GetComponent<SpriteRenderer>().sprite = this.spriteHandler.GetSprite();
        this.isFilled = true;
        this.candidates.Clear();
    }
    public Tiles<int> GetSpriteHandler() => this.spriteHandler;
    public List<T> GetCandidates() => this.candidates;
    public void RemoveCandidates(List<T> removed) => this.candidates = this.candidates.Except(removed).ToList();
    public void UpdateCandidates(List<T> updated) 
    {
        this.candidates = this.candidates.Intersect(updated).ToList();
        this.candidates.Sort();
    }
    public bool IsFilled() => this.isFilled;
}
