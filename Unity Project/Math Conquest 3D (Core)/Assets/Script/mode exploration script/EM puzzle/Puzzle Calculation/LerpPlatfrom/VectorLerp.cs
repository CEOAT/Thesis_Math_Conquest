using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLerp : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [Range(0, 1f)]
    public float T;

    public bool isFocus;

    [SerializeField] public GameObject FocusGroup;
    
    private void Update()
    {
        this.transform.position = Lerpshowcase(pointA.position, pointB.position, T);
    }

    private Vector3 Lerpshowcase(Vector3 a,Vector3 b,float t)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(a.x+(b.x-a.x)*t,a.y+(b.y-a.y)*t,a.z+(b.z-a.z)*t);
    }

    public void OnFocus(bool focus)
    {
        FocusGroup.SetActive(focus);
    }
}
