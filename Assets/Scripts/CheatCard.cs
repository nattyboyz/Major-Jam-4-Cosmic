using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheatCard : Dragable
{
    public RectTransform rectTransform;
    public Vector3 originalPosition;

    public IngredientData ingredientData;
    public CanvasGroup canvasGroup;
    public Image headerImage;
    public Image iconImage;
    public TextMeshProUGUI cardName;

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Deduct()
    {

    }

    public void Init()
    {
        if (ingredientData != null)
        {
            headerImage.color = ingredientData.Color;
            iconImage.sprite = ingredientData.Icon;
            cardName.text = ingredientData.Name;
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

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ResetPosition();
    }

}
