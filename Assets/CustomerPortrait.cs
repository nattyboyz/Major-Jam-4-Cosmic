using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class CustomerPortrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Animator animator;
    [SerializeField] Image sprite;
    [SerializeField] CharacterTypeUI characterTypeUI;
    [SerializeField]  bool show = false;
    [SerializeField]  bool active = false;
    [SerializeField] CustomerData customerData;

    public Action onClick;
    public Action onEnter;
    public Action onExit;

    public bool Show { get => show; set => show = value; }

    private void Start()
    {
        sprite.DOFade(0, 0);
    }

    public IEnumerator ieIn(Action onComplete = null)
    {
        sprite.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.3f);
        active = true;
        onComplete?.Invoke();
    }

    public IEnumerator ieOut(Action onComplete = null)
    {
        active = false;
        HideType();
        sprite.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        onComplete?.Invoke();
    }

    public void SetCharacter(CustomerData customerData)
    {
        this.customerData = customerData;
        this.sprite.sprite = customerData.Sprite;
    }

    public void SetShow(bool value)
    {
        show = value;
    }

    public void ShowType(CustomerData customerData)
    {
       characterTypeUI.Show(customerData.CustomerType);
    }

    public void ShowNone()
    {
        characterTypeUI.Show("???");
    }

    public void HideType()
    {
       characterTypeUI.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (active) onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (active) onExit?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (active) onClick?.Invoke();
    }
}
