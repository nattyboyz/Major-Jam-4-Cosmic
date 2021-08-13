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


    public virtual void Show(Action onComplete = null)
    {
        StartCoroutine(ieShow(onComplete));
    }

    public virtual void Hide(Action onComplete = null)
    {
        StartCoroutine(ieHide(onComplete));
    }

    public virtual IEnumerator ieShow(Action onComplete = null)
    {
        if (!inProcess)
        {
            inProcess = true;
            if (mainCanvas) mainCanvas.enabled = true;
            if (animator && in_clip)
            {
                animator.SetTrigger("in");
                yield return new WaitForSeconds(in_clip.length);
            }
            else yield return null;
            onComplete?.Invoke();
            isShow = true;
            inProcess = false;
        }
    }

    public virtual IEnumerator ieHide(Action onComplete = null)
    {
        if (!inProcess)
        {
            inProcess = true;
            if (animator && out_clip)
            {
                animator.SetTrigger("out");
                yield return new WaitForSeconds(out_clip.length);
            }
            else yield return null;
            if (mainCanvas) mainCanvas.enabled = false;
            onComplete?.Invoke();
            isShow = false;
            inProcess = false;
        }
    }

}
