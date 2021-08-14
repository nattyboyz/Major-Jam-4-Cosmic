using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatCardCarbinetUI : BaseUI
{
    [SerializeField] List<Transform> slots = new List<Transform>();
    [SerializeField] List<TextMeshProUGUI> slotAmount = new List<TextMeshProUGUI>();
    [ReadOnly][SerializeField] int currentSlot = 0;
    public Action<IngredientData, int> onDeduct;
    [SerializeField] Dictionary<CheatCard, int> slotIndices = new Dictionary<CheatCard, int>();
    bool isOpen = false;

    public bool TryAddCheatCard(CheatCard cheatCard)
    {
        if (currentSlot < slots.Count)
        {
            cheatCard.transform.SetParent(slots[currentSlot]);
            cheatCard.transform.localScale = new Vector3(1, 1, 1);
            cheatCard.transform.localPosition = new Vector3(0, 0, 0);
            cheatCard.CacheOriginalPosition();
            slotIndices.Add(cheatCard, currentSlot);
            int thisSlot = currentSlot;
            cheatCard.onSetAmount += (int amount) => { 
                slotAmount[thisSlot].text = amount.ToString(); 
            };
            currentSlot++;
            return true;
        }
        return false;
    }



    public void Btn_Open()
    {
        if (IsShow) Hide();
        else Show();
    }


}
