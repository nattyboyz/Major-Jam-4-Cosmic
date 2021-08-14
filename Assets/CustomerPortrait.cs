using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class CustomerPortrait : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] FloatingTextUI baseFloatingText;
    [SerializeField] Transform floatTextParent;

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
       if(characterTypeUI.isShow) HideType();
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

    public void SetMoneyFloat(int amount,float speed, float duration )
    {
        var floatText = Instantiate(baseFloatingText);
        string sign = "+";
        if (amount < 0) sign = "-";
        floatText.SetText(sign+" $" + amount);
        floatText.transform.SetParent(floatTextParent);
        floatText.transform.localScale = new Vector3(1, 1, 1);
        floatText.transform.localPosition = new Vector3(0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(floatText.RectTransform.DOAnchorPosY(speed, duration).SetSpeedBased());
        seq.Join( floatText.Text.DOFade(0, duration).OnComplete(()=>{ Destroy(floatText.gameObject); }));
        seq.Play();

    }
}
