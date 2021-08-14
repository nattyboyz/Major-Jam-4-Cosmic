using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloseShopUI : BaseUI
{
    public Action onClose;

    public void Btn_Close()
    {
        onClose?.Invoke();
    }
       

}
