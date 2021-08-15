using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//public enum SlotCardType { Normal,Cheat}
public class CardBuySlotUI : MonoBehaviour
{
    //[SerializeField] SlotCardType type;
    [SerializeField] IngredientData ingredientData;
    [SerializeField] Card card;
    [SerializeField] TextMeshProUGUI owned_txt;
    [SerializeField] TextMeshProUGUI sellPrice_txt;
    [SerializeField] TextMeshProUGUI buyPrice_txt;

    public Action<CardBuySlotUI> onBuy;
    public Action<CardBuySlotUI> onSell;

    public IngredientData IngredientData { get => ingredientData;}

    public bool TryInit(PlayerData playerData)
    {
        if(card!=null)card.allowDrag = false;

        if (IngredientData != null)
        {
            card.Init(IngredientData);
            sellPrice_txt.text = "$" + IngredientData.SellPrice.ToString();
            buyPrice_txt.text = "$" + IngredientData.BuyPrice.ToString();

            int amount = 0;
            if (playerData.ingredients.ContainsKey(IngredientData))
            {
                amount = playerData.ingredients[IngredientData];

                Debug.Log("Success buying ingredient " + IngredientData.Name + " " + amount);
            }
            else if (playerData.cheats.ContainsKey(IngredientData))
            {
                amount = playerData.cheats[IngredientData];
                Debug.Log("Success buying cheats" + IngredientData.Name + " " + amount);
            }
            UpdateAmount(amount);

            return true;
        }
        else
        {
            Destroy(this.gameObject);
            return false;
        }
    }

    public void UpdateAmount(int amount)
    {
        owned_txt.text = amount.ToString();
    }

    public void Btn_Buy()
    {
        onBuy.Invoke(this);
    }

    public void Btn_Sell()
    {
        onSell.Invoke(this);
    }
}
