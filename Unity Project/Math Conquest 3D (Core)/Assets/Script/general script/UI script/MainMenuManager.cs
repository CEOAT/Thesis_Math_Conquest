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
    public struct MathConButtonMenuNavigation
    {
        public string ButtonAction;
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
        foreach (var B_Event in ButtonEvents)
        {
            var tempbutton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (B_Event.buttonClick == tempbutton)
            {
                tempDelay = B_Event.TransistionDelay;
                TransitionMaterialChange();
                StartCoroutine(ChangeCanvas(B_Event));
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
        buttonEvent.From.TryGetComponent(out GraphicRaycaster FromRaycast);
        FromRaycast.enabled = false;
        yield return new WaitForSeconds(tempDelay+1f);
        buttonEvent.From.SetActive(false);
        FromRaycast.enabled = true;
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
}
