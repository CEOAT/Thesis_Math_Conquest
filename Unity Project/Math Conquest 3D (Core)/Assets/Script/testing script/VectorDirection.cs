using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Shapes;
using Unity.Mathematics;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class VectorDirection : MonoBehaviour
{
   [SerializeField] private Transform Vector1;
   private Vector3 vectorDummy;
   private ShapeGroup ArrowColor;
   [SerializeField] private TextMeshProUGUI Dot;
   private GameObject lookat;
   private Vector3 realone;
   private float dotproduct;
   private void Awake()
   {
     
   }

   private void Start()
   {  
    
      ArrowColor = Vector1.GetComponent<ShapeGroup>(); 
      lookat = GameObject.FindWithTag("DotTrasue");
      var lookPos = new Vector3(lookat.transform.position.x,0,lookat.transform.position.z) + new Vector3(transform.position.x,0,transform.position.z);
      transform.parent.position = lookat.transform.position- lookat.transform.forward*4 ;
      transform.up = lookPos;
       realone = transform.up;
       dotproduct = Vector3.Dot(this.transform.up, realone) *100;
      // transform.rotation= Quaternion.Euler(90,Mathf.Round(this.transform.rotation.eulerAngles.y),Mathf.Round(this.transform.rotation.eulerAngles.z));
      // transform.rotation = quaternion.Euler(Vector3.zero);
     
    /*  while (dotproduct % 10 != 0)
      {
         transform.rotation= Quaternion.Euler(90,Mathf.Round(this.transform.rotation.eulerAngles.y),Mathf.Round(Random.Range(-360f,360f)));
         dotproduct = Vector3.Dot(this.transform.up, realone) *100;
      }*/
     
     

   }

   private void Update()
   {
      // transform.up = new Vector3(lookat.transform.position.x,0,lookat.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z);
      // transform.rotation= Quaternion.Euler(90,Mathf.Round(this.transform.rotation.eulerAngles.y),Mathf.Round(this.transform.rotation.eulerAngles.z));
    
      Debug.Log("Update roation" + transform.rotation);
      Debug.Log( Vector3.Dot(this.transform.up, Vector1.up));
      ArrowColor.Color = Color.Lerp(Color.red, Color.green, Vector3.Dot(this.transform.up, Vector1.up));
      Dot.text = $"DOT : {Vector3.Dot(this.transform.up, realone).ToString("N2")}";
   }

   private void OnDrawGizmos()
   {
      Vector3 point_C = this.transform.rotation.normalized * (this.transform.position.normalized+Vector3.forward);
      Gizmos.DrawLine(this.transform.position, this.transform.position+this.transform.up);
      var lookat = GameObject.FindWithTag("DotTrasue");
      var lookPos = lookat.transform.position ;
    //  Gizmos.DrawSphere(lookPos,1.5f);
      Gizmos.DrawLine(transform.position, lookat.transform.position+ lookat.transform.forward*2);
      Gizmos.color = Color.red;
      Gizmos.DrawLine(lookat.transform.position,realone+lookat.transform.position*2);
   }
}
