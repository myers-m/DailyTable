using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiTriggerElement : UiElement, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.Do("OnPointerEnter", eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.Do("OnPointerExit", eventData);
    }
}
