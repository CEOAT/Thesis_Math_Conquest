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
        buttontext.fontMaterial.SetFloat("_GlowPower",glowpower);
    }

    private void LateUpdate()
    {
        buttontext.fontMaterial.SetFloat("_GlowPower",glowpower);
    }

    private void OnValidate()
    {
        buttontext.materialForRendering.SetFloat("_GlowPower",glowpower);
    }
}

