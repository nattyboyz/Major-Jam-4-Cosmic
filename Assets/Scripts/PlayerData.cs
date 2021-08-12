using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int money = 0;
    public int star = 3;
    public int maxStar = 5;
    public Dictionary<IngredientData, int> ingredients = new Dictionary<IngredientData, int>();
    public Dictionary<IngredientData, int> cheat = new Dictionary<IngredientData, int>();

}
