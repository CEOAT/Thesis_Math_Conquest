using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class StatueDectection : ImmediateModeShapeDrawer
{
    [Serializable]
    public enum DetectDirection
    {
        Blue_Front,
        Blue_Back,
        Red_Left,
        Red_Right,
        
    }
    
    [Header("Statue FOV Configuration")]
    [SerializeField] private  DetectDirection  detectDirection;
    [SerializeField] private float DetectionAngle;
    [SerializeField] private float DetectionRadius;
    [SerializeField] private GameObject focusparent;
    [SerializeField] private bool isfocus;
    [SerializeField] private Color selectcolor;
    [SerializeField] private GameObject LineUI;
    [SerializeField] private TextMeshProUGUI lineTxT;
    private ExplorationModePlayerControllerMovement _player;
    
     

    public bool Isfocus
    {
        get => isfocus;
        set => isfocus = value;
    }
    public DetectDirection DetectDirectionVector
    {
        get => detectDirection;
        set => detectDirection = value;
    }

    [Header("Shape Visual")]
   [SerializeField] private Disc Cone;
   [SerializeField] private Line PlayerDistanceLine;
   

    private Color tempColor;
   
  
    void Start()
    {
        
        tempColor = Cone.Color;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
        OnSelectThis();
        UpdateConeAngle();
        lookforPlayer();
        UiDistance();
        ChangeVectorDirection();
    }
    //-right = blue
    //forward = red
    public void ChangeVectorDirection()
    {
        switch (detectDirection)
        {
            case DetectDirection.Blue_Front :
                Cone.transform.LookAt(this.transform,-this.transform.right);
                break;
            case DetectDirection.Blue_Back :
                Cone.transform.LookAt(this.transform,this.transform.right);
                break;
            case DetectDirection.Red_Left :
                Cone.transform.LookAt(this.transform,-this.transform.forward);
                break;
            case DetectDirection.Red_Right :
                Cone.transform.LookAt(this.transform,this.transform.forward);
                break;
            default:
                break;
        }
    }

    public void OnSelectThis()
    {
        if (!Isfocus)
        {
            focusparent.SetActive(false);
        }
        else
        {
            focusparent.SetActive(true);
        }
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
            
            if(!Isfocus) return;
            // draw lines
            Draw.Line( Vector3.zero, Vector3.right,   Color.red   );
            Draw.Line( Vector3.zero, Vector3.up,      Color.green );
            Draw.Line( Vector3.zero, Vector3.forward, Color.blue  );
        }

    }

    
}
