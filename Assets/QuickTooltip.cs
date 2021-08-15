using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [SerializeField] bool enableTooltip = true;
   [SerializeField] GameObject toolTipGameobject;
    [SerializeField] TextMeshProUGUI text;

  [SerializeField] float hoverTime = 1f;

    bool isHover = false;
    float time = 0;
    

    public bool EnableTooltip { get => enableTooltip; set => enableTooltip = value; }
    public float HoverTime { get => hoverTime; set => hoverTime = value; }
    public TextMeshProUGUI Text { get => text; set => text = value; }

    private void Awake()
    {
        if(toolTipGameobject)toolTipGameobject.SetActive(false);
    }

    void Update()
    {
        if (EnableTooltip)
        {
            if (isHover)
            {
                time += Time.deltaTime;
                if(time>= hoverTime)
                {
                    if (toolTipGameobject) toolTipGameobject.SetActive(true);
                    isHover = false;
                    time = 0;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EnableTooltip && !eventData.dragging)
        {
            //if (toolTipGameobject) toolTipGameobject.SetActive(true);
            isHover = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EnableTooltip && !eventData.dragging)
        {
            isHover = false;
            time = 0;
            if (toolTipGameobject) toolTipGameobject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isHover = false;
        if (EnableTooltip )
        {
            isHover = false;
            time = 0;
            if (toolTipGameobject) toolTipGameobject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EnableTooltip)
        {
            isHover = true;
        }
    }

    public void SetText(string description)
    {
      if(Text!=null)Text.text = description;
    }
}
