using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleWorldSpaceObject : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    private bool isCompleteAssignValue = false;

    private void Start()
    {
        if(isCompleteAssignValue == false)
        {
            AssignValue();
        }
    }
    private void AssignValue()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        isCompleteAssignValue = true;
    }

    private void OnEnable() 
    {
        if(isCompleteAssignValue == true)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            transform.localScale = startScale;
        }
    }
}