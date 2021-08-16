using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData = new PlayerData();
    [SerializeField] PlayerDataScriptableObject playerDataScriptableObject;
    bool isPlaying = false;

    [SerializeField] int spyCost = 30;

    [Header("Card")]
    [SerializeField] Card baseCard;
    [SerializeField] Hand hand;
    [SerializeField] List<CardData> deck;
    [SerializeField] int maxHand = 5;
    [SerializeField] TMPro.TextMeshProUGUI textTotalCardInDeck;

    [Header("Cheat Card")]
    [SerializeField] CheatCard baseCheatCard;
    [SerializeField] CheatCardCarbinetUI cheatCardCabinet;
    [SerializeField] List<CheatCard> cheatCards;

    [ReadOnly] [SerializeField] Dragable currentDragable;
    [ReadOnly] [SerializeField] DragOnSpot dragOnSpot;

    [Header("Level")]

    [ReadOnly] [SerializeField] int currentLevel = 0;
    [SerializeField] List<LevelData> levelDatas;
    [SerializeField] LevelData currentLevelData;
    [SerializeField] CustomerData fallbackCustomer;
    [SerializeField] float minDurNextCustomer = 1;
    [SerializeField] float maxDurNextCustomer = 10;

    [Header("Request")]
    [SerializeField] List<RequestData> allRequests;
    [SerializeField] List<RequestBoard> requests;
    [SerializeField] List<RequestData> completeRequests;
    [SerializeField] List<RequestData> failRequests;

    [SerializeField] int customerPerWave = 3;
    int currentCustomerIndex = 0;

    [Header("Cheat cards")]
    [SerializeField] CheatCard ratCard;

    [Header("Discard")]
    [SerializeField] DiscardSpot discardSpot;
    [SerializeField] DeckSpot deckSpot;

    [Header("Utensil")]
    [SerializeField] List<KitchenTool> kitchenTools;

    [Header("UI")]
    [SerializeField] Canvas shopCanvas;
    [SerializeField] ShopOpenUI shopOpenUI;
    [SerializeField] ResultUI resultUI;
    [SerializeField] MoneyUI moneyUI;
    [SerializeField] PenaltyUI penaltyUI;
    [SerializeField] CustomerLeftUI customerLeftUI;
    [SerializeField] CloseShopUI shopCloseUI;
    [SerializeField] BuyUI buyUI;
    [SerializeField] GameOverUI gameOverUI;

    Dictionary<RequestBoard,Coroutine> customerCoroutine = new Dictionary<RequestBoard, Coroutine>();

    [Header("Ingredient")]
    [SerializeField] CustomerType veganType;
    [SerializeField] CustomerType meatLoverType;
    [SerializeField] CustomerType celebType;
    [SerializeField] CustomerType rusherType;

    [SerializeField] IngredientData meat;
    [SerializeField] IngredientData veggie;

    private void Awake()
    {
        if (playerDataScriptableObject != null) playerData = new PlayerData(playerDataScriptableObject);
        shopOpenUI.onOpen += Open;
        shopCloseUI.onClose += LevelEnd;

        
        buyUI.onBuy += BuyIngredient;
        buyUI.onSell += SellIngredient;
        buyUI.onStartShow += () => { buyUI.UpdateShopInventory(playerData); };
        buyUI.Init(playerData);


        gameOverUI.onClickOverBtn += () => {
            SceneManager.LoadScene("GameScene");
        };

        for (int i = 0; i < requests.Count; i++)
        {
            InitRequestBoard(requests[i]);
            requests[i].Active(false);
        }

        resultUI.onNext += NextLevel;
        resultUI.onHome += ToTitle;
        resultUI.onRetry += () => {
            SceneManager.LoadScene("GameScene");
        };

        InitCheatCards();
        InitDeckSpot();
        InitDiscardSpot();
        InitKitchenTool();

        textTotalCardInDeck.transform.parent.gameObject.SetActive(false);

    }

    void BuyIngredient(CardBuySlotUI cardBuySlot)
    {
        if (TryModifyMoney(-cardBuySlot.IngredientData.BuyPrice))
        {
            int amount = 0;
            if (playerData.cheats.ContainsKey(cardBuySlot.IngredientData))
            {
                amount =  playerData.cheats[cardBuySlot.IngredientData] += 1;
                Debug.Log("Success buying cheats" + cardBuySlot.IngredientData.Name + " " + amount);

            }
            else if (playerData.ingredients.ContainsKey(cardBuySlot.IngredientData))
            {
                amount = playerData.ingredients[cardBuySlot.IngredientData] += 1;

                Debug.Log("Success buying ingredient " + cardBuySlot.IngredientData.Name + " " + amount);
            }
            else
            {
                //First item in inventory
                amount = playerData.ingredients[cardBuySlot.IngredientData] = 1;
            }
            cardBuySlot.UpdateAmount(amount);
        }
        else
        {
            Debug.Log("Fail buying " + cardBuySlot.IngredientData.Name + "No enough money");
        }
    }

    void SellIngredient(CardBuySlotUI cardBuySlot)
    {
        IngredientData target= cardBuySlot.IngredientData;
        int amount = 0;
        int sellPrice = 0;


        if (playerData.cheats.ContainsKey(target) && playerData.cheats[target] > 0)
        {
            amount = playerData.cheats[target] -= 1;
            sellPrice = cardBuySlot.IngredientData.SellPrice;
            Debug.Log("Success selling cheats" + target.Name + " " + amount);

        }
        else if (playerData.ingredients.ContainsKey(target) && playerData.ingredients[target]>0)
        {
            amount = playerData.ingredients[target] -= 1;
            sellPrice = cardBuySlot.IngredientData.SellPrice;
            Debug.Log("Success selling ingredient " + target.Name + " " + amount);
        }

        TryModifyMoney(+sellPrice);
        cardBuySlot.UpdateAmount(amount);

    }

    private void Start()
    {
        shopCanvas.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(true);

        hand.gameObject.SetActive(false);
        LoadLevel();
        hand.gameObject.SetActive(true);
    }

    void InitDeck()
    {
        deck = new List<CardData>();

        foreach(KeyValuePair<IngredientData,int> kvp in playerData.ingredients)
        {
            for(int i = 0; i < kvp.Value; i++)
            {
                CardData cardData = new CardData(kvp.Key, kvp.Key.Modifiers);
                deck.Add(cardData);
            }
        }

        deck.Shuffle();

        UpdateCardRemaining();
        textTotalCardInDeck.transform.parent.gameObject.SetActive(true);
    }

    void InitCheatCards()
    {
        foreach(var kvp in playerData.cheats)
        {
            CheatCard cheatCard = Instantiate(baseCheatCard);
            if (cheatCardCabinet.TryAddCheatCard(cheatCard))
            {
                CardData cardData = new CardData(kvp.Key, kvp.Key.Modifiers);
                cheatCard.Init(cardData);
                cheatCard.onStartDrag += (c) =>
                {
                    currentDragable = cheatCard;
                    OnCheatCardStartDrag(cheatCard);
                };

                cheatCard.onEndDrag += (c) =>
                {
                    if (currentDragable == cheatCard)
                    {
                        currentDragable = null;
                        OnCheatCardEndDrag(cheatCard);
                    }
                };

                cheatCard.onDragRelease += (g) =>
                {
                    if (dragOnSpot != null)
                    {
                        if (playerData.cheats.TryGetValue(cheatCard.cardData.ingredient, out var amount) && amount>0)
                        {
                            playerData.cheats[cheatCard.cardData.ingredient] -= 1;
                            cheatCard.SetAmount(playerData.cheats[cheatCard.cardData.ingredient]);
                            dragOnSpot.Execute(cheatCard);
                            cheatCard.ResetPosition();
                            cheatCard.Active(true);
                        }
                        else
                        {
                            dragOnSpot.UnFocus();
                            cheatCard.ResetPosition();
                            cheatCard.Active(true);
                            Debug.LogError("Not enough cheat item or not equip");
                        }
                    }
                };
                
                cheatCards.Add(cheatCard);
                cheatCard.SetAmount(playerData.cheats[cheatCard.cardData.ingredient]);
            }
            else
            {
                Destroy(cheatCard.gameObject);
            }
        }
    }

    List<DragOnSpot> focusedDragOnSpot = new List<DragOnSpot>();

    void OnCheatCardStartDrag(CheatCard cheatCard)
    {
        if (string.CompareOrdinal(cheatCard.cardData.ingredient.Key, "msg") == 0)
        {
            Debug.Log("MSG highlight.");
            foreach(RequestBoard requestBoard in requests)
            {
                requestBoard.Highlight();
                focusedDragOnSpot.Add(requestBoard);
            }
        }
        else if (string.CompareOrdinal(cheatCard.cardData.ingredient.Key, "rat") == 0)
        {
            Debug.Log("Rat highlight.");
            foreach (RequestBoard requestBoard in requests)
            {
                requestBoard.Highlight();
                focusedDragOnSpot.Add(requestBoard);
            }

            foreach(KitchenTool kitchenTool in kitchenTools)
            {
                if (kitchenTool.HasRecipe(cheatCard.cardData.ingredient))
                {
                    kitchenTool.Highlight();
                    focusedDragOnSpot.Add(kitchenTool);
                }
            }
        }
    }

    void OnCheatCardEndDrag(CheatCard cheatCard)
    {
        foreach(var spot in focusedDragOnSpot)
        {
            spot.UnHighlight();
        }
        focusedDragOnSpot.Clear();
    }

    void OnCardStartDrag(Card card)
    {

        foreach (RequestBoard requestBoard in requests)
        {
            if (requestBoard.RequiredIngredient(card.cardData.ingredient))
            {
                requestBoard.Highlight();
                focusedDragOnSpot.Add(requestBoard);
            }
        }

        foreach (KitchenTool kitchenTool in kitchenTools)
        {
            if (kitchenTool.HasRecipe(card.cardData.ingredient))
            {
                kitchenTool.Highlight();
                focusedDragOnSpot.Add(kitchenTool);
            }
        }

        discardSpot.Highlight();
        focusedDragOnSpot.Add(discardSpot);
        deckSpot.Highlight();
        focusedDragOnSpot.Add(deckSpot);
    }

    void OnCardEndDrag(Card card)
    {
        foreach (var spot in focusedDragOnSpot)
        {
            spot.UnHighlight();
        }
        focusedDragOnSpot.Clear();
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
            }
        };

    }

    void InitDeckSpot()
    {
        deckSpot.onEnterDrag += (spot) => {
            if (currentDragable != null)
            {
                if (currentDragable is Card)
                {
                    var card = currentDragable as Card;
                    dragOnSpot = spot;
                    deckSpot.Focus(currentDragable);
                    card.Active(false);
                }
            }
        };

        deckSpot.onExitDrag += (spot) => {
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

        deckSpot.onExecute += (Dragable dragableObject) =>
        {
            if (dragableObject is Card)
            {
                Card card = dragableObject as Card;
                deckSpot.UnFocus();
                deck.Add(card.cardData);
                hand.Remove(card);
                Destroy(card.gameObject);
                //ConsumeCard(card);

                UpdateCardRemaining();
            }
        };
    }

    void InitKitchenTool()
    {
        foreach (KitchenTool tool in kitchenTools)
        {
            tool.onEnterDrag += (spot) =>
            {
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
                    else if (currentDragable is CheatCard)
                    {
                        var card = currentDragable as CheatCard;
                        if (tool.processMenu.TryGetValue(card.cardData.ingredient, out var pack))
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

            tool.onExitDrag += (spot) =>
            {
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
                //Debug.Log("Execute card complete on TOOL");
            };

            tool.onExecuteCheatComplete += (CheatCard card) =>
            {
                tool.UnFocus();
                //card.ResetPosition();
                //card.Active(true);
                // Debug.Log("Execute cheat card complete on TOOL");
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
                var newCard = CreateCard(cardData.ingredient, cardData.modifiers, true);
                newCard.SetType(CardType.Spoil);
                AddToHand(newCard);
            };
        }
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
               // Debug.Log("<color=green>Use card</color> " + card.name + " [ " + card.cardData.ingredient.Name + "] on " + r.name);
                hand.Remove(card);
                ConsumeCard(card);

                if (r.IsComplete())
                {
                    CustomerData customer = r.RequestData.CustomerData;
                    float baseHp = 100;
                    float hp = 100;

                    foreach (var setting in r.Settings)
                    {
                        if (setting.CardData == null) Debug.LogError("Card data is NULL");
                        if (setting.CardData.modifiers != null)
                        {
                            foreach (var modifier in setting.CardData.modifiers)
                            {
                                //LEVEL DESIGN
                                if (modifier.Type == ModifierType.Curse)
                                {
                                    baseHp = hp = 90;
                                    break;
                                }
                            }
                        }
                    }

                    foreach (var setting in r.Settings)
                    {
                        if (setting.CardData == null) Debug.LogError("Card data is NULL");
                        if (setting.CardData.modifiers != null)
                        {
                            Debug.Log(setting.CardData.modifiers.Count);
                            foreach (var modifier in setting.CardData.modifiers)
                            {
                                var val = modifier.QualityValue;
                                if (modifier.Type== ModifierType.Curse)
                                {
                                    //LEVEL DESIGN
                                    if (string.CompareOrdinal(customer.CustomerType.Name, "Delivery Guy") ==0)
                                    {
                                        val = 0;
                                        Debug.Log("Delivery guy set value to 0");
                                    }
                                    else
                                    {
                                        Debug.Log(setting.CardData.ingredient.Key + " " + modifier.Key);
                                        float add = GetCurseModifierBaseOnCustomer(customer.CustomerType.Name, setting.Ingredient.Key);
                                        val += add;
                                        Debug.Log(customer.CustomerType.Name + "add value " + add);
                                    }
                                }
                                else
                                {

                                }

                                if (hp + val < 0) hp = 0;
                                else if (hp + val > 100) hp = 100;
                                else hp += val;
                            }
                        }
                    }

                    float n = Random.Range(0f, 100f);
                    if (n < hp)
                    {
                        Debug.Log("<color=green><b>Pass</b></color>");
                        var price = r.RequestData.Menu.basePrice;
                        TryModifyMoney(price);
                        r.ShowMoney(price);
                        completeRequests.Add(r.RequestData);
                        r.Success("", ()=> { CompleteRequest(r); });

                        if (r.RequestData.StarReward > 0)
                        {
                            if (playerData.star + r.RequestData.StarReward <= playerData.maxStar)
                            {
                                Debug.Log("Get Star " + r.RequestData.StarReward);
                                penaltyUI.SetStar(playerData.star += r.RequestData.StarReward);
                            }
                        }   
                    }
                    else
                    {
                        Debug.Log("<color=red><b>Caught</b></color>");
                        //var price = r.RequestData.Menu.basePrice;
                        //ModifyMoney(price);
                        //r.ShowMoney(price);
                        r.ShowFail(-1);
                        failRequests.Add(r.RequestData);
                        r.Fail("", () => { FailRequest(r); });
                        //FailRequest(r);
                        //Deduct star
                        if (playerData.star - 1 <= 0) 
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
                    Debug.Log("This food have percent chance to pass = " + hp + " you roll-> " + n);
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
            //Debug.Log("<color=red><b>Fail</b></color>");
            if (dragableObject is Card)
            {
                Card card = dragableObject as Card;
                Debug.Log("<color=red>fail to use card</color> " + card.name + " on " + r.name);
            }
        };

        r.onTimeout += () => 
        {
            Debug.Log("<color=red><b>Timout</b></color>");
            failRequests.Add(r.RequestData);
            FailRequest(r);
            r.Timeout();
        };

        //r.onCompleteRequest += () => 
        //{
        //   // CompleteRequest(r);
        //};

        //r.onFailRequest += () =>
        //{
        //    //FailRequest(r);         
        //};
    }

    //LEVEL DESIGN
    public float GetCurseModifierBaseOnCustomer(string customerKey, string ingredientKey)
    {
        if (customerKey == "Meat Lover")
        {
            if (ingredientKey == "steak") return -5;
        }
        else if (customerKey == "Vegan")
        {
            if (ingredientKey == "veggie") return -5;
        }
        else if (customerKey == "Inspector")
        {
            return -6;
        }
        else if (customerKey == "Bypasser")
        {
            return 5;
        }
        else if (customerKey == "Rusher")
        {
            return 2;
        }
        else if (customerKey == "Celebrity")
        {
            return -2;
        }

        return 0;
    }

    #region Request Board

    bool TryModifyMoney(int amount)
    {
        if (playerData.money + amount >= 0)
        {
            playerData.money += amount;
            moneyUI.UpdateText(playerData.money);
            moneyUI.Success();
            return true;
        }
        moneyUI.Fail();
        return false;
    }

    void CompleteRequest(RequestBoard requestBoard)
    {
        if (isPlaying && IsLevelComplete())
        {
            LevelEnd();
        }
        else
        {
            NextCustomer(requestBoard);
        }
    }

    void FailRequest(RequestBoard requestBoard)
    {
        Debug.Log("[GameManager] FailRequest");
        if (isPlaying && IsLevelComplete())
        {
            LevelEnd();
        }
        else
        {
            NextCustomer(requestBoard);
        }
    }

    #endregion

    #region CardPlay

    public Card CreateCard(IngredientData data , List<ModifierData> modifiers = null,bool isSpoil = false)
    {
        CardData cardData = new CardData(data, modifiers, isSpoil);
        Card card = CreateCard(cardData);
        return card;
    }

    public Card CreateCard(CardData cardData)
    {
        Card card = Instantiate<Card>(baseCard);
        card.Init(cardData);

        card.onStartDrag += (c) =>
        {
            currentDragable = card;
            OnCardStartDrag(card);
        };

        card.onEndDrag += (c) =>
        {
            if (currentDragable == card)
            {
                currentDragable = null;
                OnCardEndDrag(card);
            }  
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

            UpdateCardRemaining();
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

            UpdateCardRemaining();
            return true;
        }
        else
        {
            card = null;
            //Debug.LogError("No card");
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
                    //Debug.Log("[GMng][hand] Draw " + newCard.cardData.ingredient.Name);
                }
                else
                {
                    //Debug.Log("[GMng][hand] <color=red>OUT OF CARD</color>");
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
        if (!card.cardData.isSpoil)
        {
            if (playerData.ingredients.TryGetValue(card.cardData.ingredient, out var amount))
            {
                playerData.ingredients[card.cardData.ingredient] -= 1;
            }
            else
            {
                Debug.LogError("No ingredient left!!!!!!!!!!!!!!!!!!!");
            }
        }
        else
        {
            Debug.Log("Consume spoil card");
        }

        Destroy(card.gameObject);
    }

    public void UpdateCardRemaining()
    {
        textTotalCardInDeck.text = ""+deck.Count;
    }

    #endregion

    #region Gameloop

    void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void NextLevel()
    {
        if(currentLevel+1>= levelDatas.Count)
        {
            Debug.LogError("No level");
        }
        else
        {
            currentLevel++;
            LoadLevel();
        }
    }

    void LoadLevel()
    {
        hand.DestroyAll();
        currentLevelData = levelDatas[currentLevel];
        CreateAllRequest();
        penaltyUI.SetStar(playerData.star);
        moneyUI.UpdateText(playerData.money);

        shopOpenUI.SetDay("DAY " + (currentLevel + 1));
        shopOpenUI.SetGoal("Goal: $" + currentLevelData.GoalMoney);
        moneyUI.UpdateGoal(currentLevelData.GoalMoney);
        shopOpenUI.ActiveButton(true);
        buyUI.Show(()=> { });
    }

    void StartGame()
    {
        InitDeck();

        currentCustomerIndex = 0; 
        for (int i = 0; i < maxHand; i++)
        {
            DrawCard();
        }

        shopCloseUI.Show(() => {
            customerLeftUI.Show(() => {
                    hand.Show(()=> {
                        isPlaying = true;
                        ShowStartRequest();
                        }
                    ); 
                });
            }
        );
        //penaltyUI.Show();
        //moneyUI.Show();
   
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
        var index = Random.Range(0, currentLevelData.Menus.Count);
        RequestData requestData = new RequestData();
        requestData.Menu = currentLevelData.Menus[index];

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
        int total = currentLevelData.Customer;
        if (completeRequests.Count + failRequests.Count >= total)
        {
            return true;
        }
       return false;
    }

    void NextCustomer(RequestBoard requestBoard)
    {
        if (allRequests.Count > 0)
        {
            float tempMax = maxDurNextCustomer;

            if (allRequests.Count == currentLevelData.Customer)
                tempMax = minDurNextCustomer;

            customerCoroutine[requestBoard] = StartCoroutine(NextCustomer(requestBoard, currentCustomerIndex, Random.Range(minDurNextCustomer, tempMax)));
            currentCustomerIndex++;
        }
    }

    IEnumerator NextCustomer(RequestBoard requestBoard,int index, float delay)
    {
        requestBoard.Init(allRequests[0]);
        allRequests.RemoveAt(0);

        yield return new WaitForSeconds(delay);

        Debug.Log("Call Next Customer " + index);       
        customerLeftUI.Fade(index);
        requestBoard.isComplete = false;
        requestBoard.Show();
    }

    public void CreateAllRequest()
    {
        currentLevelData.PossibleCustomers.RemoveAll(item => item == null);

        allRequests = new List<RequestData>();
        failRequests = new List<RequestData>();
        completeRequests = new List<RequestData>();

        int total = currentLevelData.Customer;
        int fixedCustomerAmount = currentLevelData.FixedRequests.Count;
        if (fixedCustomerAmount > total) fixedCustomerAmount = total;
        int customerLeft = total - fixedCustomerAmount;

        customerLeftUI.SetStartCustomer(total);

        for (int i = 0; i < fixedCustomerAmount;i++)
        {
            allRequests.Add(CreateRequestData(currentLevelData.FixedRequests[i]));
        }

        for (int i = 0; i < customerLeft; i++)
        {
            //Random menu
            var menuIndex = Random.Range(0, currentLevelData.Menus.Count);
            var customerIndex = Random.Range(0, currentLevelData.PossibleCustomers.Count);
            var menu = currentLevelData.Menus[menuIndex];
            var customer = fallbackCustomer;
            if(currentLevelData.PossibleCustomers.Count>0) customer = currentLevelData.PossibleCustomers[customerIndex];
            //Create request data
            RequestData requestData = CreateRequestData(menu, customer);
            allRequests.Add(requestData);
        }
    }

    public RequestData CreateRequestData(RequestData baseReq)
    {
        RequestData requestData = CreateRequestData(baseReq.Menu, baseReq.CustomerData);
        requestData.Time = baseReq.Time;
        requestData.Price = baseReq.Price;
        requestData.ShowCustomerType = baseReq.ShowCustomerType;
        requestData.StarReward = baseReq.StarReward;
        requestData.TipReward = baseReq.TipReward;
        requestData.Extra_ingredients = baseReq.Extra_ingredients;

        return requestData;
    }

    //LEVEL DESIGN
    public RequestData CreateRequestData(MenuData menu, CustomerData customer)
    {
        RequestData requestData = new RequestData();
        List<IngredientData> ingredients = new List<IngredientData>();

        foreach (IngredientData ingr in menu.ingredients)
        {
            if (customer.CustomerType == veganType)
            {
                if (ingr == meat)
                {
                    //Add แทน
                    ingredients.Add(veggie);
                }
                else
                {
                    ingredients.Add(ingr);
                }
            }
            else
            {
                ingredients.Add(ingr);
            }
        }

        //เพิ่มเนื้อถ้าเป็น meat lover
        if (customer.CustomerType == meatLoverType)
        {
            requestData.Extra_ingredients.Add(meat);
            requestData.Price += meat.BuyPrice * 2;//ให้เงินพิเศษค่าเนื้อด้วย
        }

        //เพิ่มดาวถ้าทำสำเร็จถ้าเป็น celeb
        if (customer.CustomerType == celebType)
        {
            requestData.StarReward = 3;
        }

        //เพิ่มดาวถ้าทำสำเร็จถ้าเป็น celeb
        if (customer.CustomerType == rusherType)
        {
            requestData.Time = menu.baseTime - (menu.baseTime*0.5f);
        }
        else
        {
            requestData.Time = menu.baseTime + customer.TimeModifier.GetRandomTime();
        }


        requestData.Ingredients = ingredients;
        requestData.CustomerData = customer;
        requestData.Menu = menu;
        requestData.Price += menu.basePrice;
        return requestData;
    }

    void Open()
    {
        shopOpenUI.ActiveButton(false);
        if (buyUI.IsShow)
        {
            buyUI.Hide(()=> { shopOpenUI.Hide(StartGame); });
        }
        else
        {
            shopOpenUI.Hide(StartGame);
        }
    }

    void CalculateResult()
    {
        if(playerData.money > currentLevelData.GoalMoney)
        {
            Debug.Log("<color=green>Win!!</color> : You got money " + playerData.money + " goal is " + currentLevelData.GoalMoney);
        }
        else
        {
            Debug.Log("<color=red>Lose!!</color> : You got money " + playerData.money + " goal is " + currentLevelData.GoalMoney);
        }
    }

    void LevelEnd()
    {
        Debug.Log("Close");
        isPlaying = false;

        CalculateResult();

        foreach (var r in requests)
        {
            if (!r.isComplete)
            {
                failRequests.Add(r.RequestData);
                r.Fail();
            }
            else
            {
                r.Hide();
            }
        }

        foreach (var kvp in customerCoroutine)
        {
            StopCoroutine(kvp.Value);
        }

        customerLeftUI.Hide();
        //penaltyUI.Hide();
        hand.Hide();
        //moneyUI.Hide();

        shopCloseUI.Hide();
        if (playerData.money >= currentLevelData.GoalMoney)
        {
            shopOpenUI.ActiveButton(false);
            shopOpenUI.Show(ShowResult);
        }
        else
        {
            shopOpenUI.ActiveButton(false);
            shopOpenUI.Show(Gameover);
        }

    }

    void ShowResult()
    {
        resultUI.Show();
        Debug.Log("Show result");

    }

    void Gameover()
    {
        textTotalCardInDeck.transform.parent.gameObject.SetActive(false);
        gameOverUI.Show();
        Debug.Log("Game Over.");
    }

    #endregion

    #region Special Item

    [SerializeField] Sprite spyIcon;
    [SerializeField] Animator scanlineAnimator;
    [SerializeField] AnimationClip scanLineIn;

    public void Btn_Spy()
    {
        if (isPlaying)
        {
            if (TryModifyMoney(-spyCost))
            {
                StartCoroutine(ieSpy());
            }
            else
            {
                Debug.Log("No Money");
            }
        }
    }

    IEnumerator ieSpy()
    {
        scanlineAnimator.SetTrigger("in");
        yield return new WaitForSeconds(0.2f);
        //yield return new WaitForSeconds(scanLineIn.length);
        foreach (var req in requests)
        {
            if (req.isShow && !req.isComplete && !req.RequestData.ShowCustomerType)
            {
                req.ExposeIdentity();
            }
            //req.CustomerPotrait.SetClickEvent(null,
            //    () => {
            //        req.ExposeIdentity();
            //        foreach (var req in requests)
            //        {
            //            req.CustomerPotrait.RemoveClickEvent();
            //        }
            //    }
            //);


        }
    }

    #endregion

}
