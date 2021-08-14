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
    public Action onOpen;

    public void Btn_Buy()
    {
        buyUi.Show();
    }

    public void Btn_Open()
    {
        StartCoroutine(ieOpen());
    }

    IEnumerator ieOpen()
    {
        yield return ieHide();
        onOpen?.Invoke();
    }
}
