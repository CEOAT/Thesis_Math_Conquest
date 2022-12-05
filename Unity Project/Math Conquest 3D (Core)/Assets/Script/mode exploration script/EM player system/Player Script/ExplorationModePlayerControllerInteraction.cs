using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerControllerInteraction : MonoBehaviour
{
    public Transform ray3DObject;
    private Ray ray3D;
    private RaycastHit raycastHit3D;
    public LayerMask layerMask3D;
    public float ray3DRange;

    MasterInput playerInput;

    ExplorationModePlayerControllerMovement playerMovement;

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
        playerMovement = GetComponent<ExplorationModePlayerControllerMovement>();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void FixedUpdate()
    {
        PlayerRaycast3D();
    }
    private void PlayerRaycast3D()
    {
        ray3D = new Ray(ray3DObject.position, ray3DObject.forward);
        if (ray3DRange > 0)
        {
            if (Physics.Raycast(ray3D, out raycastHit3D, ray3DRange,
                layerMask3D, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(ray3D.origin, raycastHit3D.point, Color.red);
            }
            else
            {
                Debug.DrawLine(ray3D.origin, ray3D.origin + ray3D.direction * ray3DRange, Color.green);
            }
        }
    }

    private void PlayerInteract()
    {
        if (raycastHit3D.transform == null) { return; }
        if (raycastHit3D.transform.CompareTag("Interactable"))
        {
            raycastHit3D.transform.GetComponent<ExplorationModeObjectInteractable>().Interacted();
            print(raycastHit3D.transform.name);
            playerMovement.PlayerInteract();
        }
    }
}
