using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IngredientDataAmountDict : SerializableDictionary<IngredientData, int> { }

[Serializable]
public class PlayerData
{
    public int money = 0;
    public int star = 3;
    public int maxStar = 5;
    public IngredientDataAmountDict ingredients = new IngredientDataAmountDict();
    public IngredientDataAmountDict cheats = new IngredientDataAmountDict();

    public PlayerData()
    {

    }

    public PlayerData(PlayerDataScriptableObject dataObject)
    {
        money = dataObject.money;
        star = dataObject.star;
        maxStar = dataObject.maxStar;

        foreach(var kvp in dataObject.ingredients)
        {
            ingredients.Add(kvp.Key, kvp.Value);
        }
        foreach (var kvp in dataObject.cheats)
        {
            cheats.Add(kvp.Key, kvp.Value);
        }
    }

}
