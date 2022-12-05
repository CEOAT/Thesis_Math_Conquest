using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TmpExposeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttontext;
    [SerializeField] private float glowpower;
    private void Start()
    {
        buttontext = GetComponent<TextMeshProUGUI>();
        buttontext.defaultMaterial.SetFloat("_GlowPower",glowpower);
          
    }

    private void LateUpdate()
    {
        var tmpmat = buttontext.defaultMaterial;
        tmpmat.SetFloat("_GlowPower",glowpower);
        buttontext.GetModifiedMaterial(tmpmat);
    }

    /*private void OnValidate()
    {
        var tmpmat = buttontext.defaultMaterial;
        tmpmat.SetFloat("_GlowPower",glowpower);
        buttontext.GetModifiedMaterial(tmpmat);
    }*/
}

