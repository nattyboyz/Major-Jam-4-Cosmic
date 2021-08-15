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
    [SerializeField] FloatingTextUI baseRatingText;
    [SerializeField] Transform floatTextParent;

    [SerializeField] Animator animator;
    [SerializeField] Image sprite;
    [SerializeField] CharacterTypeUI characterTypeUI;
    [SerializeField]  bool show = false;
    [SerializeField]  bool active = false;
    [SerializeField] CustomerData customerData;
    [SerializeField] Image specialIcon;

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
        this.sprite.sprite = customerData.SpriteIdle;
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
        if (active)
        {
            clickEvent?.Invoke();
            onClick?.Invoke();
            animator.SetTrigger("click");
        }
    }

    public void SetMoneyFloat(int amount,float speed, float duration, Vector3 targetPosition)
    {
        var floatText = Instantiate(baseFloatingText);
        string sign = "+";
        if (amount < 0) sign = "-";
        floatText.SetText(sign+"$" + amount);
        floatText.transform.SetParent(floatTextParent);
        floatText.transform.position = targetPosition;
        floatText.transform.localScale = new Vector3(1, 1, 1);
        floatText.transform.localPosition = new Vector3(0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(floatText.RectTransform.DOAnchorPosY(speed, duration).SetSpeedBased().SetEase(Ease.OutCubic));
        seq.Join(floatText.Text.DOFade(0, duration).SetEase(Ease.InCubic).OnComplete(()=>{ Destroy(floatText.gameObject); }));
        seq.Join(floatText.TextBG.DOFade(0, duration).SetEase(Ease.InCubic));
        seq.Play();

    }

    public void SetRatingFloat(int amount, float speed, float duration, Vector3 targetPosition)
    {
        var floatText = Instantiate(baseRatingText);
        string sign = "+";
        if (amount < 0) sign = "-";
        floatText.SetText(sign + Mathf.Abs(amount));
        floatText.transform.SetParent(floatTextParent);
        floatText.transform.position = targetPosition;
        floatText.transform.localScale = new Vector3(1, 1, 1);
        floatText.transform.localPosition = new Vector3(0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(floatText.RectTransform.DOAnchorPosY(speed, duration).SetSpeedBased().SetEase(Ease.OutCubic));
        seq.Join(floatText.Text.DOFade(0, duration).SetEase(Ease.InCubic).OnComplete(() => { Destroy(floatText.gameObject); }));
        seq.Join(floatText.TextBG.DOFade(0, duration).SetEase(Ease.InCubic));
        seq.Play();

    }


    //public void SetSpecialIcon(Sprite sprite)
    //{
    //    specialIcon.sprite = sprite;
    //}

    Action clickEvent;
    public void SetClickEvent(Sprite icon, Action clickEvent)
    {
        specialIcon.sprite = icon;
        specialIcon.gameObject.SetActive(true);
        this.clickEvent = clickEvent;
    }

    public void RemoveClickEvent()
    {
        specialIcon.gameObject.SetActive(false);
        clickEvent = null;
    }

    //public void ShowSpecialIcon(bool value)
    //{
    //    specialIcon.gameObject.SetActive(value);
    //}

    public void SetExpression(CharExpression expression)
    {
        if (customerData!=null)
        {
            switch (expression)
            {
                case CharExpression.Idle: sprite.sprite = customerData.SpriteIdle; break;
                case CharExpression.Sus:
                    {
                        if (customerData.SpriteSus != null)
                            sprite.sprite = customerData.SpriteSus;
                        else
                            Debug.Log("No sus expression");
                        break;
                    }
                case CharExpression.Happy: sprite.sprite = customerData.SpriteHappy; break;
                case CharExpression.Angry: sprite.sprite = customerData.SpriteAngry; break;
                default: sprite.sprite = customerData.SpriteIdle; break;
            }
        }
    }

}

public enum CharExpression
{
    Idle, Sus, Happy, Angry
}
