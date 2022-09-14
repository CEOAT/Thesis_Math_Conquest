using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Shapes;


public class VectorDirection : MonoBehaviour
{
   [SerializeField] private Transform Vector1;
   private Vector3 vectorDummy;
   private ShapeGroup ArrowColor;
   [SerializeField] private TextMeshProUGUI Dot;

   private void Start()
   {
      ArrowColor = Vector1.GetComponent<ShapeGroup>();
   }

   private void Update()
   {
      Debug.Log( Vector3.Dot(this.transform.up, Vector1.up));
      ArrowColor.Color = Color.Lerp(Color.red, Color.green, Vector3.Dot(this.transform.up, Vector1.up));
      Dot.text = $"DOT : {Vector3.Dot(this.transform.up, Vector1.up).ToString("N2")}";
   }

   private void OnDrawGizmos()
   {
      Vector3 point_C = this.transform.rotation.normalized * (this.transform.position.normalized+Vector3.forward);
      Gizmos.DrawLine(this.transform.position, this.transform.position+this.transform.up);
   }
}
