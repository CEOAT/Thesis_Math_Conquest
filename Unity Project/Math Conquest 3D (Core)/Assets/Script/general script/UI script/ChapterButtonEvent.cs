using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ChapterButtonEvent : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Vector3 initscale;
    private RectTransform grouptransfrom;

    private void Start()
    {
        grouptransfrom = this.GetComponent<RectTransform>();
        initscale = grouptransfrom.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        grouptransfrom.localScale = new Vector3(2.2f,2.2f,1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        grouptransfrom.localScale = initscale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        grouptransfrom.localScale = initscale;
    }

    public void OnDisable()
    {
        grouptransfrom.localScale = initscale;
    }
}
