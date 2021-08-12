using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatCard : Dragable
{

    public IngredientData ingredientData;
    public CanvasGroup canvasGroup;
    public Image headerImage;
    public Image iconImage;
    public TextMeshProUGUI cardName;

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

}
