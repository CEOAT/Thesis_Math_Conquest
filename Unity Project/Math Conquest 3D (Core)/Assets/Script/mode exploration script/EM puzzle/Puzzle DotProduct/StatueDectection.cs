using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class StatueDectection : ImmediateModeShapeDrawer
{
    public enum DotMode
    {
        DotNormDirection,
        DotDetection,
        Compass,
    }

    [Header("Statue DotProduct mode")] 
    [SerializeField] private DotMode DotProductMode;
    

    #region Status Compass Configuration
    [FolderGroupAttributeDrawer("Statue Compass Configuration")]
    [SerializeField] private Transform CompassObject;
    [FolderGroupAttributeDrawer("Statue Compass Configuration")]
    [SerializeField] private DetectDirection CompassDirection;
    [SerializeField] private TextMeshProUGUI Compasstxt;
    #endregion

    #region Statue FOV Configuration
    
    [Serializable]
    public enum DetectDirection
    {
        Blue_Front,
        Blue_Back,
        Red_Left,
        Red_Right,
    }
    
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [Header("Statue FOV Configuration")]
    [SerializeField] private  DetectDirection  detectDirection;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private float DetectionAngle;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private float DetectionRadius;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private GameObject LineUI;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private GameObject RadiusUI;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private TextMeshProUGUI linetxt;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private TextMeshProUGUI radiustxt;

    #endregion
  
    #region Statue Dot Configuration
    [FolderGroupAttributeDrawer("Statue Dot Configuration")]
    [Header("Statue DotProduct Normalize Configuration")] 
    [SerializeField] private Transform target;
    #endregion

    #region Shapes Visual
    [Header("Shape Visual for Detection")]
    [SerializeField] private Disc Cone;
    [SerializeField] private Line PlayerDistanceLine;
    [Header("Shape Visual for DotProduct normalize")]
    [SerializeField] private Disc AngleArea;
    [SerializeField] private Line VectorNormalize;
    [Header("Shape Visual for Compass ")]
    [SerializeField] private Line VectorCompass;
    #endregion
    [Space]
    [SerializeField] private GameObject focusparent;
    [SerializeField] private GameObject ShapeGroup;
    [SerializeField] private bool isfocus;
  
    #region private_var
    private ExplorationModePlayerControllerMovement _player;
   private Coroutine tempCoroutine;
   private Color tempColor;
   private float DotproductPuzzle;
   private bool PuzzleDotMatch;
   private Vector3 playerToStatueVector3;
   #endregion
    
   #region Capsule_Var
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
   public float Radius
   {
       get => DetectionRadius;
       set => DetectionRadius = value;
   }
   public float ThisDotproductvar => DotproductPuzzle;

   public bool ThisPuzzleDotMatch => PuzzleDotMatch;
   public Vector3 PlayerToStatueVector3 => playerToStatueVector3;
    
   [HideInInspector] public string DotproductInfo;

   #endregion
   void Start()
    {
        DotModeConfiguration();
        tempColor = Cone.Color;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
        if (DotProductMode == DotMode.Compass)
        {
            UpdateCompassLine();
        }
        if (DotProductMode == DotMode.DotNormDirection)
        {
            LineToTarget();
            UnderLineAngle();
        }
        if (DotProductMode == DotMode.DotDetection)
        {
            UpdateConeAngle();
            lookforPlayer();
            UiDistance();
            ChangeVectorDirection();
        }
    }
    //-right = blue
    //forward = red


    public void DotModeConfiguration()
    {
        switch (DotProductMode)
        {
            case DotMode.DotNormDirection :
                focusparent = focusparent.transform.Find("DotDirection").gameObject;
                break;
            case DotMode.DotDetection :
                focusparent = focusparent.transform.Find("DotDetection").gameObject;
                break;
            case DotMode.Compass :
                focusparent = focusparent.transform.Find("DotCompass").gameObject;
                break;
            default:
                break;
        }
    }
    

    public void OnSelectThis()
    {
        isfocus = true;
        if (!Isfocus)
        {
            focusparent.SetActive(false);
        }
        else
        {
            focusparent.SetActive(true);
        }
    }

    public void UnSelectThis()
    {
        isfocus = false;
        if (!Isfocus)
        {
            focusparent.SetActive(false);
        }
        else
        {
            focusparent.SetActive(true);
        }
    }

    public void DisableVisual(bool enable = false)
    {
        ShapeGroup.SetActive(enable);
    }
    public void EnableVisual(bool enable = true)
    {
        ShapeGroup.SetActive(enable);
    }

    #region DotCompass

    public void UpdateCompassLine()
    {
        VectorCompass.End =   CompassObject.position - this.transform.position  ;
        switch (CompassDirection)
        {
            case DetectDirection.Blue_Front :
            case DetectDirection.Blue_Back :
                if (Vector3.Dot(CompassObject.forward, (this.transform.position - CompassObject.position).normalized )> 0)
                {
                    var tempDot = Vector3.Dot(CompassObject.forward,
                        (this.transform.position - CompassObject.position).normalized);
                    string thaiInfo = $"DotProduct(A,B) : {tempDot}"+ "\n ค่า Dot มากกว่า 0 อยู่ในทิศเดียวกับ vector foward ";
                    DotproductInfo = thaiInfo;
                    Compasstxt.text = "Front Side";
                }
                else
                {
                    var tempDot = Vector3.Dot(CompassObject.forward,
                        (this.transform.position - CompassObject.position).normalized);
                    string thaiInfo = $"DotProduct(A,B) : {tempDot}"+"\n ค่า Dot มากกว่า 0 อยู่ในทิศตรงข้ามกับ vector foward ";
                    DotproductInfo = thaiInfo;
                    Compasstxt.text = "Back Side";
                }
                break;
            case DetectDirection.Red_Left :
            case DetectDirection.Red_Right:
                if (Vector3.Dot(CompassObject.right, (this.transform.position - CompassObject.position).normalized )> 0)
                {
                    var tempDot = Vector3.Dot(CompassObject.right,
                        (this.transform.position - CompassObject.position).normalized);
                    string thaiInfo = $"DotProduct(A,B) : {tempDot}"+"\n ค่า Dot  มากกว่า 0 อยู่ในทิศเดียวกับ vector Right ";
                    DotproductInfo = thaiInfo;
                    Compasstxt.text = "Right Side";
                }
                else
                {
                    var tempDot = Vector3.Dot(CompassObject.right,
                        (this.transform.position - CompassObject.position).normalized);
                    string thaiInfo =  $"DotProduct(A,B) : {tempDot}"+ "\n ค่า Dot  มากกว่า 0 อยู่ในทิศเดียวกับ vector Left ";
                    DotproductInfo = thaiInfo;
                    Compasstxt.text = "Left Side";
                }
                break;
            default:
                break;
        }
    }
    

    #endregion

    #region DotNormalize

    public void LineToTarget()
    {
        var tempTargetVector = target.transform.position - VectorNormalize.transform.position;
        var tempvec =  (VectorNormalize.transform.worldToLocalMatrix* (tempTargetVector)).normalized *2f;
        tempvec.y = 0f;
        VectorNormalize.End = tempvec;
    }

    public void UnderLineAngle()
    {
      //  Debug.Log((VectorNormalize.End).normalized);
      //  Debug.Log(Mathf.Acos(Vector3.Dot(VectorNormalize.End.normalized,Vector3.right))*0.5f*Mathf.Rad2Deg);
        AngleArea.AngRadiansEnd = Mathf.Acos(Vector3.Dot(VectorNormalize.End.normalized, Vector3.forward));
        switch (VectorNormalize.End.x)
        {
            case <0 :
                if (AngleArea.AngRadiansEnd > 0) AngleArea.AngRadiansEnd = Mathf.Clamp(-AngleArea.AngRadiansEnd,-360f,0f);
                break;
            case >0 :
                if (AngleArea.AngRadiansEnd < 0) AngleArea.AngRadiansEnd = Mathf.Clamp(AngleArea.AngRadiansEnd,0f,360f);
                break;
            default:
                AngleArea.AngRadiansEnd = 0f;
                break;
        }

        DotproductPuzzle = Vector3.Dot(VectorNormalize.End.normalized, Vector3.forward);
        
        var tempInverselerp = Mathf.InverseLerp(-45f, 45f, AngleArea.AngRadiansEnd*Mathf.Rad2Deg);
        if ( tempInverselerp <= 0.65 && tempInverselerp>=0.35f)
        {
            VectorNormalize.Color = new Color(0.8f,1,0,1f);
            PuzzleDotMatch = true;
        }
        else
        {
            PuzzleDotMatch = false;
            VectorNormalize.Color = Color.white;
        }
        
    }

    #endregion

    #region DotDetection

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

    public void UiDistance()
    {
        float tempDistance = Vector3.Distance(_player.transform.position, transform.position);
        Vector3 tempvec = Vector3.Lerp(_player.transform.position, this.transform.position, 0.5f);
        tempvec.y = transform.position.y;
        LineUI.transform.position = tempvec;
        linetxt.text = "|A|: "+ tempDistance.ToString("F1");
        RadiusUI.transform.position =  Vector3.Lerp(this.transform.position,Cone.transform.position + Cone.transform.right*Cone.Radius,1) ;
        radiustxt.text = "Radius : " + DetectionRadius;
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
        playerToStatueVector3 = toPlayer;
        Vector3 tempPlayerScanLine = this.transform.worldToLocalMatrix*  toPlayer;
        PlayerDistanceLine.End = tempPlayerScanLine ;
        if (toPlayer.magnitude <= DetectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized,Cone.transform.right) > Mathf.Cos(DetectionAngle* 0.5f * Mathf.Deg2Rad) )
            {
                OnDetected();
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
    
    public void OnDetected()
    {
        if(tempCoroutine!=null) return;
        tempCoroutine=  StartCoroutine(PunishPlayer());
    }

    IEnumerator PunishPlayer()
    {
        if(_player.TryGetComponent<ExplorationModePlayerHealth>(out ExplorationModePlayerHealth playerHealth))
            playerHealth.PlayerTakenDamage(35f);
        yield return new WaitForSeconds(0.5f);
        tempCoroutine = null;
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
            if (DotProductMode == DotMode.Compass)
            {
                Draw.Matrix = CompassObject.localToWorldMatrix;
                switch (CompassDirection)
                {
                       
                    case DetectDirection.Blue_Front :
                        Draw.Line( Vector3.zero,  Vector3.forward,   Color.blue   );
                        Draw.Cone(Vector3.forward,Vector3.forward,0.1f,0.5f, Color.blue);
                        break;
                    case DetectDirection.Blue_Back :
                        Draw.Line( Vector3.zero,  -Vector3.forward,   Color.cyan   );
                        Draw.Cone(-Vector3.forward,Vector3.forward,0.1f,0.5f,Color.cyan  );
                        break;
                    case DetectDirection.Red_Right :
                        Draw.Line( Vector3.zero,  Vector3.right,   Color.red   );
                        Draw.Cone(Vector3.right,Vector3.right,0.1f,0.5f,Color.red   );
                        break;
                    case DetectDirection.Red_Left :
                        Draw.Line( Vector3.zero, -Vector3.right ,   new Color(0.7f,0.2f,0)   );
                        Draw.Cone(-Vector3.right,-Vector3.right,0.1f,0.5f,new Color(0.7f,0.2f,0)  );
                        break;
                }
            }
            if (DotProductMode == DotMode.DotNormDirection)
            {
                Draw.Line( Vector3.zero, Vector3.forward*2f,   Color.yellow   );
            }
            if (DotProductMode == DotMode.DotDetection)
            {
                Draw.Line( Vector3.zero, Vector3.right*1f,   Color.red   );
                Draw.Line( Vector3.zero, Vector3.forward*1f, Color.blue  );
            }
        
        }

    }
  
}

public class FolderGroupAttributeDrawer : PropertyGroupAttribute
{
    public float R, G, B, A;

    public FolderGroupAttributeDrawer(string path) : base(path)
    {
        
    }
     
}