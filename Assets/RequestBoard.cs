using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class RequestBoard : DragOnSpot
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] IngredientIcon baseIngredientIcon;
    [SerializeField] Image focusGraphic;
    [SerializeField] Image menuImage;

    [SerializeField] RequestData requestData;
    [SerializeField]  List<IngredientSetting> settings;
    Dictionary<IngredientSetting, IngredientIcon> ingredientIcons = new Dictionary<IngredientSetting, IngredientIcon>();
    [SerializeField] Transform ingredientParent;


    public bool isComplete = false;
    public Action onCompleteRequest;
    public Action onFailRequest;
    public Action<Card> onExecuteComplete;
    public Action<Card> onExecuteFail;

    public RequestData RequestData { get => requestData; set => requestData = value; }

    private void Start()
    {
        UnFocus();
    }

    public void Init(RequestData requestData)
    {
        this.RequestData = requestData;
        isComplete = false;

        foreach (var kvp in ingredientIcons)
        {
            Destroy(kvp.Value.gameObject);
        }

        ingredientIcons = new Dictionary<IngredientSetting, IngredientIcon>();
        settings = new List<IngredientSetting>();

        foreach (var ingredient in requestData.menu.ingredients)
        {
            IngredientSetting setting = new IngredientSetting(ingredient, false);
            settings.Add(setting);

            var icon = Instantiate<IngredientIcon>(baseIngredientIcon);
            icon.Set(setting.ingredient);
            icon.transform.SetParent(ingredientParent);
            icon.transform.localScale = new Vector3(1, 1, 1);
            menuImage.sprite = requestData.menu.sprite;
            ingredientIcons.Add(setting, icon);
        }
    }

    public override void Focus(Card card)
    {
        focusGraphic.gameObject.SetActive(true);
    }

    public override void UnFocus()
    {
        focusGraphic.gameObject.SetActive(false);
    }

    public override void Execute(Card card)
    {
        UnFocus();

        foreach (var setting in settings)
        {
            if(card.ingredientData.Key == setting.ingredient.Key && !setting.complete)
            {
                setting.complete = true;
                ingredientIcons[setting].SetCheck(CheckType.Pass);
                ExecuteComplete(card);
                return;
            }
        }

        ExecuteFail(card);   
    }

    public bool IsComplete()
    {
        foreach (var setting in settings)
        {
            if (setting.complete== false)
            {
                return false;
            }
        }
        return true;
    }

    public void CompleteRequest()
    {
        onCompleteRequest?.Invoke();
        isComplete = true;
        Active(false);
    }

    public void FailRequest()
    {
        onFailRequest?.Invoke();
        isComplete = true;
        Active(false);
    }

    public virtual void ExecuteFail(Card card)
    {
        onExecuteFail?.Invoke(card);
    }

    public virtual void ExecuteComplete(Card card)
    {
        onExecuteComplete?.Invoke(card);
    }

    public void Active(bool active)
    {
        if (active) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
    }

}

[System.Serializable]
public class IngredientSetting
{
    public IngredientData ingredient;
    public bool complete = false;
    
    public IngredientSetting(IngredientData ingredient, bool complete)
    {
        this.ingredient = ingredient;
        this.complete = complete;
    }
}
