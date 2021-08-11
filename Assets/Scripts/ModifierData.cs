using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifierData", menuName = "ScriptableObjects/ModifierData", order = 1)]
public class ModifierData : ScriptableObject
{
    public string key;
    public string modifierName;
    [TextArea]
    public string description;
    public Sprite sprite;
}
