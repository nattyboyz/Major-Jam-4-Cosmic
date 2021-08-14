using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameOverUI : BaseUI
{
    public Action onClickOverBtn;

    public void Awake()
    {
        mainCanvas.enabled = false;
    }

    public void Btn_Over()
    {
        onClickOverBtn?.Invoke();
    }
   


}
