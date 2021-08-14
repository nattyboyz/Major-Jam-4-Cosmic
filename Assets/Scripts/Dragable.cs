using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public bool allowDrag = true;
    public Vector3 deltaDragPos;
    public Action<Dragable> onStartDrag;
    public Action<Dragable> onEndDrag;
    public Action<GameObject> onDragRayUpdate;
    public Action<GameObject> onDragRelease;

    public static Vector3 zero = new Vector3(0, 0, 0);
    public static Vector3 one = new Vector3(1, 1, 1);

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
       
        if (allowDrag)
        {

            this.transform.localScale *= 1.2f;
            deltaDragPos = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
            deltaDragPos.Set(deltaDragPos.x, deltaDragPos.y, 0);
            onStartDrag?.Invoke(this);
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (allowDrag)
        {
            var pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.Set(pos.x, pos.y, this.transform.position.z);

            this.gameObject.transform.position = pos + deltaDragPos;
            var result = eventData.pointerCurrentRaycast;
            if (result.gameObject != null && result.gameObject != gameObject)
            {
                onDragRayUpdate?.Invoke(result.gameObject);
            }
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (allowDrag)
        {

            this.transform.localScale = new Vector3(1, 1, 1);
            deltaDragPos = zero;

            var result = eventData.pointerCurrentRaycast;
            if (result.gameObject != null && result.gameObject != gameObject)
            {
                onDragRelease?.Invoke(result.gameObject);
            }

            onEndDrag?.Invoke(this);
        }
    }

}
