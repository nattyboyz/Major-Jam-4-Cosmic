using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum CustomerType { Normal, MeatLover,Vegan,Rusher,Bypasser,DeliveryGuy,Celebrity,Inspector }
[CreateAssetMenu(fileName = "CustomerType", menuName = "ScriptableObjects/CustomerType", order = 1)]
public class CustomerType : ScriptableObject
{
    [SerializeField] string name ="";
    [SerializeField] Color color = new Color32(255,255,255,255);
    [SerializeField] Sprite icon;
    [TextArea]
    [SerializeField] string description;

    public string Name { get => name;}
    public Color Color { get => color;}
    public Sprite Icon { get => icon; }
    public string Description { get => description;}
}
