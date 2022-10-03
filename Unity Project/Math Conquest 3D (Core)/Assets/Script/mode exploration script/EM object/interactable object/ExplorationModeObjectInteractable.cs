using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExplorationModeObjectInteractable : MonoBehaviour
{
    public delegate void PuzzleInteract();
    public static event PuzzleInteract puzzleInteract;

    private void OnEnable()
    {
        ExplorationModeObjectInteractableWindow.puzzleReaction += ActiveReaction;
    }
    private void OnDisable()
    {
        ExplorationModeObjectInteractableWindow.puzzleReaction -= ActiveReaction;
    }

    public bool isReadyToInteract = true;
    public bool isInteractionDone = false;

    [Tooltip("Type of interaction")] public InteractionType interactionType;
    public enum InteractionType
    {
        puzzleWindow,       //create puzzle window
        instance            //active some action (open door, call elevator)
    };
    //**implement later**

    public GameObject ReactionObject;
    [Tooltip("Type of reaction")] public ReactionType reactionType;
    public enum ReactionType
    {
        moveObject,
        removeObject
    };

    //called from player
    public void Interacted()    
    {
        if (isInteractionDone == false)
        {
            puzzleInteract();
            if (isReadyToInteract == true)
            {
                isReadyToInteract = false;
            }
            else
            {
                isReadyToInteract = true;
            }
        }
    }

    public void ActiveReaction()
    {
        isInteractionDone = true;
        switch (reactionType)
        {
            case ReactionType.moveObject:
                {
                    ReactionMoveObject();
                    break;
                }
            case ReactionType.removeObject:
                {
                    ReactionRemoveObject();
                    break;
                }
        }
    }
    private void ReactionMoveObject()
    {
        ReactionObject.transform.position = ReactionObject.transform.position + (Vector3.up * 0.25f);
    }
    private void ReactionRemoveObject()
    {
        GameObject.Destroy(ReactionObject.gameObject);
    }
}

#if UNITY_EDITOR
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
        if (GUILayout.Button("Active Reaction"))
        {
            interactable.ActiveReaction();
        }
    }
}
#endif