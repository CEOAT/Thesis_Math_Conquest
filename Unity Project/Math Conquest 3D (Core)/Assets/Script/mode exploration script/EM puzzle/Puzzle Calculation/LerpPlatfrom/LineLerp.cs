using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class LineLerp : MonoBehaviour
{
    [SerializeField] private Transform PointA;

    [SerializeField] private Transform PointB;

    private Line lerpLine;
    void Start()
    {
        lerpLine = GetComponent<Line>();
       
    }

    private void Update()
    {
        lerpLine.Start =    PointA.transform.localPosition    ;
        lerpLine.End =     PointB.transform.localPosition  ;
    }
}
