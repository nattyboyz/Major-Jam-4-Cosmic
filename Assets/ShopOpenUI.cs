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
    [SerializeField] Button close_btn;

    public Action onOpen;
    public Action onClose;

    public void ActiveOpenButton(bool active)
    {
        open_btn.gameObject.SetActive(active);
    }

    public void ActiveCloseButton(bool active)
    {
        close_btn.gameObject.SetActive(active);
    }

    public void Btn_Open()
    {
        StartCoroutine(ieOpen());
    }

    IEnumerator ieOpen()
    {
        //animator.SetTrigger("in");
        //yield return new WaitForSeconds(in_clip.length);
        yield return ieShow();
        onOpen?.Invoke();
    }

    public void Btn_Close()
    {
        StartCoroutine(ieClose());
    }

    public void Btn_Buy()
    {
        buyUi.Show();
    }


    IEnumerator ieClose()
    {
        //animator.SetTrigger("out");
        //yield return new WaitForSeconds(out_clip.length);
        yield return ieHide();
        onClose?.Invoke();
    }
}
