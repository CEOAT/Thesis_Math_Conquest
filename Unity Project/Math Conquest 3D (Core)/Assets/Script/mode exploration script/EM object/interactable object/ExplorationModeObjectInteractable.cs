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
        CheckInteractionTypeOnEnable();
    }
    private void OnDisable()
    {
        CheckOnInteractionTypeOnDisable();
    }

    private void CheckInteractionTypeOnEnable()
    {
        if (GetComponent<ExplorationModeObjectInteractableWindowUi>() != null)
        {
            interactionType = "Interactable Window";
            ExplorationModeObjectInteractableWindowUi.puzzleReaction += ActiveReaction;
        }
        else
        {
            interactionType = "Instance";
        }
    }
    private void CheckOnInteractionTypeOnDisable()
    {
        if (interactionType == "Interactable Window")
        {
            ExplorationModeObjectInteractableWindowUi.puzzleReaction -= ActiveReaction;
        }
    }

    public string interactionType;
    public bool isReadyToInteract = true;
    public bool isInteractionDone = false;

    public GameObject ReactionObject;
    [Tooltip("Type of reaction")] public ReactionType reactionType;
    public enum ReactionType
    {
        moveObject,
        setActiveObject,
        removeObject
    };

    //called from player
    public void Interacted()    
    {
        if (isInteractionDone == false)
        {
            if (interactionType == "Interactable Window")
            {
                puzzleInteract();
            }
            if (interactionType == "Instance")
            {
                ActiveReaction();
            }

            InteractReadyCheck();
        }
    }
    private void InteractReadyCheck()
    {
        if (isReadyToInteract == true)
        {
            isReadyToInteract = false;
        }
        else
        {
            isReadyToInteract = true;
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
            case ReactionType.setActiveObject:
                {
                    ReactionSetActiveObject();
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
    private void ReactionSetActiveObject()
    {
        ReactionObject.SetActive(true);
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