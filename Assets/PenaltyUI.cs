using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenaltyUI : MonoBehaviour
{
    [SerializeField] List<Image> stars;
    [SerializeField] Color normalColor;
    [SerializeField] Color penaltyColor;


    public void AddPenalty(int index)
    {
        if (index < stars.Count)
        {
            stars[index].color = penaltyColor;
        }
    }
}
