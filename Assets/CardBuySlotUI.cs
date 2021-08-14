using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBuySlotUI : MonoBehaviour
{
    [SerializeField] IngredientData ingredientData;
    [SerializeField] Card card;
    [SerializeField] TextMeshProUGUI owned_txt;
    [SerializeField] TextMeshProUGUI sellPrice_txt;
    [SerializeField] TextMeshProUGUI buyPrice_txt;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        if(card!=null)card.allowDrag = false;

        if (ingredientData != null)
        {
            card.Init(ingredientData); 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Btn_Buy()
    {

    }

    public void Btn_Sell()
    {

    }
}
