using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMaterialsInstance : MonoBehaviour
{
    //private MaterialExposeController image;
    private Material imagemat;
     
    void Start()
    {
        if (TryGetComponent(out MaterialExposeController oof ))
        {
            imagemat = oof.material;
            oof.material = Instantiate(imagemat);
        }
        else if (TryGetComponent(out Image oof2 ))
        {
            imagemat = oof2.material;
            oof2.material = Instantiate(imagemat);
        }

      /*   image = this.GetComponent<MaterialExposeController>();
        var shaderring =  this.GetComponent<MaterialExposeController>().material;
        Material mat = Instantiate(shaderring);
        image.material = mat;*/
    
    }
}
