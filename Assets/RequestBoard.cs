using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class RequestBoard : DragOnSpot
{
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

    [Header("Event")]
    public bool isComplete = false;
    public Action onCompleteRequest;
    public Action onFailRequest;
    public Action<Dragable> onExecuteComplete;
    public Action<Dragable> onExecuteFail;

    [SerializeField] Processbar processbar;
    [SerializeField] bool isProcessing = false;
    float time = 0;

    public RequestData RequestData { get => requestData; set => requestData = value; }

    private void Start()
    {
        UnFocus();
    }

    private void Update()
    {
        if (isProcessing)
        { 
            time += Time.deltaTime;
            if (time >= this.requestData.time)
            {
                TimeUp();
                isProcessing = false;
            }
            else
            {
                processbar.Set(1 - (time / this.requestData.time));
            }

        }
    }

    public void Init(RequestData requestData)
    {
        this.RequestData = requestData;
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


        price_txt.text = "$" +requestData.menu.basePrice.ToString();
        name_txt.text = requestData.menu.menuName;
    }

    void TimeUp()
    {
        FailRequest();
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

            foreach (var setting in settings)
            {
                if (card.ingredientData.Key == setting.ingredient.Key && !setting.complete)
                {
                    setting.complete = true;
                    ingredientIcons[setting].SetCheck(CheckType.Pass);
                    ExecuteComplete(card);
                    return;
                }
            }
            ExecuteFail(card);
        }
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
        //onCompleteRequest?.Invoke();
        isComplete = true;
        isProcessing = false;
        time = 0;
        Hide(onCompleteRequest);
    }

    public void FailRequest()
    {
        //onFailRequest?.Invoke();
        isComplete = true;
        isProcessing = false;
        time = 0;
        Hide(onFailRequest);
    }

    public virtual void ExecuteFail(Card card)
    {
        onExecuteFail?.Invoke(card);
    }

    public virtual void ExecuteComplete(Card card)
    {
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
        canvasGroup.alpha = 1;
        animator.SetTrigger("in");
        yield return new WaitForSeconds(in_clip.length);
        onComplete?.Invoke();
        isProcessing = true;
    }


    IEnumerator ieHide(Action onComplete = null)
    {
        animator.SetTrigger("out");
        yield return new WaitForSeconds(out_clip.length);
        canvasGroup.alpha = 0;
        onComplete?.Invoke();
    }


    #endregion
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
