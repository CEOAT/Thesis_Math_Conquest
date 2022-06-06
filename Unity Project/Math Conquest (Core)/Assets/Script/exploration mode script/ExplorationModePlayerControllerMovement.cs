using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerControllerMovement : MonoBehaviour
{
    public float playerMoveSpeed = 5f;
    public Transform playerRaycastPoint;

    private float upDownInput;
    private float leftRightInput;

    Rigidbody rigidbody;
    MasterInput playerInput;

    private void Awake()
    {
        SetupConponent();
        SetupControl();
    }
    private void SetupConponent()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
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
        PlayerMoveUpDown();
        PlayerMoveLeftRight();
    }
    private void PlayerMoveUpDown()
    {
        upDownInput = playerInput.PlayerControlExploration.MoveUpdown.ReadValue<float>();
        
        if (upDownInput == 1 && leftRightInput ==0)
        {
            rigidbody.AddForce(Vector3.forward * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (upDownInput == -1 && leftRightInput == 0)
        {
            rigidbody.AddForce(-Vector3.forward * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void PlayerMoveLeftRight()
    {
        leftRightInput = playerInput.PlayerControlExploration.MoveLeftRight.ReadValue<float>();

        if (leftRightInput == 1 && upDownInput == 0)
        {
            rigidbody.AddForce(Vector3.right * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (leftRightInput == -1 && upDownInput == 0)
        {
            rigidbody.AddForce(-Vector3.right * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}
