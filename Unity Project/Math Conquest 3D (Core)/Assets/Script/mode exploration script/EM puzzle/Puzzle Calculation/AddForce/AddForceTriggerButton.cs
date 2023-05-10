using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTriggerButton : MonoBehaviour
{
    [SerializeField] private VectorAddForceManager _manager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _manager.thisPlayerInput.Enable();
            _manager.Interable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _manager.Interable = false;
            _manager.thisPlayerInput.Disable();
        }
    }

    public void DisableCollider()
    {
        var temocollider = GetComponent<BoxCollider>();
        temocollider.enabled = false;
    }
}
