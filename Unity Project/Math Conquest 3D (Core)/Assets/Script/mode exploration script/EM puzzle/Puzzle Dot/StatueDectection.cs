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
    }
    [Serializable]
    public enum DetectDirection
    {
        Blue_Front,
        Blue_Back,
        Red_Left,
        Red_Right,
        
    }

    [Header("Statue DotProduct mode")] 
    [SerializeField] private DotMode DotProductMode;
    
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [Header("Statue FOV Configuration")]
    [SerializeField] private  DetectDirection  detectDirection;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private float DetectionAngle;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private float DetectionRadius;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private GameObject focusparent;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private bool isfocus;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private GameObject LineUI;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private GameObject RadiusUI;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private TextMeshProUGUI linetxt;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    [SerializeField] private TextMeshProUGUI radiustxt;
    [FolderGroupAttributeDrawer("Statue FOV Configuration")]
    private ExplorationModePlayerControllerMovement _player;
    
    [FolderGroupAttributeDrawer("Statue Dot Configuration")]
    [Header("Statue DotProduct Normalize Configuration")] 
    [SerializeField] private Transform target;
    
    [Header("Shape Visual for Detection")]
    [SerializeField] private Disc Cone;
    [SerializeField] private Line PlayerDistanceLine;
    [Header("Shape Visual for DotProduct normalize")]
    [SerializeField] private Disc AngleArea;
    [SerializeField] private Line VectorNormalize;
    
   
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
   
   private Coroutine tempCoroutine;
   private Color tempColor;
   
  
    void Start()
    {
        DotModeConfiguration();
        tempColor = Cone.Color;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
        OnSelectThis();
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
            default:
                break;
        }
    }

    private void ConfigurationVisual()
    {
        switch (DotProductMode)
        {
            case DotMode.DotNormDirection :
                
                break;
            case DotMode.DotDetection :
             
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
        Cone.enabled = enable;
    }
    public void EnableVisual(bool enable = true)
    {
        Cone.enabled = enable;
    }

    #region DotNormalize

    public void LineToTarget()
    {
        var tempvec =  (VectorNormalize.transform.worldToLocalMatrix* (target.transform.position-VectorNormalize.transform.position)).normalized *2f;
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

        
        var tempInverselerp = Mathf.InverseLerp(-45f, 45f, AngleArea.AngRadiansEnd*Mathf.Rad2Deg);
        if ( tempInverselerp <= 0.65 && tempInverselerp>=0.35f)
        {
            VectorNormalize.Color = new Color(0.8f,1,0,1f);
        }
        else
        {
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
        linetxt.text = tempDistance.ToString("F1");
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