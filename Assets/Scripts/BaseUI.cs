using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [SerializeField] protected Canvas mainCanvas;
    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip in_clip;
    [SerializeField] protected AnimationClip out_clip;
    protected bool inProcess = false;
    protected bool isShow = false;
    Coroutine showHideRoutine;

    public bool IsShow { get => isShow;}
    public bool InProcess { get => inProcess;}

    public virtual void Show(Action onComplete = null)
    {
        if (showHideRoutine != null) StopCoroutine(showHideRoutine);
        showHideRoutine = StartCoroutine(ieShow(onComplete));
    }

    public virtual void Hide(Action onComplete = null)
    {
        if (showHideRoutine != null) StopCoroutine(showHideRoutine);
        showHideRoutine = StartCoroutine(ieHide(onComplete));
    }

    public virtual IEnumerator ieShow(Action onComplete = null)
    {
        //if (!InProcess)
        //{
        inProcess = true;
        isShow = true;
        if (mainCanvas) mainCanvas.enabled = true;
        if (animator && in_clip)
        {
            animator.SetTrigger("in");
            yield return new WaitForSeconds(in_clip.length);
        }
        else yield return null;
        onComplete?.Invoke();
        inProcess = false;
        // }
    }

    public virtual IEnumerator ieHide(Action onComplete = null)
    {
        //if (!InProcess)
        //{
        inProcess = true;
        isShow = false;
        if (animator && out_clip)
        {
            animator.SetTrigger("out");
            yield return new WaitForSeconds(out_clip.length);
        }
        else yield return null;
        if (mainCanvas) mainCanvas.enabled = false;
        onComplete?.Invoke();

        inProcess = false;
        //}
    }
}
