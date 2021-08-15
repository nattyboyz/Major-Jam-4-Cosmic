using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BuyUI : BaseUI
{
    [SerializeField] List<CardBuySlotUI> cardSlots;
    [SerializeField] List<CardBuySlotUI> cheatCardSlot;
    public Action<CardBuySlotUI> onBuy;
    public Action<CardBuySlotUI> onSell;
    public Action onStartShow;


    public Button ingredientBtn;
    public Button blackMarketBtn;
    public GameObject ingredientScroll;
    public GameObject blackMarketScroll;

    private void Start()
    {
        Btn_Ingredient();
    }

    public void Init(PlayerData playerData)
    {
        foreach (var slot in cardSlots)
        {
            if (slot!=null && slot.TryInit(playerData))
            {
                slot.onBuy += onBuy;
                slot.onSell += onSell;
            }
        }

        foreach (var slot in cheatCardSlot)
        {
            if (slot != null && slot.TryInit(playerData))
            {
                slot.onBuy += onBuy;
                slot.onSell += onSell;
            }
        }
    }

    public void UpdateShopInventory(PlayerData playerData)
    {
        foreach (var slot in cardSlots)
        {
            if (slot != null && slot.TryInit(playerData))
            {
            }
        }

        foreach (var slot in cheatCardSlot)
        {
            if (slot != null && slot.TryInit(playerData))
            {
            }
        }
    }

    public void Btn_Done()
    {
        Hide();
    }

    public void Btn_Ingredient()
    {
        ingredientScroll.SetActive(true);
        blackMarketScroll.SetActive(false);
        ingredientBtn.interactable = false;
        blackMarketBtn.interactable = true;
    }

    public void Btn_BlackMarket()
    {
        ingredientScroll.SetActive(false);
        blackMarketScroll.SetActive(true);
        ingredientBtn.interactable = true;
        blackMarketBtn.interactable = false;
    }

    public override IEnumerator ieShow(Action onComplete = null)
    {
        onStartShow?.Invoke();
        return base.ieShow(onComplete);
    }
}
