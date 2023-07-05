using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleRandomizer;

public class Test : MonoBehaviour
{
    WeightedRandom w = new WeightedRandom();
    public List<float> testList = new List<float>()
    {
        5,5,5,5
    };

    public Dictionary<string, float> testDictionary = new Dictionary<string, float>()
    {
        {"Sampaguita", 125},
        {"Rafflesia", 25},
        {"Waling-waling", 125},
        {"Ylang-ylang", 25},

    };
    int listIndex = 0;
    float listValue = 0;

    string dictionaryKey = "";
    float dictionaryValue = 0;
    public int tries = 0;
    
    [Header("TOTAL")]
    
    public float ATotal = 0;
    public float BTotal = 0;
    public float CTotal = 0;
    public float DTotal = 0;
    public float negative1 = 0;

    [Header("CHANCES")]
    public float AChances;
    public float BChances;
    public float CChances;
    public float DChances;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TestListIndex();
        //TestListValue();
        //TestDictionaryKey();
        TestDictionaryValue();
    }

    void TestListIndex()
    {
        listIndex = w.WeightedRandomIndex(testList);
        tries++;
        
        if(listIndex == 0) ATotal++;
        else if(listIndex == 1) BTotal++;
        else if(listIndex == 2) CTotal++;
        else if(listIndex == 3) DTotal++;
        else negative1++;

        AChances = ATotal/(float)tries * 100;
        BChances = BTotal/(float)tries * 100;
        CChances = CTotal/(float)tries * 100;
        DChances = DTotal/(float)tries * 100;
    }

    void TestListValue()
    {
        listValue = w.WeightedRandomValue(testList);
        tries++;

        Debug.Log(listValue);

        if(listValue == testList[0]) ATotal++;
        else if(listValue == testList[1]) BTotal++;
        else if(listValue == testList[2]) CTotal++;
        else if(listValue == testList[3]) DTotal++;
        else negative1++;

        AChances = ATotal/(float)tries * 100;
        BChances = BTotal/(float)tries * 100;
        CChances = CTotal/(float)tries * 100;
        DChances = DTotal/(float)tries * 100;
    }

    void TestDictionaryKey()
    {
        dictionaryKey = w.WeightedRandomKey<string>(testDictionary);
        tries++;
        
        if(dictionaryKey == testDictionary.ElementAt(0).Key) ATotal++;
        else if(dictionaryKey == testDictionary.ElementAt(1).Key) BTotal++;
        else if(dictionaryKey == testDictionary.ElementAt(2).Key) CTotal++;
        else if(dictionaryKey == testDictionary.ElementAt(3).Key) DTotal++;
        else negative1++;

        AChances = ATotal/(float)tries * 100;
        BChances = BTotal/(float)tries * 100;
        CChances = CTotal/(float)tries * 100;
        DChances = DTotal/(float)tries * 100;
    }

    void TestDictionaryValue()
    {
        dictionaryValue = w.WeightedRandomValue<string>(testDictionary);
        tries++;
        
        if(dictionaryValue == testDictionary.ElementAt(0).Value) ATotal++;
        else if(dictionaryValue == testDictionary.ElementAt(1).Value) BTotal++;
        else if(dictionaryValue == testDictionary.ElementAt(2).Value) CTotal++;
        else if(dictionaryValue == testDictionary.ElementAt(3).Value) DTotal++;
        else negative1++;

        AChances = ATotal/(float)tries * 100;
        BChances = BTotal/(float)tries * 100;
        CChances = CTotal/(float)tries * 100;
        DChances = DTotal/(float)tries * 100;
    }
}
