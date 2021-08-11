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

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip in_clip;
    [SerializeField] AnimationClip out_clip;

    public void Awake()
    {
        mainCanvas.enabled = false;
    }

    public void Init()
    {

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
