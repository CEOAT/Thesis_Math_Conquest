using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quickdemo : MonoBehaviour
{
   private Collider Childbox;
   
 
   private void Start()
   {
      Childbox = GetComponent<Collider>();
   }

   private void OnTriggerEnter(Collider other)
   {
       Debug.Log(other.bounds.center);
      
       foreach (var VARIABLE in this.GetComponentsInChildren<Transform>())
       {
         
           if (this.transform.GetInstanceID() != VARIABLE.GetInstanceID())
           {
               Debug.Log(Vector3.Magnitude(other.bounds.center - VARIABLE.transform.position) , VARIABLE );
               if (Vector3.Magnitude(other.bounds.center - VARIABLE.transform.position) <= VARIABLE.GetComponent<BoxCollider>().size.x)
               {
                   Debug.Log("BIG OOF " + VARIABLE.gameObject.name);
               }
           }
       }
   }

 
}
