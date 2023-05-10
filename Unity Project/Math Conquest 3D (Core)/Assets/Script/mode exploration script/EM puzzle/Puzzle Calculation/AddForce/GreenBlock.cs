using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBlock : MonoBehaviour
{
    [SerializeField] private VectorAddForceManager _manager;
    [SerializeField] private AddForceTriggerButton puzzleTrigger;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("VectorBall"))
        {
           _manager.EndPuzzle();
           puzzleTrigger.DisableCollider();
           puzzleTrigger.enabled = false;
        }
    }
}
