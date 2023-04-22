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
   
    void Start()
    {
        Cone.AngRadiansStart = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.AngRadiansEnd = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.Radius = DetectionRadius;
        var tempPlayer = FindObjectOfType<ExplorationModePlayerControllerMovement>();
        _player = tempPlayer.GetComponent<ExplorationModePlayerControllerMovement>();
    }

    
    void Update()
    {
       
       // lookforPlayer();
    }

  /*  public void UpdateConeAngle()
    {
        Cone.AngRadiansStart = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.AngRadiansEnd = DetectionAngle*Mathf.Deg2Rad *0.5f;
        Cone.Radius = DetectionRadius;
    }*/

    public void lookforPlayer()
    {
        Vector3 enemyPostion = transform.position;
        Vector3 toPlayer = _player.transform.position - enemyPostion;
        toPlayer.y = 0;
        if (toPlayer.magnitude <= DetectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(DetectionAngle) * 0.5f * Mathf.Deg2Rad)
            {
                
            }
        }
    }




}
