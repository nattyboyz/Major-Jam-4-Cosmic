using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    [SerializeField] string key;
    [SerializeField] Color color;
    [SerializeField] Sprite icon;
    [SerializeField] string name;
    [Range(-100, 100)]
    [SerializeField] float qualityValue = 0;
    [SerializeField] List<ModifierData> modifiers;
    [Min(0)]
    [SerializeField] int buyPrice = 0;
    [Min(0)]
    [SerializeField] int sellPrice = 0;


    public Color Color { get => color; }
    public Sprite Icon { get => icon; }
    public string Name { get => name;}
    public string Key { get => key; }
    public float QualityValue { get => qualityValue;}
    public List<ModifierData> Modifiers { get => modifiers;}
    public int BuyPrice { get => buyPrice;}
    public int SellPrice { get => sellPrice;}
}
