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

}
