using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExplorationModeObjectInteractable : MonoBehaviour
{
    [Header("Interaction Setting")]
    [Tooltip("Type of interaction")] [SerializeField] public InteractionType interactionType;
    public enum InteractionType
    {
        interactionWindow,
        interactionWindowMultipleChoice,
        instance,
        uninteractable
    };

    [Header("Enemy Prohibitation")]
    [SerializeField] public float enemyCheckRange;

    [Header("Repeat Interact Cooldown")]
    public bool isRepeatableInteractionOrStatic = false;
    public bool isNeedReenterTrigger = true;
    public float interactionCooldownTime = 3f;
    [SerializeField] public bool isWaitReenter = false;

    [Header("Player Disabled Time")]
    public float PlayerInteractionDisableTime = 3f;

    [Header("Reaction Cutscene Setting")]
    public bool isReactionTriggerCutscene = false;
    public float ReactionCutsceneDelayTime = 3f;

    [Header("Interact Check")]
    public bool isReadyToInteract = true;
    public bool isInteractionDone = false;

    public GameObject reactionObject;
    [Tooltip("Type of reaction")] [SerializeField] public ReactionType reactionType;
    public enum ReactionType
    {
        moveObjectUp,
        moveObjectDown,
        setActiveObject,
        removeObject
    };


    [Header("Interaction Bubble")]
    [SerializeField] public GameObject interactionBubbleObject;

    [Header("Game Controller")]
    public ExplorationModeGameController GameController;
    private ExplorationModeObjectInteractableWindowUi InteractionWindow;
    private ExplorationModePuzzleWorldSpaceWindow WorldSpaceWindow;
    private BoxCollider interactCollider;

    private void Awake()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        interactCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SetupInteractionType();
    }
    private void SetupInteractionType()
    {
        if (interactionType == InteractionType.interactionWindow)
        {
            InteractionWindow = GetComponent<ExplorationModeObjectInteractableWindowUi>();
            if(WorldSpaceWindow != null)
                InteractionWindow.enabled = false;
        }
        else if (interactionType == InteractionType.interactionWindowMultipleChoice)
        {
            WorldSpaceWindow = GetComponent<ExplorationModePuzzleWorldSpaceWindow>();
            if(WorldSpaceWindow != null)
                WorldSpaceWindow.enabled = false;
        }
        else if (interactionType == InteractionType.uninteractable)
        {
            interactionBubbleObject.GetComponent<Animator>().SetBool("boolIsUninteractable", true);
        }
    }

    //called from player
    public void Interacted()    
    {
        if (isInteractionDone == false)
        {
            if (interactionType == InteractionType.interactionWindow)
            {
                InteractionWindow.enabled = true;
                InteractionWindow.WindowActivation();
                isReactionTriggerCutscene = false;
                isRepeatableInteractionOrStatic = true;
            }
            else if (interactionType == InteractionType.interactionWindowMultipleChoice)
            {
                WorldSpaceWindow.enabled = true;
                WorldSpaceWindow.StartMultipleChoicePuzzleWindow();
                isReactionTriggerCutscene = false;
                isRepeatableInteractionOrStatic = true;
            }
            else if (interactionType == InteractionType.instance)
            {
                ActiveReaction();
            }

            HideInteractionBubble();
            InteractReadyCheck();
        }

        if (isRepeatableInteractionOrStatic == true)
        {
            StartCoroutine(InteractionCooldown());
        }
        else if (isRepeatableInteractionOrStatic == false && isReactionTriggerCutscene == false)
        {
            DisableObjectAfterInteraction();
        }
    }
    private void DisableObjectAfterInteraction()
    {
        this.gameObject.SetActive(false);
    }
    private IEnumerator InteractionCooldown()
    {
        if (isNeedReenterTrigger == false)
        {
            MoveTriggeroff();
            yield return new WaitForSeconds(interactionCooldownTime);
            AllowInteraction();
        }
    }
    public void AllowInteraction()
    {
        ShowInteractionBubble();
        isReadyToInteract = true;
        isInteractionDone = false;
        MoveTriggerIn();
    }
    public void AllowInteractionAfterReenter()
    {
        ShowInteractionBubble();
        isReadyToInteract = true;
        isInteractionDone = false;
        isWaitReenter = false;
    }
    public void MoveTriggeroff()
    {
        interactCollider.center = new Vector3(interactCollider.center.x,
                                                interactCollider.center.y - 30,
                                                interactCollider.center.z);
    }
    private void MoveTriggerIn()
    {
        interactCollider.center = new Vector3(interactCollider.center.x,
                                                interactCollider.center.y + 30,
                                                interactCollider.center.z);
    }
    private void InteractReadyCheck()
    {
        if( isNeedReenterTrigger == true && isWaitReenter == false)
        {
            isWaitReenter = true;
        }

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
        else if (isReactionTriggerCutscene == false 
            && interactionType != InteractionType.interactionWindow)
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
        GameController.PlayerMovement.PlayerEnabledMovement();
        DisableObjectAfterInteraction();
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

    [SerializeField] private LayerMask layerMask;
    private bool isNoEnemyInRange;
    public bool CheckNoEnemyInRange()
    {
        isNoEnemyInRange = true;

        Collider[] enemyArray = Physics.OverlapSphere(transform.position, enemyCheckRange, layerMask);

        foreach(Collider gameObject in enemyArray)
        {
            if(gameObject.tag == "Enemy")
            {
                isNoEnemyInRange = false;
                break;
            }
        }

        return isNoEnemyInRange;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), enemyCheckRange);
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