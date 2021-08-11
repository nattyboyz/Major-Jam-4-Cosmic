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
    [SerializeField] string[] modifiers;


    public Color Color { get => color; set => color = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Name { get => name; set => name = value; }
    public string Key { get => key; set => key = value; }
}
