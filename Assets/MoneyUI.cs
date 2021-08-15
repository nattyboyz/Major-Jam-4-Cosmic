using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyUI : BaseUI
{
    [SerializeField] TextMeshProUGUI moneyTxt;
    public void UpdateText(int money)
    {
        moneyTxt.text = "$ " + money;
    }

    public void Success()
    {
        animator.SetTrigger("success");
    }

    public void Fail()
    {
        animator.SetTrigger("fail");
    }

}
