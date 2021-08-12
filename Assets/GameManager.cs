using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static PlayerData playerData = new PlayerData();
    bool isPlaying = false;

    [Header("Card")]

    [SerializeField] Card baseCard;
    [SerializeField] Hand hand;
    [SerializeField] List<IngredientData> deck;
    [SerializeField] List<IngredientData> shrine;
    [SerializeField] int maxHand = 5;

    [ReadOnly] [SerializeField] Dragable currentDragable;
    [ReadOnly] [SerializeField] DragOnSpot dragOnSpot;

    [Header("Level")]

    [SerializeField] LevelData levelData;
    [SerializeField] List<RequestData> allRequests;
    [SerializeField] List<RequestBoard> requests;
    [SerializeField] List<RequestData> completeRequests;
    [SerializeField] List<RequestData> failRequests;

    [SerializeField] int customerPerWave = 3;
    bool isStart = false;

    [Header("Cheat cards")]
    [SerializeField] CheatCard ratCard;

    [Header("Discard")]
    [SerializeField] DiscardSpot discardSpot;

    [Header("Utensil")]
    [SerializeField] KitchenTool pan;

    [Header("UI")]
    [SerializeField] ShopOpenUI shopOpenUI;
    [SerializeField] ResultUI resultUI;
    [SerializeField] MoneyUI moneyUI;

    private void Awake()
    {
        shopOpenUI.onOpen += StartGame;
        shopOpenUI.onClose += Close;
    }

    private void Start()
    {
        deck.Shuffle();
        CreateAllRequest();
        InitKitchenTool(pan);
        InitDiscardSpot();
        moneyUI.UpdateText(playerData.money);

        for (int i = 0; i < requests.Count; i++)
        {
            InitRequest(requests[i]);
            requests[i].Active(false);
        }
        //openShopBtn.gameObject.SetActive(true);
        shopOpenUI.ActiveOpenButton(true);

    }

    void StartGame()
    {
        InitCheatCard(ratCard);

        shopOpenUI.ActiveOpenButton(false);
        shopOpenUI.ActiveCloseButton(true);
        for (int i = 0; i < maxHand; i++)
        {
            DrawCard();
        }

        moneyUI.Show(()=> 
        {
            hand.Show(ShowStartRequest);
        });

        isPlaying = true;
    }

    void ShowStartRequest()
    {
        for (int i = 0; i < requests.Count; i++)
        {
            requests[i].Init(allRequests[0]);
            allRequests.RemoveAt(0);
            requests[i].Show();
        }
    }

    void InitDiscardSpot()
    {
        discardSpot.onEnterDrag += (spot) => {
            if (currentDragable != null)
            {
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    dragOnSpot = spot;
                    discardSpot.Focus(currentDragable);
                    card.Active(false);
                }
            }
        };

        discardSpot.onExitDrag += (spot) => {
            if (currentDragable != null)
            {
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    dragOnSpot = null;
                    spot.UnFocus();
                    card.Active(true);
                }
            }
        };

        discardSpot.onExecute += (Dragable dragableObject)=>
        {
            if (dragableObject is Card)
            {
                Card card = dragableObject as Card;
                discardSpot.UnFocus();
                hand.Remove(card);
                ConsumeCard(card);
                Debug.Log("Execute complete");
            }
            //else if (dragableObject is CheatCard)
            //{
            //    CheatCard card = dragableObject as CheatCard;
            //    discardSpot.UnFocus();
            //    ConsumeCheatCard(card);
            //}
        };

    }

    //void ConsumeCheatCard(CheatCard card)
    //{
    //    card.onDragRelease += (g) =>
    //    {
    //        if (dragOnSpot != null)
    //        {
    //            dragOnSpot.Execute(card);
    //        }

    //    };
    //}


    void InitKitchenTool(KitchenTool tool)
    {
        tool.onEnterDrag += (spot) => {
            if (!tool.IsOccupied && currentDragable != null)
            {
                dragOnSpot = spot;
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    if (tool.processMenu.TryGetValue(card.ingredientData, out var pack))
                    {
                        tool.processIngredient.Set(pack.data, pack.amount);
                        tool.processIngredient.gameObject.SetActive(true);
                        spot.Focus(currentDragable);
                        card.Active(false);
                    }
                }
                else if(currentDragable is CheatCard)
                {
                    var card = currentDragable as CheatCard;
                    if (tool.processMenu.TryGetValue(card.ingredientData, out var pack))
                    {
                        tool.processIngredient.Set(pack.data, pack.amount);
                        tool.processIngredient.gameObject.SetActive(true);
                        spot.Focus(currentDragable);
                        card.Active(false);
                    }
                    Debug.Log("Try using cheat card");
                }
            }
        };

        tool.onExitDrag += (spot) => {
            if (currentDragable != null)
            {
                if (spot == dragOnSpot) dragOnSpot = null;
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    spot.UnFocus();
                    card.Active(true);
                }
                else if (currentDragable is CheatCard)
                {
                    var card = currentDragable as CheatCard;
                    spot.UnFocus();
                    card.Active(true);
                }
            }
        };

        tool.onExecuteComplete += (Card card) =>
        {
            tool.UnFocus();
            hand.Remove(card);
            ConsumeCard(card);
            Debug.Log("Execute card complete on TOOL");
        };

        tool.onExecuteCheatComplete += (CheatCard card) =>
        {
            tool.UnFocus();
            card.Deduct();
            card.ResetPosition();
            card.Active(true);
            Debug.Log("Execute cheat card complete on TOOL");
        };

        tool.onProcessComplete += (Pack pack) => 
        {
            ////Add that amount of card to hand
            //for (int i = 0; i < pack.amount; i++)
            //{
            //    var newCard = CreateCard(pack.data);
            //    newCard.SetType(CardType.Spoil);
            //    AddToHand(newCard);
            //}
        };

        tool.onClickToRecieveCard += (CardData cardData) =>
        {
            var newCard = CreateCard(cardData.ingredient, cardData.modifiers);
            newCard.SetType(CardType.Spoil);
            AddToHand(newCard);
        };

    }

    void ConsumeCard(Card card)
    {
       Destroy(card.gameObject);
    }

    void InitRequest(RequestBoard r)
    {
        r.onEnterDrag += (spot) => {
            if (currentDragable != null)
            {
                dragOnSpot = spot;
                spot.Focus(currentDragable);
            }
        };

        r.onExitDrag += (spot) => {
            if (currentDragable != null)
            {
                if (spot == dragOnSpot) dragOnSpot = null;
                spot.UnFocus();
            }
        };


        r.onExecuteComplete += (Dragable dragableObject) =>
        {
            if (dragableObject is Card)
            {
                Card card = dragableObject as Card;
                Debug.Log("<color=green>Use card</color> " + card.name + " [ " + card.ingredientData.Name + "] on " + r.name);
                hand.Remove(card);
                ConsumeCard(card);

                if (r.IsComplete())
                {
                    r.CompleteRequest();
                }
            }
            //else if (dragableObject is CheatCard)
            //{
            //    CheatCard card = dragableObject as CheatCard;  
            //    ConsumeCheatCard(card);
            //}
        };

        r.onExecuteFail += (Dragable dragableObject) =>
        {
            if (dragableObject is Card)
            {
                Card card = dragableObject as Card;
                Debug.Log("<color=red>fail to use card</color> " + card.name + "on " + r.name);
            }
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
        playerData.money += requestBoard.RequestData.menu.basePrice;
        moneyUI.UpdateText(playerData.money);
        if(isPlaying) NextCustomer(requestBoard);
    }

    void FailRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] FailRequest");
        failRequests.Add(requestBoard.RequestData);
        if (isPlaying) NextCustomer(requestBoard);
    }

    public Card CreateCard(IngredientData data , List<ModifierData> modifiers = null)
    {
        Card card = Instantiate<Card>(baseCard);
        card.Init(data, modifiers);

        card.onStartDrag += (c) => 
        { 
            currentDragable = card; 
        };

        card.onEndDrag += (c) => 
        {
            if (currentDragable == card) currentDragable = null; 
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

    public bool TryDrawCard(out Card card)
    {
        if (deck.Count > 0)
        {
            card = CreateCard(deck[0]);
            AddToHand(card);
            deck.RemoveAt(0);
            return true;
        }
        else
        {
            card = null;
            Debug.LogError("No card");
            return false;
        }
    }

    public void TryPlayCard(Card card , DragOnSpot spot)
    {
        spot.Execute(card);

        //Auto Draw card
        if(hand.Amount< maxHand)
        {
            int amountToDraw = maxHand - hand.Amount;

            for (int i = 0; i < amountToDraw; i++)
            {
                if (TryDrawCard(out var newCard))
                {
                    Debug.Log("Draw " + newCard.ingredientData.Name);
                }
                else
                {
                    Debug.Log("<color=red>OUT OF CARD</color>");
                    break;
                }
            }
        }
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
                requests[i].Show();
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
        Debug.Log("Call Next Customer");

        if (allRequests.Count == 0)
        {
            if (IsLevelComplete()) CompleteLevel();
            return;
        }

        requestBoard.Init(allRequests[0]);
        requestBoard.Show();
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
        shopOpenUI.ActiveCloseButton(false);
        EndGame();
    }

    void CalculateResult()
    {
        if(playerData.money > levelData.goalMoney)
        {
            Debug.Log("<color=green>Win!!</color> : You got money " + playerData.money + " goal is " + levelData.goalMoney);
        }
        else
        {
            Debug.Log("<color=red>Lose!!</color> : You got money " + playerData.money + " goal is " + levelData.goalMoney);
        }
    }

    void EndGame()
    {
        CalculateResult();
        isPlaying = false;
        foreach (var r in requests)
        {
            if (!r.isComplete)
            {
                r.FailRequest();
            }
            else
            {
                r.Hide();
            }
        }

        hand.Hide();

        ShowResultUI();
    }

    void ShowResultUI()
    {
        resultUI.Show();
        Debug.Log("Show result");

    }

    #endregion


    public void InitCheatCard(CheatCard cheatCard)
    {
        cheatCard.Init();
        cheatCard.onStartDrag += (c) =>
        {
            currentDragable = cheatCard;
        };

        cheatCard.onEndDrag += (c) =>
        {
            if (currentDragable == cheatCard) currentDragable = null;
        };

        cheatCard.onDragRelease += (g) =>
        {
            if (dragOnSpot != null)
            {
                Debug.Log("Execute drag release");
                dragOnSpot.Execute(cheatCard);
            }

        };
    }

}
