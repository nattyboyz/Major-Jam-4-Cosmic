using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultUI : BaseUI
{
    public Action onNext;
    public Action onRetry;
    public Action onHome;

    public void Awake()
    {
        mainCanvas.enabled = false;
    }

    public void Btn_Next()
    {
        Hide(onNext);
    }

    public void Btn_Retry()
    {
       // Hide(onRetry);
    }

    public void Btn_Home()
    {
        Hide(onHome);
    }

}
