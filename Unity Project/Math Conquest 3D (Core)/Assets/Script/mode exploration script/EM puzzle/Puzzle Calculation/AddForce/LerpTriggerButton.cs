using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTriggerButton : MonoBehaviour
{
    [SerializeField] private VectorLerpManager _manager;

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            _manager.SetUpInput();
            _manager.PlayerInput.Enable();
            _manager.FindCurrentPlatform();
            _manager.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _manager.interactable = false;
            _manager.PlayerInput.Disable();
        }
    }
}
