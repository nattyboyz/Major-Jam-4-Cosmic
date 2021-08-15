using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ShopOpenUI : BaseUI
{
    [SerializeField] BuyUI buyUi;
    [SerializeField] Button open_btn;
    [SerializeField] Button buy_btn;

    [SerializeField] Text day_txt;
    [SerializeField] Text goal_txt;

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
        day_txt.gameObject.SetActive(value);
        goal_txt.gameObject.SetActive(value);
    }

    public void SetDay(string day)
    {
        day_txt.text = day.ToString();
    }

    public void SetGoal(string goal)
    {
        goal_txt.text = goal.ToString();
    }


}
