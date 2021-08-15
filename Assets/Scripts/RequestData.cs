using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RequestData 
{

    [SerializeField] CustomerData customerData;
    [SerializeField] MenuData menu;
    [Tooltip("in second")]
    [SerializeField] float time = 60;
    [SerializeField] int penalty = 1;
    [SerializeField] int price = 0;
    [SerializeField] List<IngredientData> ingredients = new List<IngredientData>();
    [SerializeField] List<IngredientData> extra_ingredients = new List<IngredientData>();
    [SerializeField] bool showCustomerType;

    [SerializeField] int starReward = 0;
    [SerializeField] int tipReward = 0;


    public CustomerData CustomerData { get => customerData; set => customerData = value; }
    public MenuData Menu { get => menu; set => menu = value; }
    public float Time { get => time; set => time = value; }
    public int Penalty { get => penalty; set => penalty = value; }
    public int Price { get => price; set => price = value; }
    public List<IngredientData> Extra_ingredients { get => extra_ingredients; set => extra_ingredients = value; }
    public bool ShowCustomerType { get => showCustomerType; set => showCustomerType = value; }
    public List<IngredientData> Ingredients { get => ingredients; set => ingredients = value; }
    public int StarReward { get => starReward; set => starReward = value; }
    public int TipReward { get => tipReward; set => tipReward = value; }
}
