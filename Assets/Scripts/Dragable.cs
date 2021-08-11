using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
   

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
     
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
   
    }

}
