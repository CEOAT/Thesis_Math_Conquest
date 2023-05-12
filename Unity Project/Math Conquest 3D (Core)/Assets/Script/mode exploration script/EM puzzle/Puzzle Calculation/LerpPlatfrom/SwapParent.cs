using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapParent : MonoBehaviour
{
    private Transform tempPlayerParent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tempPlayerParent = other.transform.parent;
            other.transform.SetParent(this.transform);
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(tempPlayerParent);
        }
    }
}
