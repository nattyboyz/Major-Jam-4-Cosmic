using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class DragOnSpot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action<DragOnSpot> onEnterDrag;
    public Action<DragOnSpot> onExitDrag;
    public Action<Dragable> onExecute;
    //public Action<Card> onExecuteComplete;
    //public Action<Card> onExecuteFail;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            Debug.Log(gameObject.name + " Enter while dragging ");
            onEnterDrag?.Invoke(this);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            Debug.Log(gameObject.name + " Exit while dragging ");
            onExitDrag?.Invoke(this);
        }
    }


    public virtual void Focus(Dragable dragable)
    {
        Debug.Log("Focus");
    }

    public virtual void UnFocus()
    {
        Debug.Log("UnFocus");
    }

    public virtual void Execute(Dragable dragable)
    {
        onExecute?.Invoke(dragable);
        Debug.Log("Execute");
    }

    //public virtual void ExecuteFail(Card card)
    //{
    //    onExecuteFail?.Invoke(card);
    //}

    //public virtual void ExecuteComplete(Card card)
    //{
    //    onExecuteComplete?.Invoke(card);
    //}

}
