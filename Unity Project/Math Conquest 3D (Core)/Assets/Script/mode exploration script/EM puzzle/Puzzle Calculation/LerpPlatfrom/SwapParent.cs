using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapParent : MonoBehaviour
{
    private Transform tempPlayerParent;
    private VectorLerpManager tempmanager;
    private ExplorationModePlayerControllerMovement _player;
    
    private void Start()
    {
        var tempPlayer = FindObjectOfType<ExplorationModeGameController>();
        _player = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        tempPlayerParent = _player.transform.parent;
        var tempmanagerstart = FindObjectOfType<VectorLerpManager>();
        tempmanager = tempmanagerstart.GetComponent<VectorLerpManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
            foreach (var VARIABLE in tempmanager.Platforms)
            {
                if (this.transform == VARIABLE.transform)
                {
                    Debug.Log("found plat");
                    VARIABLE.isFocus =(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(tempPlayerParent);
            foreach (var VARIABLE in tempmanager.Platforms)
            {
                if (this.transform == VARIABLE.transform)
                {
                    VARIABLE.isFocus = (false);
                }
            }
        }
    }
}
