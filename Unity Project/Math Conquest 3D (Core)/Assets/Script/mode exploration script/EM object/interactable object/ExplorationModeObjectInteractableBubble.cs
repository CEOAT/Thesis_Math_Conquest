using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractableBubble : MonoBehaviour
{
    [Header("Interact Bubble")]
    [SerializeField] public GameObject interactableBubbbleObject;
    private Animator interactableBubbbleAnimator;
    private ExplorationModeObjectInteractable ObjectInteractable;

    [Header("Bubble Animation")]
    public EntryAnimation entryAnimation;
    public enum EntryAnimation
    {
        popIn
    };
    public LeaveAnimation leaveAnimation;
    public enum LeaveAnimation
    {
        fadeDown
    };

    private void Start()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        interactableBubbbleAnimator = interactableBubbbleObject.GetComponent<Animator>();
        ObjectInteractable = GetComponent<ExplorationModeObjectInteractable>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player") == true)
        {
            PlayBubbleEntryAnimation();
        }
    }
    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player") == true)
        {
            if(ObjectInteractable.isNeedReenterTrigger == true
            && ObjectInteractable.isWaitReenter == true)
            {
                ObjectInteractable.AllowInteractionAfterReenter();
            }
            PlayerBubbleLeaveAnimation();
        }
    }

    private void PlayBubbleEntryAnimation()
    {
        if(ObjectInteractable.isReadyToInteract == false) { return; }

        if (entryAnimation == EntryAnimation.popIn)
        {
            interactableBubbbleObject.SetActive(true);
            ResetAnimatoTrigger();
            interactableBubbbleAnimator.SetBool("boolIsUIPopUp", true);
        }
    }
    private void PlayerBubbleLeaveAnimation()
    {
        if (leaveAnimation == LeaveAnimation.fadeDown)
        {
            interactableBubbbleAnimator.SetBool("boolIsUIPopUp", false);
        }
    }
    private void ResetAnimatoTrigger()
    {
        interactableBubbbleAnimator.SetBool("boolIsUIPopUp", false);
    }
}