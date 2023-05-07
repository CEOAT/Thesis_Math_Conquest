using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneControllerMenuIntro : MonoBehaviour
{

    [SerializeField] private GameObject menuTransitionObject;

    private MasterInput PlayerInput;

    private void Start()
    {
       SubscribeButton();
    }
    private void SubscribeButton()
    {
        PlayerInput = new MasterInput();
        PlayerInput.WindowControl.CloseWindow.performed += context => SkipIntro();
        PlayerInput.Enable();
    }

    private void OnDisable() 
    {
        ActiveMainMenuTransition();
        UnsubscribeButton();
    }
    private void ActiveMainMenuTransition()
    {
        menuTransitionObject.SetActive(false);
        menuTransitionObject.SetActive(true);
    }
    private void UnsubscribeButton()
    {
        PlayerInput.Disable();
    }

    private void SkipIntro()
    {
        this.gameObject.SetActive(false);
    }
}
