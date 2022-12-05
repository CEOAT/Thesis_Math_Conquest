using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMaterialsInstance : MonoBehaviour
{
    private MaterialExposeController image;
     
    void Start()
    {
         image = this.GetComponent<MaterialExposeController>();
        var shaderring =  this.GetComponent<MaterialExposeController>().material;
        Material mat = Instantiate(shaderring);
        image.material = mat;
    
    }
}
