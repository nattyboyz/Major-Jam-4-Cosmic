using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType {None, Curse, Buff}
[CreateAssetMenu(fileName = "ModifierData", menuName = "ScriptableObjects/ModifierData", order = 1)]
public class ModifierData : ScriptableObject
{
    public string key;
    public string modifierName;
    [TextArea]
    public string description;
    public Sprite sprite;
    [Range(-100,100)]
    public float modifier = 0;
    public ModifierType type;

}
