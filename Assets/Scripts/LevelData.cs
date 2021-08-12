using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] List<MenuData> menus;
    [SerializeField] int inspector_amount = 0;
    [SerializeField] int meat_lover_amount = 0;
    [Min(1)]
    [SerializeField] int max_wave = 6;
    [SerializeField] int goalMoney = 600;
    [SerializeField] List<CustomerData> possibleCustomers;
    [SerializeField] List<RequestData> fixedCustomer;

    public List<MenuData> Menus { get => menus; set => menus = value; }
    public List<CustomerData> PossibleCustomers { get => possibleCustomers;}
    public int GoalMoney { get => goalMoney;}
    public int Max_wave { get => max_wave; }
    public int Meat_lover_amount { get => meat_lover_amount;}
    public int Inspector_amount { get => inspector_amount; }
    public List<RequestData> FixedCustomer { get => fixedCustomer;}
}
