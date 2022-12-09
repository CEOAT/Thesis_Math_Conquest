using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerControllerInteraction : MonoBehaviour
{
    public LayerMask interactableLayerMask;
    private MasterInput playerInput;
    private  ExplorationModePlayerControllerMovement PlayerMovement;

    private void Awake()
    {
        SetupConrol();
        SetupComponent();
    }
    private void SetupConrol()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlExploration.Interact.performed += context => PlayerInteract();
    }
    private void SetupComponent()
    {
        PlayerMovement = GetComponent<ExplorationModePlayerControllerMovement>();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), 1f);
    }

    private Collider[] interactableObject;
    private void PlayerInteract()
    {
        if (PlayerMovement.canControlCharacter == false) { return; }

        interactableObject = Physics.OverlapSphere(
                transform.position,
                1f,
                interactableLayerMask);

        if (interactableObject.Length != 0)
        {
            if (interactableObject[0].transform.CompareTag("Interactable") == true)
            {
                ExplorationModeObjectInteractable InteractableObject = interactableObject[0].transform.GetComponent<ExplorationModeObjectInteractable>();
                if (InteractableObject.isReadyToInteract == true)
                {
                    InteractableObject.Interacted();
                    PlayerMovement.PlayerWait();
                    PlayerMovement.PlayerInteract();
                }
            }
        }
    }
}
