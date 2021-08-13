using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IngredientDataAmountDict : SerializableDictionary<IngredientData, int> { }

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
        ingredients = dataObject.ingredients;
        cheats = dataObject.cheats;
    }

}
