using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class AddForce : ImmediateModeShapeDrawer
{
    [SerializeField] private Vector3 force;

    private Rigidbody thisRigid;

    public Vector3 Force
    {
        get => force;
        set => force = value;
    }
   
    void Start()
    {
        thisRigid = GetComponent<Rigidbody>();
        
    }

   
    void FixedUpdate()
    {
        thisRigid.AddForce(Vector3.ClampMagnitude(force,2f));
    }
    
    public override void DrawShapes( Camera cam ){

        using( Draw.Command( cam ) ){

            // set up static parameters. these are used for all following Draw.Line calls
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 8; // 4px wide

            // set static parameter to draw in the local space of this object

            var tempVec = Vector3.ClampMagnitude(force, 2f);
            Draw.Line(this.transform.position,this.transform.position +tempVec);
            Draw.Cone(this.transform.position+tempVec,force.normalized,0.25f,0.25f);
          
        
        }

    }
}
