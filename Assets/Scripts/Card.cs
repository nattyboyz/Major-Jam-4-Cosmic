using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;

public enum CardType { Normal, Spoil};

public class Card : Dragable, IPointerEnterHandler, IPointerExitHandler
{
    public ModifierIcon baseModifierIcon;

    public CardType cardType;
    [SerializeField] RectTransform rect;
    [SerializeField] Hand hand;
    public int handIndex;
    public Vector2 handPosition;
    //public Vector3 deltaDragPos;
    //static Vector3 zero = new Vector3(0, 0, 0);
    //static Vector3 one = new Vector3(1 ,1, 1);

    //Cache data
    //public IngredientData ingredientData;
    //public List<ModifierData> modifiers = new List<ModifierData>();

    public CardData cardData;

    public Transform modifierIconParent;
    public Dictionary<ModifierData, ModifierIcon> modifierIconDict = new Dictionary<ModifierData, ModifierIcon>();


    public Action<Card> onEnter;
    public Action<Card> onExit;
    //public Action<GameObject> onDragRayUpdate;
    //public Action<GameObject> onDragRelease;


    public Image headerImage;
    public Image iconImage;
    public TextMeshProUGUI cardName;
    public CanvasGroup canvasGroup;


    public RectTransform Rect { get => rect; set => rect = value; }

    public void Active(bool active)
    {
        if (active) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
    }

    //public override void OnBeginDrag(PointerEventData eventData)
    //{
    //    this.transform.localScale *= 1.2f;
    //    deltaDragPos = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    //    deltaDragPos.Set(deltaDragPos.x, deltaDragPos.y, this.transform.position.z);
    //    onStartDrag?.Invoke(this);
    //}

    //public override void OnDrag(PointerEventData eventData)
    //{
    //    var pos = Camera.main.ScreenToWorldPoint(eventData.position);
    //    pos.Set(pos.x, pos.y, this.transform.position.z);
    //   // Debug.Log(pos);

    //    this.gameObject.transform.position = pos + deltaDragPos;
    //    var result = eventData.pointerCurrentRaycast;
    //    if(result.gameObject != null && result.gameObject != gameObject)
    //    {
    //        onDragRayUpdate?.Invoke(result.gameObject);
    //    }
    //}

    //public override void OnEndDrag(PointerEventData eventData)
    //{
    //    this.transform.localScale = new Vector3(1, 1, 1);
    //    deltaDragPos = zero;

    //    var result = eventData.pointerCurrentRaycast;
    //    if (result.gameObject != null && result.gameObject != gameObject)
    //    {
    //        onDragRelease?.Invoke(result.gameObject);
    //    }

    //    onEndDrag?.Invoke(this);
    //}

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(this);
    }

    #region Setup


    public virtual void Init(CardData cardData)
    {
        this.cardData = cardData;
        Set(cardData.ingredient, cardData.modifiers);
    }


    public virtual void Init(IngredientData data, List<ModifierData> modifiers = null)
    {
        cardData = new CardData(data, modifiers);
        Set(cardData.ingredient, cardData.modifiers);
    }

    void Set(IngredientData data, List<ModifierData> modifiers = null)
    {
        if (modifiers != null)
        {
            foreach (ModifierData m_data in modifiers)
            {
                var icon = Instantiate(baseModifierIcon);
                icon.transform.SetParent(modifierIconParent);
                icon.transform.localScale = one;
                icon.Init(m_data);

                if (!modifierIconDict.ContainsKey(m_data))
                {
                    modifierIconDict.Add(m_data, icon);
                }
                else
                {
                    Debug.Log("<color=red>Cannot add same modifier</color>. Already have " + m_data + " key");
                }
            }
        }
        SetColor(data.Color);
        SetName(data.Name);
        SetImage(data.Icon);
    }

    public virtual void SetColor(Color color)
    {
        if (headerImage != null) headerImage.color = color;
    }

    public virtual void SetName(string name)
    {
        if(cardName!=null) cardName.text = name;
    }

    public virtual void SetImage(Sprite sprite)
    {
        if (iconImage != null) iconImage.sprite = sprite;
    }

    public virtual void SetType(CardType cardType)
    {
        this.cardType = cardType;
    }

    #endregion

    public void Discard()
    {

    }

}

[Serializable]
public class CardData
{
    public IngredientData ingredient;
    public bool isSpoil = false;
    public List<ModifierData> modifiers = new List<ModifierData>();
    

    public CardData(IngredientData data, List<ModifierData> modifiers, bool isSpoil= false)
    {
        this.ingredient = data;
        if (modifiers != null)
        {
            this.modifiers = new List<ModifierData>(modifiers);
        }
        else
        {
            Debug.Log("Modifier is NULL " + data.Name);
        }
        this.isSpoil = isSpoil;
    }
}
