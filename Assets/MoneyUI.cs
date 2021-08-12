using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] Canvas mainCanvas;
    [SerializeField] TextMeshProUGUI moneyTxt;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip in_clip;
    [SerializeField] AnimationClip out_clip;

    public void UpdateText(int money)
    {
        moneyTxt.text = "$ " + money;
    }

    public void Show(Action onComplete = null)
    {
        StartCoroutine(ieShow(onComplete));
    }

    public void Hide(Action onComplete = null)
    {
        StartCoroutine(ieHide(onComplete));
    }

    IEnumerator ieShow(Action onComplete = null)
    {
        mainCanvas.enabled = true;
        animator.SetTrigger("in");
        yield return new WaitForSeconds(in_clip.length);
        onComplete?.Invoke();
    }


    IEnumerator ieHide(Action onComplete = null)
    {
        animator.SetTrigger("out");
        yield return new WaitForSeconds(out_clip.length);
        mainCanvas.enabled = false;
        onComplete?.Invoke();
    }

}
