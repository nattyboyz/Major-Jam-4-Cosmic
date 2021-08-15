using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardSpot : DragOnSpot
{
    [SerializeField] Image spotImage;

    public override void Focus(Dragable dragable)
    {
        base.Focus(dragable);
        if (dragable is Card)
        {
            focus.SetActive(true);
        }
        else
        {
            Debug.Log("Cannot drag this to discard ");
        }
    }

    public override void UnFocus()
    {
        base.UnFocus();
        focus.SetActive(false);
    }

}
