using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIfaceCam : MonoBehaviour
{
   private Camera tempcam;
   private void Start()
   {
       var temp = GameObject.FindObjectsOfType<Camera>();
       foreach (var VARIABLE in temp)
       {
           if (VARIABLE.name == "UIcamera")
           {
               tempcam = VARIABLE;
           }
       }
   }

   private void LateUpdate()
   {
       
      transform.LookAt(transform.position+ tempcam.transform.rotation*Vector3.forward,tempcam.transform.rotation*Vector3.up);
   }
}
