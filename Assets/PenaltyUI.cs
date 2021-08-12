using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenaltyUI : BaseUI
{
    [SerializeField] List<Image> stars;//Max = 5

    public void SetStar(int amount)
    {
        for(int i = 0; i < stars.Count; i++)
        {
            if (i < amount)
            {
                stars[i].gameObject.SetActive(true);
            }
            else
            {
                stars[i].gameObject.SetActive(false);
            }
        }

    }
}
