using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class IngredientProcessDict : SerializableDictionary<IngredientData, Pack>{}

public class KitchenTool : DragOnSpot
{
    [SerializeField] Processbar processbar;
    public ProcessResourceIcon processIngredient;
    [SerializeField] Image focus_img;


    public IngredientProcessDict processMenu;
    public Action<Card> onExecuteComplete;
    public Action<Card> onExecuteFail;
    public Action<Pack> onProcessComplete;

    Card processingCard;
    bool processing = false;
    float processTime = 2f;
    float time = 0;

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
        processIngredient.gameObject.SetActive(false);
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
                }
                processingCard = null;
                processing = false;
                time = 0;
                processbar.gameObject.SetActive(false);
            }
        }
    }

    public bool IsProcessing()
    {
        return processing;
    }

}

[Serializable]
public class Pack
{
    public IngredientData data;
    public int amount;
}
