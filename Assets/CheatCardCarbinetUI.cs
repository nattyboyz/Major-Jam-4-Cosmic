using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCardCarbinetUI : BaseUI
{
    [SerializeField] List<Transform> slots = new List<Transform>();
    [ReadOnly][SerializeField] int currentSlot = 0;
    bool isOpen = false;

    public bool TryAddCheatCard(CheatCard cheatCard)
    {
        if (currentSlot < slots.Count)
        {
            cheatCard.transform.SetParent(slots[currentSlot]);
            cheatCard.transform.localScale = new Vector3(1, 1, 1);
            cheatCard.transform.localPosition = new Vector3(0, 0, 0);
            cheatCard.CacheOriginalPosition();
            currentSlot++;
            return true;
        }
        return false;
    }

    public void Btn_Open()
    {
        if (isShow) Hide();
        else Show();
    }


}
