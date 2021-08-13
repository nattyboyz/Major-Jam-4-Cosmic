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
    int currentCustomerIndex = 0;

    [Header("Cheat cards")]
    [SerializeField] CheatCard ratCard;

    [Header("Discard")]
    [SerializeField] DiscardSpot discardSpot;

    [Header("Utensil")]
    [SerializeField] KitchenTool pan;
    [SerializeField] KitchenTool knife;

    [Header("UI")]
    [SerializeField] ShopOpenUI shopOpenUI;
    [SerializeField] ResultUI resultUI;
    [SerializeField] MoneyUI moneyUI;
    [SerializeField] PenaltyUI penaltyUI;
    [SerializeField] CustomerLeftUI customerLeftUI;

    private void Awake()
    {
        shopOpenUI.onOpen += StartGame;
        shopOpenUI.onClose += Close;
    }

    private void Start()
    {
        deck.Shuffle();
        penaltyUI.SetStar(playerData.star);
        CreateAllRequest();
        InitKitchenTool(pan);
        InitKitchenTool(knife);
        InitDiscardSpot();
        moneyUI.UpdateText(playerData.money);

        for (int i = 0; i < requests.Count; i++)
        {
            InitRequestBoard(requests[i]);
            requests[i].Active(false);
        }
        //openShopBtn.gameObject.SetActive(true);
        shopOpenUI.ActiveOpenButton(true);

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

    void InitKitchenTool(KitchenTool tool)
    {
        tool.onEnterDrag += (spot) => {
            if (!tool.IsOccupied && currentDragable != null)
            {
                dragOnSpot = spot;
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    if (tool.processMenu.TryGetValue(card.cardData.ingredient, out var pack))
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

    void InitRequestBoard(RequestBoard r)
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
                Debug.Log("<color=green>Use card</color> " + card.name + " [ " + card.cardData.ingredient.Name + "] on " + r.name);
                hand.Remove(card);
                ConsumeCard(card);

                if (r.IsComplete())
                {
                    float hp = 100;
                    foreach(var setting in r.Settings)
                    {
                        if (setting.CardData == null) Debug.LogError("Card data is NULL");
                        if (setting.CardData.modifiers != null)
                        {
                            foreach (var modifier in setting.CardData.modifiers)
                            {
                                if (hp + modifier.QualityValue < 0) hp = 0;
                                else if (hp + modifier.QualityValue > 100) hp = 100;
                                else hp += modifier.QualityValue;
                            }
                        }
                    }

                    float n = Random.Range(0f, 100f);
                    if (n < hp)
                    {
                        Debug.Log("<color=green><b>Pass</b></color>");
                        r.CompleteRequest();
                    }
                    else
                    {
                        Debug.Log("<color=red><b>Fail</b></color>");
                        r.FailRequest();

                        //Deduct star
                        if (playerData.star - 1 < 0) 
                        {
                            playerData.star = 0;
                            penaltyUI.SetStar(playerData.star);
                            Gameover();
                        }
                        else
                        {
                            playerData.star--;
                            penaltyUI.SetStar(playerData.star);
                        }
                        //

                    }

                    Debug.Log("This food have percent chance to pass = " + n + " from " + + hp);
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
                Debug.Log("<color=red>fail to use card</color> " + card.name + " on " + r.name);
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

    #region Request Board

    void CompleteRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] CompleteRequest");
        completeRequests.Add(requestBoard.RequestData);
        playerData.money += requestBoard.RequestData.Menu.basePrice;
        moneyUI.UpdateText(playerData.money);
        if(isPlaying) NextCustomer(requestBoard);
    }

    void FailRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] FailRequest");
        failRequests.Add(requestBoard.RequestData);
        

        if (isPlaying) NextCustomer(requestBoard);
    }

    #endregion

    #region CardPlay

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
                    Debug.Log("Draw " + newCard.cardData.ingredient.Name);
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

    void ConsumeCard(Card card)
    {
        Destroy(card.gameObject);
    }

    #endregion

    #region Gameloop

    void StartGame()
    {
        InitCheatCard(ratCard);
        currentCustomerIndex = 0;
        shopOpenUI.ActiveOpenButton(false);
        shopOpenUI.ActiveCloseButton(true);
        for (int i = 0; i < maxHand; i++)
        {
            DrawCard();
        }

        customerLeftUI.Show();
        penaltyUI.Show();
        moneyUI.Show(() =>
        {
            hand.Show(ShowStartRequest);
        });

        isPlaying = true;
    }

    void ShowStartRequest()
    {
        for (int i = 0; i < requests.Count; i++)
        {
            NextCustomer(requests[i]);
        }
    }

    public void AddRandomCustomer()
    {
        var index = Random.Range(0, levelData.Menus.Count);
        RequestData requestData = new RequestData();
        requestData.Menu = levelData.Menus[index];


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
        int maxWave = levelData.Customer;
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
        StartCoroutine(NextCustomer(requestBoard, Random.Range(1, 5)));
    }

    IEnumerator NextCustomer(RequestBoard requestBoard, float delay)
    {
        requestBoard.Init(allRequests[0]);
        allRequests.RemoveAt(0);

        yield return new WaitForSeconds(delay);

        Debug.Log("Call Next Customer");
        customerLeftUI.Fade(currentCustomerIndex);
        currentCustomerIndex++;
        requestBoard.Show();

    }

    public void CreateAllRequest()
    {
        int total = levelData.Customer;
        int fixedCustomerAmount = levelData.FixedRequests.Count;
        if (fixedCustomerAmount > total) fixedCustomerAmount = total;
        int customerLeft = total - fixedCustomerAmount;

        customerLeftUI.SetStartCustomer(total);

        for (int i = 0; i < fixedCustomerAmount;i++)
        {
            allRequests.Add(levelData.FixedRequests[i]);
        }

        for (int i = 0; i < total; i++)
        {
            //Random menu
            var menuIndex = Random.Range(0, levelData.Menus.Count);
            var customerIndex = Random.Range(0, levelData.PossibleCustomers.Count);
            var menu = levelData.Menus[menuIndex];
            var customer = levelData.PossibleCustomers[customerIndex];
            //Create request data
            RequestData requestData = new RequestData();
            requestData.CustomerData = customer;
            requestData.Menu = menu;
            requestData.Time = menu.baseTime + customer.TimeModifier.GetRandomTime();
            requestData.Price = menu.basePrice;

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
        LevelEnd();
    }

    void CalculateResult()
    {
        if(playerData.money > levelData.GoalMoney)
        {
            Debug.Log("<color=green>Win!!</color> : You got money " + playerData.money + " goal is " + levelData.GoalMoney);
        }
        else
        {
            Debug.Log("<color=red>Lose!!</color> : You got money " + playerData.money + " goal is " + levelData.GoalMoney);
        }
    }

    void LevelEnd()
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

        customerLeftUI.Hide();
        penaltyUI.Hide();
        hand.Hide();
        moneyUI.Hide();

        ShowResult();
    }

    void ShowResult()
    {
        resultUI.Show();
        Debug.Log("Show result");

    }

    void Gameover()
    {
        Debug.Log("Game Over.");
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
