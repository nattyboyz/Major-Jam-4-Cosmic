using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CheatCard : Dragable
{
    public RectTransform rectTransform;
    public Vector3 originalPosition;

    public CardData cardData;
    //public IngredientData ingredientData;



    public CanvasGroup canvasGroup;
    public Image headerImage;
    public Image iconImage;
    public TextMeshProUGUI cardName;
    public int amountLeft = 0;
    public Action<int> onSetAmount;
   

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Init(CardData cardData)
    {
        if (cardData != null)
        {
            this.cardData = cardData; ;
            headerImage.color = cardData.ingredient.Color;
            iconImage.sprite = cardData.ingredient.Icon;
            cardName.text = cardData.ingredient.Name;
        }
    }

    public void Active(bool active)
    {
        if (active) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }

    public void CacheOriginalPosition()
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ResetPosition();
    }

    public void SetAmount(int amount)
    {
        onSetAmount?.Invoke(amount);
    }
}
