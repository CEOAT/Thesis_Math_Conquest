using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectorObject : MonoBehaviour
{
    public Transform selectorTrackPoint;
    public float selectorRotateSpeed;
    private void FixedUpdate()
    {
        transform.position = selectorTrackPoint.position + new Vector3(0,0,0);
        transform.Rotate(Vector3.forward * (selectorRotateSpeed * Time.deltaTime));
    }
}