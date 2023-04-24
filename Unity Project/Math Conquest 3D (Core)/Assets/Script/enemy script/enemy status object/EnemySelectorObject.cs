using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectorObject : MonoBehaviour
{
    public Transform selectorTrackPoint;
    public float selectorRotateSpeed;
    private void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x * selectorTrackPoint.localScale.x
                                            , transform.localScale.y * selectorTrackPoint.localScale.y
                                            , transform.localScale.z * selectorTrackPoint.localScale.z);
    }
    private void FixedUpdate()
    {
        transform.position = selectorTrackPoint.position + new Vector3(0,0,0);
        transform.Rotate(Vector3.forward * (selectorRotateSpeed * Time.deltaTime));
    }
}