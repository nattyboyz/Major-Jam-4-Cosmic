using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ShopOpenUI : BaseUI
{
    [SerializeField] BuyUI buyUi;
    [SerializeField] Button open_btn;
    [SerializeField] Button buy_btn;
    public Action onOpen;

    private void Awake()
    {
        mainCanvas.enabled = true;
        ActiveButton(true);
    }

    public void Btn_Buy()
    {
        buyUi.Show();
    }

    public void Btn_Open()
    {
        onOpen?.Invoke();
    }

    public void ActiveButton(bool value)
    {
        open_btn.gameObject.SetActive(value);
        buy_btn.gameObject.SetActive(value);
    }
}
