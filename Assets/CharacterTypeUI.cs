using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterTypeUI : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip in_clip;
    [SerializeField] protected AnimationClip out_clip;

    [SerializeField] Image icon;
    [SerializeField] Image lineImg;
    [SerializeField] TextMeshProUGUI characterTxt;

    public void Show(string name)
    {
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        lineImg.color = Color.white;
        characterTxt.color = Color.white;
        characterTxt.text = name;
        Show();
    }

    public void Show(CustomerType customerType)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = customerType.Icon;
        lineImg.color = customerType.Color;
        characterTxt.color = customerType.Color;
        characterTxt.text = customerType.Name;
        Show();
    }

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
            if (animator && in_clip)
            {
                animator.SetTrigger("in");
                yield return new WaitForSeconds(in_clip.length);
            }
            else yield return null;
            onComplete?.Invoke();   
    }

    public virtual IEnumerator ieHide(Action onComplete = null)
    {
            if (animator && out_clip)
            {
                animator.SetTrigger("out");
                yield return new WaitForSeconds(out_clip.length);
            }
            else yield return null;

            onComplete?.Invoke();
    }


}
