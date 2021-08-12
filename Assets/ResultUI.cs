using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultUI : BaseUI
{
    public Action onClickNext;
    public Action onClickRetry;
    public Action onClickHome;

    //Event
    public Action onNext;
    public Action onRetry;
    public Action onHome;

    public void Awake()
    {
        mainCanvas.enabled = false;
    }

    public void Init()
    {

    }

    public void Btn_Next()
    {
        onNext?.Invoke();
    }

    public void Btn_Retry()
    {
        onRetry?.Invoke();
    }

    public void Btn_Home()
    {
        onHome?.Invoke();
    }

}
