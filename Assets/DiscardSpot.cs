using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardSpot : DragOnSpot
{
    [SerializeField] Image spotImage;
    [SerializeField] Image focusImage;

    private void Start()
    {
        focusImage.gameObject.SetActive(false);
    }

    public override void Focus(Card card)
    {
        base.Focus(card);
        focusImage.gameObject.SetActive(true);
    }

    public override void UnFocus()
    {
        base.UnFocus();
        focusImage.gameObject.SetActive(false);
    }


}
