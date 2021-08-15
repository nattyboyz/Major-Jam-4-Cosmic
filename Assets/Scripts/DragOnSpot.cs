using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DragOnSpot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image focus_img;
    [SerializeField] protected Image highlight_img;

    public Action<DragOnSpot> onEnterDrag;
    public Action<DragOnSpot> onExitDrag;
    public Action<Dragable> onExecute;

    private void Awake()
    {
        UnFocus();
        UnHighlight();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
           // Debug.Log(gameObject.name + " Enter while dragging ");
            onEnterDrag?.Invoke(this);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
           // Debug.Log(gameObject.name + " Exit while dragging ");
            onExitDrag?.Invoke(this);
        }
    }

    public virtual void Focus(Dragable dragable)
    {
       // Debug.Log("Focus");
    }

    public virtual void UnFocus()
    {
       // Debug.Log("UnFocus");
    }

    public virtual void Execute(Dragable dragable)
    {
        onExecute?.Invoke(dragable);
       // Debug.Log("Execute");
    }

    public  virtual void Highlight()
    {
        Debug.Log("<color=yellow> Highlight </color>" + name);
        if(highlight_img!=null)highlight_img.gameObject.SetActive(true);
    }

    public virtual void UnHighlight()
    {
        Debug.Log("<color=orange> UnHighlight </color>" + name);
        if (highlight_img != null)
            highlight_img.gameObject.SetActive(false);
    }

}
