using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerDataScriptableObject : ScriptableObject
{
    public int money = 200;
    public int star = 3;
    public int maxStar = 5;
    public IngredientDataAmountDict ingredients = new IngredientDataAmountDict();
    public IngredientDataAmountDict cheats = new IngredientDataAmountDict();
}
