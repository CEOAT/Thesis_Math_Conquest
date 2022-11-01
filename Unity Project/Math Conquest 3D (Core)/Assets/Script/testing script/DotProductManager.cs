using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProductManager : MonoBehaviour
{
    [System.Serializable]
    public struct dotproductTarget
    {
        public string key;
        public Transform VectorPointDestination;
        public DotSolution MethodSolution;
    }
    
    public enum DotSolution
    {
        InterectRotate,
        Gussing,
    }

    [SerializeField] private dotproductTarget[] DotproductTargets;
    public static DotProductManager inst;

    private void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
