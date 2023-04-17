using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerResetPositionTrigger : MonoBehaviour
{

    [SerializeField] private List<Transform> objectList = new List<Transform>();
    private List<Vector3> vectorList = new List<Vector3>();
    private bool isCompleteAssignValue;
    private void Start()
    {
        if(isCompleteAssignValue == false)
        {
            AssignPreviousPosition();
            isCompleteAssignValue = true;
        }
    }
    private void AssignPreviousPosition()
    {
        foreach(Transform objectTransform in objectList)
        {
            vectorList.Add(objectTransform.position);
        }
    }
    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            ResetPostion();
        }
    }
    private void ResetPostion()
    {
        int positionCount = 0;
        foreach(Transform objectTransform in objectList)
        {
            objectTransform.position = vectorList[positionCount];
            positionCount++;
        }
    }
}