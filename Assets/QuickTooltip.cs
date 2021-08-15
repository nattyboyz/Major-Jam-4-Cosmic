using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool enableTooltip = true;
   [SerializeField] GameObject toolTipGameobject;
    [SerializeField] float hoverTime = 1f;

    bool isHover = false;
    float time = 0;
    

    public bool EnableTooltip { get => enableTooltip; set => enableTooltip = value; }
    public float HoverTime { get => hoverTime; set => hoverTime = value; }

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
        if (EnableTooltip)
        {
            //if (toolTipGameobject) toolTipGameobject.SetActive(true);
            isHover = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EnableTooltip)
        {
            isHover = false;
            time = 0;
            if (toolTipGameobject) toolTipGameobject.SetActive(false);
        }
    }
}
