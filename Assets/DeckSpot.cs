using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckSpot : DragOnSpot
{
    [SerializeField] Image spotImage;
    [SerializeField] Image focusImage;

    private void Start()
    {
        focusImage.gameObject.SetActive(false);
    }

    public override void Focus(Dragable dragable)
    {
        base.Focus(dragable);
        if (dragable is Card)
        {
            focusImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Cannot drag this to deck ");
        }
    }

    public override void UnFocus()
    {
        base.UnFocus();
        focusImage.gameObject.SetActive(false);
    }

}
