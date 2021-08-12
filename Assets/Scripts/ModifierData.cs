using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType {None, Curse, Buff}
[CreateAssetMenu(fileName = "ModifierData", menuName = "ScriptableObjects/ModifierData", order = 1)]
public class ModifierData : ScriptableObject
{
    [SerializeField] string key;
    [SerializeField] string modifierName;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite sprite;

    [Range(-100, 100)]
    [SerializeField] float qualityValue = 0;
    [SerializeField] ModifierType type;

    public ModifierType Type { get => type; }
    public float QualityValue { get => qualityValue;}
    public string Key { get => key;}
    public string ModifierName { get => modifierName; }
    public string Description { get => description;}
    public Sprite Sprite { get => sprite;}
}
