using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CustomerType { Normal, MeatLover,Vegan,Rusher,Bypasser,DeliveryGuy,Celebrity,Inspector }

[CreateAssetMenu(fileName = "CustomerData", menuName = "ScriptableObjects/CustomerData", order = 1)]
public class CustomerData: ScriptableObject
{
    [SerializeField] CustomerType customerType;
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Color color = new Color32(232,167,11,255);
    [SerializeField] Sprite sprite;
    [SerializeField] TimeData timeModifier;

    public CustomerType CustomerType { get => customerType; }
    public string Name { get => name;}
    public string Description { get => description;}
    public Sprite Sprite { get => sprite; }
    public TimeData TimeModifier { get => timeModifier; }
}

[Serializable]
public class TimeData
{
    [SerializeField] int min;
    [SerializeField] int max;
    [SerializeField] bool waitForeaver = false;

    public int Min { get => min;}
    public int Max { get => max; }
    public bool WaitForeaver { get => waitForeaver;}

    public int GetRandomTime()
    {
        return UnityEngine.Random.Range(min, max+1);
    }
}
