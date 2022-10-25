using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class UIRotateCharacter : MonoBehaviour ,IBeginDragHandler,IDragHandler
{
    private Vector2 touchStart;
    private Vector2 currentTouch;

    public Action<float> OnDragLeft;
    public Action<float> OnDragRight;

    public static UIRotateCharacter inst;

    private void Awake()
    {
        inst = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        touchStart = eventData.position;
  //      Debug.Log("Being" + eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentTouch = eventData.position;
//        Debug.Log("Drag" + eventData.position);
        if(touchStart.x > currentTouch.x)
        {
            float distance = touchStart.x - currentTouch.x;
            OnDragLeft?.Invoke(distance);
            touchStart = currentTouch;
        }
        else if(touchStart.x < currentTouch.x)
        {
            float distance = touchStart.x - currentTouch.x;
            OnDragRight?.Invoke(distance);
            touchStart = currentTouch;
        }
        else if(touchStart.x == currentTouch.x)
        {
            return;
        }
    }
}
