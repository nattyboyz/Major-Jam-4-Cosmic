using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Card")]

    [SerializeField] Card baseCard;
    [SerializeField] Hand hand;
    [SerializeField] List<IngredientData> deck;
    [SerializeField] List<IngredientData> shrine;
    [SerializeField] int maxHand = 5;

    Card currentDragCard;
    DragOnSpot dragOnSpot;

    [Header("Level")]

    [SerializeField] LevelData levelData;
    [SerializeField] List<RequestData> allRequests;
    [SerializeField] List<RequestBoard> requests;
    [SerializeField] List<RequestData> completeRequests;
    [SerializeField] List<RequestData> failRequests;

    [SerializeField] int customerPerWave = 3;
    bool isStart = false;

    [Header("Utensil")]
    [SerializeField] KitchenTool pan;

    [Header("UI")]
    [SerializeField] ResultUI resultUI;
    [SerializeField] Button openShopBtn;
    [SerializeField] Button closeShopBtn;

    private void Awake()
    {
        openShopBtn.onClick.AddListener(StartGame);
        closeShopBtn.onClick.AddListener(Close);
    }


    private void Start()
    {
        CreateAllRequest();
        InitKitchenTool(pan);

        for(int i = 0; i < requests.Count; i++)
        {
            InitRequest(requests[i]);
            requests[i].Active(false);
        }
        openShopBtn.gameObject.SetActive(true);
    }

    void StartGame()
    {
        openShopBtn.gameObject.SetActive(false);

        for (int i = 0; i < requests.Count; i++)
        {
            requests[i].Init(allRequests[0]);
            allRequests.RemoveAt(0);
            requests[i].Active(true);
        }

        for (int i = 0; i < maxHand; i++)
        {
            DrawCard();
        }

    }

    void InitKitchenTool(KitchenTool tool)
    {
        tool.onEnterDrag += (spot) => {
            if (!tool.IsProcessing() && currentDragCard != null)
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
            CompleteRequest(r);
        };

        r.onFailRequest += () =>
        {
            FailRequest(r);         
        };
    }

    void CompleteRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] CompleteRequest");
        completeRequests.Add(requestBoard.RequestData);
        NextCustomer(requestBoard);

    }

    void FailRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] FailRequest");
        failRequests.Add(requestBoard.RequestData);
        NextCustomer(requestBoard);
    }

    void InitPan()
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


    #region Level

    public void AddRandomCustomer()
    {
        var index = Random.Range(0, levelData.menus.Count - 1);
        RequestData requestData = new RequestData();
        requestData.customerType = CustomerType.Normal;
        requestData.menu = levelData.menus[index];


        for (int i =0;i< 3; i++)
        {
            if(requests[i].isComplete)
            {
                requests[i].Init(requestData);
                requests[i].Active(true);
                return;
            }
        }

        Debug.Log("Still reach maximum customer");
    }

    bool IsLevelComplete()
    {
        int maxWave = levelData.max_wave;
        int total = maxWave * customerPerWave;

        if (completeRequests.Count + failRequests.Count >= total)
        {
            return true;
        }

       return false;
    }

    void NextCustomer(RequestBoard requestBoard)
    {
        if (allRequests.Count == 0)
        {
            if (IsLevelComplete()) CompleteLevel();
            return;
        }

        requestBoard.Init(allRequests[0]);
        allRequests.RemoveAt(0);
    }

    public void CreateAllRequest()
    {
        int maxWave = levelData.max_wave;
        int total = maxWave * customerPerWave;

        for(int i = 0; i < total; i++)
        {
            var index = Random.Range(0, levelData.menus.Count - 1);
            RequestData requestData = new RequestData();
            requestData.customerType = CustomerType.Normal;
            requestData.menu = levelData.menus[index];

            allRequests.Add(requestData);
        }
    }

    void CompleteLevel()
    {
        Debug.Log("CompleteLevel");
    }

    void Close()
    {
        Debug.Log("Close");
    }

    void ShowResultUI()
    {
        Debug.Log("Show result");

    }

    #endregion


}
