using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpResetPuzzle : MonoBehaviour
{
    [SerializeField] private VectorLerpManager _manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _manager.ResetPuzzle();
        }
    }
}
