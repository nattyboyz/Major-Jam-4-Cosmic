using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Card baseCard;
    [SerializeField] Hand hand;
    [SerializeField] List<IngredientData> deck;

    [SerializeField] Card currentDragCard;
    [SerializeField] DragOnSpot dragOnSpot;


    [SerializeField] List<RequestBoard> requests;
    [SerializeField] List<RequestData> completeRequests;
    [SerializeField] List<RequestData> failRequests;

    [SerializeField] KitchenTool pan;

    public Card CurrentDragCard { get => currentDragCard; }

    private void Start()
    {
        InitKitchenTool(pan);

        foreach (RequestBoard r in requests)
        {
            InitRequest(r);
        }
    }

    void InitKitchenTool(KitchenTool tool)
    {
        tool.onEnterDrag += (spot) => {
            if (currentDragCard != null)
            {
                dragOnSpot = spot;
                if (tool.processMenu.TryGetValue(currentDragCard.ingredientData, out var pack))
                {
                    tool.processIngredient.Set(pack.data, pack.amount);
                    tool.processIngredient.gameObject.SetActive(true);
                    spot.Focus(currentDragCard);
                    currentDragCard.Active(false);
                }
            }
        };

        tool.onExitDrag += (spot) => {
            if (currentDragCard != null)
            {
                if (spot == dragOnSpot) dragOnSpot = null;
                spot.UnFocus();
                currentDragCard.Active(true);
            }
        };

        tool.onExecuteComplete += (Card card) =>
        {
            tool.UnFocus();
            hand.Remove(card);
            ConsumeCard(card);
            Debug.Log("Execute complete");
        };

        tool.onProcessComplete += (Pack pack) => 
        {
            for (int i = 0; i < pack.amount; i++)
            {
                var newCard = CreateCard(pack.data);
                newCard.SetType(CardType.Spoil);
                AddToHand(newCard);
            }
        };

    }

    void ConsumeCard(Card card)
    {
       Destroy(card.gameObject);
    }

    void InitRequest(RequestBoard r)
    {
        r.onEnterDrag += (spot) => {
            if (currentDragCard != null)
            {
                dragOnSpot = spot;
                spot.Focus(currentDragCard);
            }
        };

        r.onExitDrag += (spot) => {
            if (currentDragCard != null)
            {
                if (spot == dragOnSpot) dragOnSpot = null;
                spot.UnFocus();
            }
        };

        r.onExecuteComplete += (card) =>
        {
            Debug.Log("<color=green>Use card</color> " + card.name + "on " + r.name);
            hand.Remove(card);
            ConsumeCard(card);

            if (r.IsComplete())
            {
                r.CompleteRequest();
            }
        };

        r.onExecuteFail += (card) =>
        {
            Debug.Log("<color=red>fail to use card</color> " + card.name + "on " + r.name);
        };

        r.onCompleteRequest += () => 
        {
            CompleteRequest(r.RequestData);
        };

        r.onFailRequest += () =>
        {
            FailRequest(r.RequestData);         
        };
    }

    void CompleteRequest(RequestData requestData)
    {
        Debug.Log("[GameManager] CompleteRequest");
        completeRequests.Add(requestData);
    }

    void FailRequest(RequestData requestData)
    {
        Debug.Log("[GameManager] FailRequest");
        failRequests.Add(requestData);
    }

    void InitPan()
    {

    }

    public void StartGame()
    {
      
    }

    public Card CreateCard(IngredientData data)
    {
        Card card = Instantiate<Card>(baseCard);
        card.Set(data);

        card.onStartDrag += (c) => 
        { 
            currentDragCard = card; 
        };

        card.onEndDrag += (c) => 
        {
            if (currentDragCard == card) currentDragCard = null; 
        };

        return card;
    }

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            var card = CreateCard(deck[0]);
            AddToHand(card);
            deck.RemoveAt(0);
        }
        else
        {
            Debug.LogError("No card");
        }
    }

    public void TryPlayCard(Card card ,DragOnSpot spot)
    {
        spot.Execute(card);
    }

    public void AddToHand(Card card)
    {

        //card.onDragRayUpdate += (g) =>
        //{
        //    if(g.TryGetComponent< DragOnSpot >(out var spot))
        //    {
        //        //dragOnSpot = spot;
        //        //spot.Focus(card);
        //        //Debug.Log(g);
        //    }
        //};

        card.onDragRelease += (g) =>
        {
            if (dragOnSpot != null)
            {
                TryPlayCard(card, dragOnSpot);
            }

        };

        hand.Add(card);
    }

    public void RemoveCardFromHand(Card card)
    {
        hand.Remove(card);
    }

}
