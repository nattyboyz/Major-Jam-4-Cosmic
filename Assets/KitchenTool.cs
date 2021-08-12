using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[Serializable]
public class IngredientProcessDict : SerializableDictionary<IngredientData, Pack>{}

public class KitchenTool : DragOnSpot, IPointerClickHandler
{
    [SerializeField] Processbar processbar;
    public ProcessResourceIcon processIngredient;
    [SerializeField] Image focus_img;


    public IngredientProcessDict processMenu;
    public Action<Card> onExecuteComplete;
    public Action<Card> onExecuteFail;
    public Action<Pack> onProcessComplete;
    public Action<CardData> onClickToRecieveCard;

    List<CardData> resultCards = new List<CardData>();

    Card processingCard;
    bool processing = false;
    float processTime = 2f;
    float time = 0;

    public bool IsResult { get { return resultCards.Count > 0; } }
    public bool IsProcessing { get { return processing; } }
    public bool IsOccupied { get { return processing || IsResult; } }

    private void Start()
    {
        processbar.gameObject.SetActive(false);
        UnFocus();
    }

    public override void Focus(Card card)
    {
        base.Focus(card);
        if (processMenu.TryGetValue(card.ingredientData, out var pack))
        {
            focus_img.gameObject.SetActive(true);
        }
    }

    public override void UnFocus()
    {
        if(!IsOccupied) processIngredient.gameObject.SetActive(false);
        focus_img.gameObject.SetActive(false);
    }

    public override void Execute(Card card)
    {
        if(processMenu.TryGetValue(card.ingredientData,out var pack))
        {
            processingCard = card;
            processing = true;
            processbar.gameObject.SetActive(true);
            ExecuteComplete(card);
        }
        else
        {
            ExecuteFail(card);
        }
    }

    public virtual void ExecuteFail(Card card)
    {
        onExecuteFail?.Invoke(card);
    }

    public virtual void ExecuteComplete(Card card)
    {

        onExecuteComplete?.Invoke(card);
    }

    public void Update()
    {
        if (processing)
        {
            time += Time.deltaTime;
            processbar.image.fillAmount = time / processTime;
            if (time >= processTime)
            {   
                if (processMenu.TryGetValue(processingCard.ingredientData, out var pack))
                {
                    onProcessComplete?.Invoke(pack);
                    resultCards = new List<CardData>();
                    for (int i = 0; i < pack.amount; i++)
                    {
                        resultCards.Add(new CardData(pack.data, pack.modifier));
                    }
                }
                processingCard = null;
                processing = false;
                time = 0;
                processbar.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (resultCards.Count > 0)
        {
            var cardData = resultCards[0];
            resultCards.RemoveAt(0);

            Debug.Log("Get " + cardData.ingredient.Name );
            processIngredient.Set(cardData.ingredient, resultCards.Count);
            onClickToRecieveCard?.Invoke(cardData);
            if (!IsOccupied) processIngredient.gameObject.SetActive(false);

        }
    }
}

[Serializable]
public class Pack
{
    public IngredientData data;
    public int amount;
    public List<ModifierData> modifier;
}

