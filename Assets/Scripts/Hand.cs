using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{

    [Header("General")]
    [SerializeField] Card baseCard;
    [SerializeField] List<Card> cards = new List<Card>();
    [SerializeField] Transform parent;
    static Vector3 VectorOne = new Vector3(1, 1, 1);

    public float zOrder = 0.01f;
    public int fullHand = 5;
    public int normalHand = 7;
   

     float handWidth = 1283f;
     float cardWidth = 170f;

    public float defaultSpace = 6;
    public float currentSpace = 6;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip in_clip;
    [SerializeField] AnimationClip out_clip;


    Card focus;

    void Start()
    {

    }

    public int Amount { get { return cards.Count; } }

    public void Add(Card card)
    {
        card.transform.SetParent(parent);
        card.transform.localScale = VectorOne;
        cards.Add(card);

        card.onEnter += Focus;
        card.onExit += UnFocus;
        card.onEndDrag += EndDrag;
        card.onStartDrag += StartDrag;

        card.handIndex = cards.IndexOf(card);
        card.gameObject.name = "Card " + card.handIndex;
        Rearrange();
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
        card.handIndex = -1;
        card.onEnter -= Focus;
        card.onExit -= UnFocus;
        card.onEndDrag -= EndDrag;
        card.onStartDrag -= StartDrag;
        Rearrange();
    }

    public void Rearrange()
    {
        var cardWidth = 170f;
        var totalCard = cards.Count;

        if(totalCard*cardWidth > handWidth)
        {
            var exceedWidth = handWidth - (totalCard * cardWidth);
            var space = exceedWidth / (totalCard - 1);
            currentSpace = space;
        }
        else
        {
            currentSpace = defaultSpace;
        }

        var screenWidth = 1920;
        var totalSpace = (currentSpace * (totalCard - 1));
        var totalCardWidth = (cardWidth * totalCard);
        var frontPosition = - (screenWidth*0.5f - (screenWidth - totalSpace - totalCardWidth) * 0.5f);
        var startPosition = frontPosition + (cardWidth*0.5f);
        //Debug.Log(startPosition);

        for (int i = 0; i < cards.Count; i++)
        {
          
            cards[i].handPosition = new Vector2(startPosition + ((cardWidth + currentSpace) * i), parent.position.y);
            cards[i].Rect.anchoredPosition = cards[i].handPosition;
            cards[i].transform.SetSiblingIndex(cards[i].handIndex);
        }

        // horizontalLayoutGroup.enabled = false;
        //horizontalLayoutGroup.enabled = true;

        //var startSpot = 
        //Debug.Log(cardWidth);

        //for (int i = 0; i < cards.Count; i++)
        //{
        //    //cards[i].transform.position = new zOrder * i;
        //    cards[i].positionInHand = i;
        //}

    }

    public void Return(Card card)
    {
        cards.Insert(card.handIndex, card);
        Rearrange();
    }

    public void Focus(Card card)
    {
        if (focus!=null && focus != card)
        {
            UnFocus(focus);
        }
        focus = card;

        if (currentSpace < 0)
        {
            RectTransform rect = card.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x -(currentSpace), rect.sizeDelta.y);
        }
        card.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        //card.transform.localPosition.Set(card.transform.localPosition.x, card.transform.localPosition.y + 20, card.transform.localPosition.z);
        card.Rect.anchoredPosition = new Vector2(card.Rect.anchoredPosition.x, card.Rect.anchoredPosition.y + 50);

    }

    public void UnFocus(Card card)
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(170f, rect.sizeDelta.y);
        card.transform.localScale = VectorOne;
        //card.transform.localPosition.Set(card.transform.localPosition.x, card.transform.localPosition.y - 20, card.transform.localPosition.z);
        card.Rect.anchoredPosition = card.handPosition;
        if (focus == card) focus = null;
        else Debug.LogError("WRONG");
    }
    
    public void StartDrag(Dragable dragable)
    {
        if (dragable is Card)
        {
            Card card = dragable as Card;
            card.transform.SetAsLastSibling();
            //card.onExit -= UnFocus;
        }
    }

    public void EndDrag(Dragable dragable)
    {
        if (dragable is Card)
        {
            Card card = dragable as Card;
            card.Rect.anchoredPosition = card.handPosition;
            card.transform.SetSiblingIndex(card.handIndex);
            //card.onExit += UnFocus;
        }
    }

    public void Show(Action onComplete = null)
    {
        StartCoroutine(ieShow(onComplete));
    }

    public void Hide(Action onComplete = null)
    {
        Debug.Log("Hand hide");
        StartCoroutine(ieHide(onComplete));
    }

    IEnumerator ieShow(Action onComplete = null)
    {
        animator.SetTrigger("in");
        yield return new WaitForSeconds(in_clip.length);
        onComplete?.Invoke();
    }

    IEnumerator ieHide(Action onComplete = null)
    {
        animator.SetTrigger("out");
        yield return new WaitForSeconds(out_clip.length);
        onComplete?.Invoke();
    }

    public void DestroyAll()
    {
        foreach(var card in cards)
        {
            Destroy(card.gameObject);
        }

        cards = new List<Card>();
    }
}
