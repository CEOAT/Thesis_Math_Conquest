using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBlock : MonoBehaviour
{
    [SerializeField] private Transform ResetPos;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("VectorBall"))
        {
            collision.gameObject.transform.position = ResetPos.position;
        }
    }
}
