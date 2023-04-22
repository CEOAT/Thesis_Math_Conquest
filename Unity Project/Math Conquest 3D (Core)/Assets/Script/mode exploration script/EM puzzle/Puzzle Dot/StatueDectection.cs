using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class StatueDectection : MonoBehaviour
{
    
    public float DetectionAngle;
    
    public float DetectionRadius;
    
    private ExplorationModePlayerControllerMovement _player;

    public Disc Cone;

    private Color tempColor;
   
    void Start()
    {
        tempColor = Cone.Color;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
        UpdateConeAngle();
        lookforPlayer();
    }

    public void UpdateConeAngle()
    {
       Cone.AngRadiansStart = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.AngRadiansEnd = -DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.Radius = DetectionRadius;
    }
    
    private Vector3 _tempLineVec3;
    private Vector3 _tempPlayerVec3;
    public void lookforPlayer()
    {
        Vector3 enemyPostion = Cone.transform.position;
        Vector3 tempPlayerPose = _player.transform.position;
        enemyPostion.y = 0;
        tempPlayerPose.y = 0;
        Vector3 toPlayer = tempPlayerPose - enemyPostion;
        toPlayer.y = 0;
        _tempLineVec3 = toPlayer;
        Debug.Log(Vector3.Dot(toPlayer.normalized,Cone.transform.right));
        if (toPlayer.magnitude <= DetectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized,Cone.transform.right) > Mathf.Cos(DetectionAngle* 0.5f * Mathf.Deg2Rad) )
            {
                Cone.Color = Color.red;
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

   

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Cone.transform.position,Cone.transform.position+_tempLineVec3);
    }
}
