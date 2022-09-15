using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExplorationModeObjectInteractable : MonoBehaviour
{
    public delegate void Interact();
    public static event Interact interaction;

    public bool isReadyToInteract = true;

    [Tooltip("Type of interaction")] public InteractionType interactionType;
    public enum InteractionType
    {
        puzzleWindow,       //create puzzle window
        instance            //active some action (open door, call elevator)
    };
    //**implement later**

    public void Interacted()    //call from player
    {
        if (isReadyToInteract || !isReadyToInteract)
        {
            GetInteraction();
            interaction();
        }

    }
    private void GetInteraction()   //when interact successful **subcribe to this
    {
        isReadyToInteract = !isReadyToInteract;
    }
    private void LeaveInteraction() //call when close the window or leave interaction **reference this method
    {
        isReadyToInteract = true;
    }   
}

[CustomEditor(typeof(ExplorationModeObjectInteractable))]
public class ObjectInteractableTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExplorationModeObjectInteractable interactable = (ExplorationModeObjectInteractable)target;

        if (GUILayout.Button("Interact"))
        {
            interactable.Interacted();
        }
    }
}