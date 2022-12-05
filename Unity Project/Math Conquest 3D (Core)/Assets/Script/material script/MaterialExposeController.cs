using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] 
public class MaterialExposeController : Image
{
[SerializeField] private  float _myCustomFloat;
[ColorUsageAttribute(true,true)]
[SerializeField] private Color _Customcolor;

 
protected override void Awake() {
    base.Awake();
}

protected override void Start()
{
    base.Start();
   
}

protected void Update()
{
   GetModifiedMaterial(defaultMaterial);
}

public override Material GetModifiedMaterial(Material baseMaterial)
{
    Material tmp = base.GetModifiedMaterial(baseMaterial);
    tmp.SetColor("_Emission",_Customcolor);
    tmp.SetFloat("_Lerp",_myCustomFloat);
    return tmp;
}

}
