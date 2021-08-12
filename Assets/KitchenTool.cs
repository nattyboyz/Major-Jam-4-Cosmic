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

    public Action<CheatCard> onExecuteCheatComplete;
    public Action<CheatCard> onExecuteCheatFail;

    List<CardData> resultCards = new List<CardData>();

    IngredientData processingIngredient;
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

    public override void Focus(Dragable dragable)
    {
        base.Focus(dragable);
        if (dragable is Card)
        {
            var card = dragable as Card;
            if (processMenu.TryGetValue(card.ingredientData, out var pack))
            {
                focus_img.gameObject.SetActive(true);
            }
        }
    }

    public override void UnFocus()
    {
        if(!IsOccupied) processIngredient.gameObject.SetActive(false);
        focus_img.gameObject.SetActive(false);
    }

    public override void Execute(Dragable dragableObject)
    {
        if (dragableObject is Card)
        {
            Debug.Log("Execute processing normal card");
            Card card = dragableObject as Card;
            if (processMenu.TryGetValue(card.ingredientData, out var pack))
            {
                processingIngredient = card.ingredientData;
                processing = true;
                processbar.gameObject.SetActive(true);
                onExecuteComplete?.Invoke(card);
            }
            else
            {
                onExecuteFail?.Invoke(card);
            }
        }
        else if (dragableObject is CheatCard)
        {
            Debug.Log("Execute processing cheat card");
            CheatCard card = dragableObject as CheatCard;
            if (processMenu.TryGetValue(card.ingredientData, out var pack))
            {
                processingIngredient = card.ingredientData;
                processing = true;
                processbar.gameObject.SetActive(true);
                onExecuteCheatComplete?.Invoke(card);
            }
            else
            {
                onExecuteCheatComplete?.Invoke(card);
            }

        }
    }

    public void Update()
    {
        if (processing)
        {
            time += Time.deltaTime;
            processbar.image.fillAmount = time / processTime;
            if (time >= processTime)
            {   
                if (processMenu.TryGetValue(processingIngredient, out var pack))
                {
                    onProcessComplete?.Invoke(pack);
                    resultCards = new List<CardData>();
                    for (int i = 0; i < pack.amount; i++)
                    {
                        resultCards.Add(new CardData(pack.data, pack.modifier));
                    }
                }
                processingIngredient = null;
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

