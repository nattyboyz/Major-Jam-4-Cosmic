using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public List<MenuData> menus;
    public int inspector_amount = 0;
    public int meat_lover_amount = 0;
    [Min(1)]
    public int max_wave = 6;
}
