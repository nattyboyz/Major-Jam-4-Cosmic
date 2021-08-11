using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuData", menuName = "ScriptableObjects/MenuData", order = 1)]
public class MenuData : ScriptableObject
{
    public string menuName;
    public List<IngredientData> ingredients;
    public int basePrice;
    public Sprite sprite;
}
