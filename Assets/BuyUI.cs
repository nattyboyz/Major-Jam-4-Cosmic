using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BuyUI : BaseUI
{
    [SerializeField] List<CardBuySlotUI> cardSlots;
    public Action<CardBuySlotUI> onBuy;
    public Action<CardBuySlotUI> onSell;

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
    }

    public void Btn_Done()
    {
        Hide();
    }

}
