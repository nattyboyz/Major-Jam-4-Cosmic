using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckSpot : DragOnSpot
{
    [SerializeField] Image spotImage;

    public override void Focus(Dragable dragable)
    {
        base.Focus(dragable);
        if (dragable is Card)
        {
            focus.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Cannot drag this to deck ");
        }
    }

    public override void UnFocus()
    {
        base.UnFocus();
        focus.gameObject.SetActive(false);
    }

}
