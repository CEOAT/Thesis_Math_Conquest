using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractableBubble : MonoBehaviour
{
    [Header("Interact Bubble")]
    public GameObject interactableBubbbleObject;
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
            interactableBubbbleAnimator.SetTrigger("trigger Interaction UI Pop Up");
        }
    }
    private void PlayerBubbleLeaveAnimation()
    {
        if (leaveAnimation == LeaveAnimation.fadeDown)
        {
            interactableBubbbleAnimator.SetTrigger("trigger Interaction UI Fade Down");
        }
    }
    private void ResetAnimatoTrigger()
    {
        interactableBubbbleAnimator.ResetTrigger("trigger Interaction UI Pop Up");
        interactableBubbbleAnimator.ResetTrigger("trigger Interaction UI Fade Down");
    }
}