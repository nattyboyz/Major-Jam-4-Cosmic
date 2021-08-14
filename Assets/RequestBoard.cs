using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public enum CustomerReaction { Normal, Enjoy, Mad, Exciting}

public class RequestBoard : DragOnSpot
{
    [SerializeField] CustomerPortrait customerPotrait;

    [Header("UI")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] IngredientIcon baseIngredientIcon;
    [SerializeField] Image focusGraphic;
    [SerializeField] Image menuImage;

    [SerializeField] Image name_img;
    [SerializeField] TextMeshProUGUI name_txt;

    [SerializeField] Image price_img;
    [SerializeField] TextMeshProUGUI price_txt;

    [Header("Cache Data")]
    [SerializeField] RequestData requestData;
    [SerializeField] List<IngredientSetting> settings;
    Dictionary<IngredientSetting, IngredientIcon> ingredientIcons = new Dictionary<IngredientSetting, IngredientIcon>();
    [SerializeField] Transform ingredientParent;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip in_clip;
    [SerializeField] AnimationClip out_clip;
    [SerializeField] AnimationClip fail_clip;
    [SerializeField] AnimationClip complete_clip;

    public bool allowInteract = false;

    [Header("Event")]
    public bool isComplete = false;
    public Action onCompleteRequest;
    public Action onFailRequest;
    public Action<Dragable> onExecuteComplete;
    public Action<Dragable> onExecuteFail;
    public Action onTimeout;

    [SerializeField] Processbar processbar;
    [SerializeField] bool isProcessing = false;
    float time = 0;

    public RequestData RequestData { get => requestData;}
    public List<IngredientSetting> Settings { get => settings;}

    [Header("Money Float")]
    [SerializeField] float moneyFloatTime = 2;
    [SerializeField] float moneyFloatSpeed = 4;


    private void Start()
    {
        UnFocus();
    }

    private void Update()
    {
        if (isProcessing)
        { 
            time += Time.deltaTime;
            if (time >= this.requestData.Time)
            {
                TimesUp();
                isProcessing = false;
            }
            else
            {
                processbar.Set(1 - (time / this.requestData.Time));
            }
        }
    }

    #region Time

    public void ChargeTime(float second)
    {
        time += second;
    }

    void TimesUp()
    {
        onTimeout?.Invoke();
        RequestTimeout();
    }

    #endregion

    public void Init(RequestData requestData)
    {
        this.requestData = requestData;
        isComplete = false;
        isProcessing = false;
        time = 0;

        processbar.Set(1);

        foreach (var kvp in ingredientIcons)
        {
            Destroy(kvp.Value.gameObject);
        }

        ingredientIcons = new Dictionary<IngredientSetting, IngredientIcon>();
        settings = new List<IngredientSetting>();

        foreach (var ingredient in requestData.Menu.ingredients)
        {
            Settings.Add(CreateIngredientSetting(ingredient));
        }

        foreach(var ingredient in requestData.Extra_ingredients)
        {
            Settings.Add(CreateIngredientSetting(ingredient));
        }

        customerPotrait.SetCharacter(requestData.CustomerData);
        customerPotrait.onEnter = () => {
            if (requestData.ShowCustomerType) customerPotrait.ShowType(requestData.CustomerData);
            else customerPotrait.ShowNone();
        };
        customerPotrait.onExit = () => {
            customerPotrait.HideType();
        };

        price_txt.text = "$" +requestData.Price.ToString();
        name_txt.text = requestData.Menu.menuName;
    }

    IngredientSetting CreateIngredientSetting(IngredientData ingredient)
    {
        IngredientSetting setting = new IngredientSetting(ingredient, false);
        Settings.Add(setting);

        var icon = Instantiate<IngredientIcon>(baseIngredientIcon);
        icon.Set(setting.Ingredient);
        icon.transform.SetParent(ingredientParent);
        icon.transform.localScale = new Vector3(1, 1, 1);
        menuImage.sprite = requestData.Menu.sprite;
        ingredientIcons.Add(setting, icon);
        return setting;
    }

    public override void Focus(Dragable dragable)
    {
        focusGraphic.gameObject.SetActive(true);
    }

    public override void UnFocus()
    {
        focusGraphic.gameObject.SetActive(false);
    }

    public override void Execute(Dragable dragableObject)
    {
        if (dragableObject is Card)
        {
            Card card = dragableObject as Card;
            UnFocus();

            foreach (var setting in Settings)
            {
                if (card.cardData.ingredient.Key == setting.Ingredient.Key && !setting.Complete)
                {
                    setting.Complete = true;
                    setting.CardData = card.cardData;
                    CheckType _check = CheckType.Pass;

                    if (card.cardData.modifiers != null)
                    {
                        foreach (var m in card.cardData.modifiers)
                        {
                            if (m.Type == ModifierType.Curse)
                            {
                                _check = CheckType.Doubt;
                                break;
                            }
                            else if (m.Type == ModifierType.Buff)
                            {
                                _check = CheckType.Great;
                            }
                        }
                    }

                    ingredientIcons[setting].SetCheck(_check);
                    ExecuteComplete(card);
                    return;
                }
            }
            ExecuteFail(card);
        }
        else if (dragableObject is CheatCard)
        {
            CheatCard card = dragableObject as CheatCard;
            UnFocus();
            Debug.Log("RequestBoard:: try use CHEAT CARD ->" + card.ingredientData.Name);
            if(card.ingredientData.Name == "Ratz")
            {
                //requestData.
                ChargeTime(5);
            }
        }
    }

    public bool IsComplete()
    {
        foreach (var setting in Settings)
        {
            if (setting.Complete== false)
            {
                return false;
            }
        }
        return true;
    }

    #region Complete request

    public void CompleteRequest(string reason = "")
    {
        isComplete = true;
        isProcessing = false;
        time = 0;
        Hide(onCompleteRequest);
    }

    public void FailRequest(string reason = "")
    {
        isComplete = true;
        isProcessing = false;
        time = 0;
        Hide(onFailRequest);
    }

    public void RequestTimeout()
    {
        isComplete = true;
        isProcessing = false;
        time = 0;
        Hide(onFailRequest);
    }

    #endregion

    public void SetCustomerReaction(CustomerReaction reaction, Action onComplete = null)
    {

    }

    public virtual void ExecuteFail(Card card)
    {
        onExecuteFail?.Invoke(card);
    }

    public virtual void ExecuteComplete(Card card)
    {
        animator.SetTrigger("add");
        onExecuteComplete?.Invoke(card);
    }

    #region UI

    public void Active(bool active)
    {
        if (active) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
    }

    public void Show(Action onComplete = null)
    {
        StartCoroutine(ieShow(onComplete));
    }

    public void Hide(Action onComplete = null)
    {
        StartCoroutine(ieHide(onComplete));
    }

    IEnumerator ieShow(Action onComplete = null)
    {
        yield return customerPotrait.ieIn();
        yield return new WaitForSeconds(1);
        canvasGroup.alpha = 1;
        animator.SetTrigger("in");
        yield return new WaitForSeconds(in_clip.length);
        onComplete?.Invoke();
        isProcessing = true;
        allowInteract = true;
    }

    IEnumerator ieHide(Action onComplete = null)
    {
        allowInteract = false;
        animator.SetTrigger("out");
        yield return new WaitForSeconds(out_clip.length);
        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(1);
        //onComplete?.Invoke();
        yield return customerPotrait.ieOut(onComplete);
    }

    #endregion

    #region Customer

    public void ExposeIdentity()
    {
        RequestData.ShowCustomerType = true; 
    }

    public void ShowMoney(int amount)
    {
        customerPotrait.SetMoneyFloat(amount, moneyFloatSpeed, moneyFloatTime);
    }

    #endregion


}

[System.Serializable]
public class IngredientSetting
{
    [SerializeField] IngredientData ingredient;
    [SerializeField] bool complete = false;
    [SerializeField] CardData cardData;

    public IngredientSetting(IngredientData ingredient, bool complete)
    {
        this.Ingredient = ingredient;
        this.Complete = complete;
    }

    public CardData CardData { get => cardData; set => cardData = value; }
    public bool Complete { get => complete; set => complete = value; }
    public IngredientData Ingredient { get => ingredient; set => ingredient = value; }
}
