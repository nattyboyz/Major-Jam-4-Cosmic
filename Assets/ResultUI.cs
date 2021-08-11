using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultUI : MonoBehaviour
{
   [SerializeField] Canvas mainCanvas;
    public Action onClickNext;
    public Action onClickRetry;
    public Action onClickHome;



    public void Init()
    {

    }

    public void Show()
    {
        mainCanvas.enabled = true;
    }

    public void Hide()
    {
        mainCanvas.enabled = false;
    }

    public void Btn_Next()
    {

    }

    public void Btn_Retry()
    {

    }

    public void Btn_Home()
    {

    }

}
