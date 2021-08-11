using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProcessResourceIcon : MonoBehaviour
{
    public IngredientData ingredientData;
    public int amount;
    public Image headerImage;
    public Image iconImage;
    public TextMeshProUGUI amountTxt;

    public virtual void Set(IngredientData data, int amount)
    {
        ingredientData = data;
        if (headerImage != null) headerImage.color = data.Color;
        if (iconImage != null) iconImage.sprite = data.Icon;
        this.amount = amount;
        amountTxt.text = amount.ToString();
    }

}
