using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringCheatDataDict : SerializableDictionary<string, IngredientData> { }

[CreateAssetMenu(fileName = "CheatDatabase", menuName = "ScriptableObjects/CheatDatabase", order = 1)]
public class CheatDatabase : ScriptableObject
{
   [SerializeField] StringCheatDataDict database;

    public StringCheatDataDict Database { get => database; }
}
