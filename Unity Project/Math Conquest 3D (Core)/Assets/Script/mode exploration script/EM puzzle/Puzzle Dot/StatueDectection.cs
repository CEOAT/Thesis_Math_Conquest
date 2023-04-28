using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class StatueDectection : ImmediateModeShapeDrawer
{
    [Header("Statue FOV Configuration")]
    [SerializeField] private float DetectionAngle;
    [SerializeField] private float DetectionRadius;
    [SerializeField] private bool Isfocus;
    [SerializeField] private Color selectcolor;
    [SerializeField] private GameObject LineUI;
    [SerializeField] private TextMeshProUGUI lineTxT;
    private ExplorationModePlayerControllerMovement _player;
    
    [Header("Shape Visual")]
   [SerializeField] private Disc Cone;
   [SerializeField] private Line PlayerDistanceLine;
   

    private Color tempColor;
   
    //-right = blue
    //forward = red
    void Start()
    {
        Cone.transform.LookAt(this.transform,this.transform.forward);
  
        
        tempColor = Cone.Color;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
        UpdateConeAngle();
        lookforPlayer();
        UiDistance();
    }

    public void UiDistance()
    {
        float tempDistance = Vector3.Distance(_player.transform.position, transform.position);
        Vector3 tempvec = Vector3.Lerp(_player.transform.position, this.transform.position, 0.5f);
        tempvec.y = transform.position.y;
        LineUI.transform.position = tempvec;
        lineTxT.text = Mathf.Round(tempDistance).ToString();
    }

    public void UpdateConeAngle()
    {
       Cone.AngRadiansStart = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.AngRadiansEnd = -DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.Radius = DetectionRadius;
    }

  
    private Vector3 _tempLineVec3;
    private Vector3 _tempPlayerVec3;

    #region DetectPlayer

    public void lookforPlayer()
    {
        Vector3 enemyPostion = Cone.transform.position;
        Vector3 tempPlayerPose = _player.transform.position;
        enemyPostion.y = 0;
        tempPlayerPose.y = 0;
        Vector3 toPlayer = tempPlayerPose - enemyPostion;
        toPlayer.y = 0;
        Vector3 tempPlayerScanLine = this.transform.worldToLocalMatrix*  toPlayer;
        PlayerDistanceLine.End = tempPlayerScanLine ;
        if (toPlayer.magnitude <= DetectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized,Cone.transform.right) > Mathf.Cos(DetectionAngle* 0.5f * Mathf.Deg2Rad) )
            {
                Cone.Color = new Color(1,0,0,0.5f);
            }
            else
            {
                Cone.Color = tempColor;
            }
        }
        else
        {
            Cone.Color = tempColor;
        }
      
    }

    #endregion
    

    public override void DrawShapes( Camera cam ){

        using( Draw.Command( cam ) ){

            // set up static parameters. these are used for all following Draw.Line calls
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 8; // 4px wide

            // set static parameter to draw in the local space of this object
            Draw.Matrix = transform.localToWorldMatrix;

            // draw lines
            Draw.Line( Vector3.zero, Vector3.right,   Color.red   );
            Draw.Line( Vector3.zero, Vector3.up,      Color.green );
            Draw.Line( Vector3.zero, Vector3.forward, Color.blue  );
        }

    }

    
}
