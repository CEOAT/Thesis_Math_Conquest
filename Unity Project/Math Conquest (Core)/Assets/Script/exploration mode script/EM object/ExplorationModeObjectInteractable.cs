using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractable : MonoBehaviour
{
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
        if (isReadyToInteract == true)
        {
            GetInteraction();
        }
    }
    private void GetInteraction()   //when interact successful **subcribe to this
    {
        isReadyToInteract = false;
    }
    private void LeaveInteraction() //call when close the window or leave interaction **reference this method
    {
        isReadyToInteract = true;
    }
}