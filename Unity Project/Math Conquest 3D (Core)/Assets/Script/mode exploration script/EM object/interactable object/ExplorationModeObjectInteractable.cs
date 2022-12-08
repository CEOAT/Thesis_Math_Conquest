using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExplorationModeObjectInteractable : MonoBehaviour
{
    [Header("Interaction Setting")]
    [Tooltip("Type of interaction")] public InteractionType interactionType;
    public enum InteractionType
    {
        interactionWindow,
        instance
    };

    [Header("Repeat Interact Cooldown")]
    public bool isRepeatableInteraction = false;
    public float interactionCooldownTime = 3f;

    [Header("Player Disabled Time")]
    public float PlayerInteractionDisableTime = 3f;

    [Header("Reaction Cutscene Setting")]
    public bool isReactionTriggerCutscene = false;
    public float ReactionCutsceneDelayTime = 3f;

    [Header("Interact Check")]
    public bool isReadyToInteract = true;
    public bool isInteractionDone = false;

    public delegate void PuzzleInteract();
    public static event PuzzleInteract puzzleInteract;

    public GameObject reactionObject;
    [Tooltip("Type of reaction")] public ReactionType reactionType;
    public enum ReactionType
    {
        moveObjectUp,
        moveObjectDown,
        setActiveObject,
        removeObject
    };


    [Header("Interaction Bubble")]
    public GameObject interactionBubbleObject;

    [Header("Game Controller")]
    private BoxCollider interactCollider;
    public ExplorationModeGameController GameController;

    private void Start()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        interactCollider = GetComponent<BoxCollider>();
    }

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
            ExplorationModeObjectInteractableWindowUi.puzzleReaction += ActiveReaction;
        }
        else
        {
            
        }
    }
    private void CheckOnInteractionTypeOnDisable()
    {
        if (interactionType == InteractionType.interactionWindow)
        {
            ExplorationModeObjectInteractableWindowUi.puzzleReaction -= ActiveReaction;
        }
    }

    //called from player
    public void Interacted()    
    {
        if (isInteractionDone == false)
        {
            if (interactionType == InteractionType.interactionWindow)
            {
                puzzleInteract();
                isReactionTriggerCutscene = false;
            }
            else if (interactionType == InteractionType.instance)
            {
                ActiveReaction();
            }

            HideInteractionBubble();
            InteractReadyCheck();
        }

        if (isRepeatableInteraction == true)
        {
            StartCoroutine(InteractionCooldown());
        }
        else if (isRepeatableInteraction == false)
        {
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator InteractionCooldown()
    {
        interactCollider.center = new Vector3(interactCollider.center.x, 
                                                interactCollider.center.y - 30,
                                                interactCollider.center.z);
        
        yield return new WaitForSeconds(interactionCooldownTime);
        ShowInteractionBubble();
        isReadyToInteract = true;
        isInteractionDone = false;
        interactCollider.center = new Vector3(interactCollider.center.x,
                                                interactCollider.center.y + 30,
                                                interactCollider.center.z);
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
    public void ActiveReaction() // will be called from this script and puzzle window
    {
        isInteractionDone = true;
        TriggerReactionCutscene();

        switch (reactionType)
        {
            case ReactionType.moveObjectUp:
                {
                    ReactionMoveObjectUp();
                    break;
                }
            case ReactionType.moveObjectDown:
                {
                    ReactionMoveObjectDown();
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
    private void TriggerReactionCutscene()
    {
        if (isReactionTriggerCutscene == true)
        {
            GameController.TriggerCutscene();
            StartCoroutine(EndCutsceneDelay());
        }
        else if (isReactionTriggerCutscene == false && interactionType != InteractionType.interactionWindow)
        {
            GameController.PlayerMovement.PlayerDisabledMovement();
            StartCoroutine(EndInteractionDelay());
        }
    }
    private IEnumerator EndCutsceneDelay()
    {
        if (PlayerInteractionDisableTime < 3f) { PlayerInteractionDisableTime = 3f; }

        yield return new WaitForSeconds(ReactionCutsceneDelayTime);
        GameController.AllowMovement();
    }
    private IEnumerator EndInteractionDelay()
    {
        if(PlayerInteractionDisableTime < 1.25f) { PlayerInteractionDisableTime = 1.25f; }

        yield return new WaitForSeconds(PlayerInteractionDisableTime);
        GameController.AllowMovement();
    }
    private void ReactionMoveObjectUp()
    {
        reactionObject.GetComponent<Animator>().SetTrigger("triggerMoveUp");
    }
    private void ReactionMoveObjectDown()
    {
        reactionObject.GetComponent<Animator>().SetTrigger("triggerMoveDown");
    }
    private void ReactionSetActiveObject()
    {
        reactionObject.SetActive(true);
    }
    private void ReactionRemoveObject()
    {
        GameObject.Destroy(reactionObject.gameObject);
    }

    private void ShowInteractionBubble()
    {
        if (interactionBubbleObject != null)
        {
            interactionBubbleObject.SetActive(true);
        }
    }
    private void HideInteractionBubble()
    {
        if (interactionBubbleObject != null)
        {
            interactionBubbleObject.SetActive(false);
        }
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