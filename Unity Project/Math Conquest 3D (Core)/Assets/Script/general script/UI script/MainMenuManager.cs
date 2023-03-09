using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Serializable]
    public enum ButtonBehavior
    {
        Default,
        Instant
    }
    
    [Serializable]
    public struct MathConButtonMenuNavigation
    {
        public string ButtonAction;
        public ButtonBehavior buttonBehavior;
        public float TransistionDelay;
        public Button buttonClick;
        public GameObject From;
        public GameObject To;
    }
    
   
    private float tempDelay;
    [SerializeField] private MathConButtonMenuNavigation[] ButtonEvents;
    public Image transistion;

    public void playtransistion()
    {
        var tempbutton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        foreach (var B_Event in ButtonEvents)
        {
            if (B_Event.buttonClick == tempbutton)
            {
              // Debug.Log("CLICK");
                if (B_Event.buttonBehavior == ButtonBehavior.Default)
                {
                    tempDelay = B_Event.TransistionDelay;
                    TransitionMaterialChange();
                    StartCoroutine(ChangeCanvas(B_Event));
                }
                if (B_Event.buttonBehavior == ButtonBehavior.Instant)
                {
                    tempDelay = B_Event.TransistionDelay;
                    StartCoroutine(ChangeCanvas(B_Event));
                }
            } ;
        }
    }

    public void TransitionMaterialChange()
    {
        transistion.raycastTarget = false;
        StartCoroutine(ChangeTransistionMaterial());
    }

    IEnumerator ChangeCanvas(MathConButtonMenuNavigation buttonEvent)
    {
        buttonEvent.From.TryGetComponent(out GraphicRaycaster fromRaycast);
        if (!ReferenceEquals(fromRaycast,null))fromRaycast.enabled = false;
        yield return new WaitForSeconds(tempDelay+1f);
        buttonEvent.From.SetActive(false);
        if(!ReferenceEquals(fromRaycast,null)) fromRaycast.enabled = true;
        buttonEvent.To.SetActive(true);
        yield break;
    }

    IEnumerator ChangeTransistionMaterial()
    {
        transistion.raycastTarget = true;
        yield return new WaitForSeconds(0.25f);
        Material tempmat = transistion.material;
        Sequence s = DOTween.Sequence();
        s.Append(DOVirtual.Float(0, 1f, tempDelay, v => tempmat.SetFloat("_Cutoff",v )));
        yield return new WaitForSeconds(2f);
        s.Append(DOVirtual.Float(1F, 0f, tempDelay, v => tempmat.SetFloat("_Cutoff",v )));
        transistion.raycastTarget = false;
        yield break;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
