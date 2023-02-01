using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GenericHoldButtonEvent : MonoBehaviour,IUpdateSelectedHandler,IPointerDownHandler,IPointerUpHandler
{
    private bool isPressed;
    private bool preventanim;
    public Button Button;
    
              public void OnUpdateSelected(BaseEventData data)
              {
                  if (isPressed)
                  {
                   Debug.Log("ooooof long hold"+isPressed);
                  }
                  
              }
              public void OnPointerDown(PointerEventData data)
              {
                  isPressed = true;
                  Debug.Log(isPressed);
              }
              public void OnPointerUp(PointerEventData data)
              {
                  isPressed = false;
                  Debug.Log(isPressed);
              }

              public void OnPointerExit(PointerEventData data)
              {
                
              }

              public void OnPointerClick(BaseEventData data)
              {
                  preventanim = true;
                  Button.animator.SetBool("PreventTransition",preventanim);
                  Debug.Log("preventanim"+preventanim);
              }

              public void ResetDefaultState()
              {
                  preventanim = false;
                  Button.animator.SetBool("PreventTransition",preventanim);
              }
          
}
