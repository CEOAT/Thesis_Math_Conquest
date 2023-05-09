using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class AddForce : ImmediateModeShapeDrawer
{
    [SerializeField] private Vector3 Force;

    private Rigidbody thisRigid;
    // Start is called before the first frame update
    void Start()
    {
        thisRigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thisRigid.AddForce(Vector3.ClampMagnitude(Force,2));
    }
    
    public override void DrawShapes( Camera cam ){

        using( Draw.Command( cam ) ){

            // set up static parameters. these are used for all following Draw.Line calls
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 8; // 4px wide

            // set static parameter to draw in the local space of this object

            var tempVec = Vector3.ClampMagnitude(Force, 2f);
            Draw.Line(this.transform.position,this.transform.position +tempVec);
            Draw.Cone(this.transform.position+tempVec,Force.normalized,0.25f,0.25f);
          
        
        }

    }
}
