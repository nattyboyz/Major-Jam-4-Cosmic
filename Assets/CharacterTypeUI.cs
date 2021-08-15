using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CharacterTypeUI : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler
{
    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip in_clip;
    [SerializeField] protected AnimationClip out_clip;

    [SerializeField] Image icon;
    [SerializeField] Image lineImg;
    [SerializeField] TextMeshProUGUI characterTxt;
    [SerializeField] TextMeshProUGUI descriptionTxt;
    public bool isShow = false;

    public void Show(string name)
    {
     
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        lineImg.color = Color.white;
        characterTxt.color = Color.white;
        characterTxt.text = name;
        descriptionTxt.text = "";
        Show();
    }

    public void Show(CustomerType customerType)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = customerType.Icon;
        lineImg.color = customerType.Color;
        characterTxt.color = customerType.Color;
        characterTxt.text = customerType.Name;
        descriptionTxt.text = customerType.Description;
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
        isShow = true;
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
        isShow = false;
        if (animator && out_clip)
            {
                animator.SetTrigger("out");
                yield return new WaitForSeconds(out_clip.length);
            }
            else yield return null;

            onComplete?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        descriptionTxt.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        descriptionTxt.gameObject.SetActive(true);
    }
}
