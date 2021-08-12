using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerType { Normal, MeatLover,Vegan,Rusher,Bypasser,DeliveryGuy,Gentry,Inspector }

[CreateAssetMenu(fileName = "CustomerData", menuName = "ScriptableObjects/CustomerData", order = 1)]
public class CustomerData: ScriptableObject
{
    [SerializeField] CustomerType customerType;
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Color color = new Color32(232,167,11,255);
    [SerializeField] Sprite sprite;

    public CustomerType CustomerType { get => customerType; }
    public string Name { get => name;}
    public string Description { get => description;}
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
