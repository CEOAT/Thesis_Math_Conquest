using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractableWindow : MonoBehaviour
{
    public GameObject windowGroup;

    private void OnEnable()
    {
        ExplorationModeObjectInteractable.interaction += WindowActivation;
    }
    private void OnDisable()
    {
        ExplorationModeObjectInteractable.interaction -= WindowActivation;
    }

    private void Awake()
    {
        SetupObject();
    }
    private void SetupObject()
    {
        windowGroup.SetActive(false);
    }

    private void WindowActivation()
    {
        if (windowGroup.activeInHierarchy == false)
        {
            windowGroup.SetActive(true);
        }
        else if (windowGroup.activeInHierarchy == true)
        {
            windowGroup.SetActive(false);
        }
    }

    public void ConfirmAnswer()
    {
        
    }

}
