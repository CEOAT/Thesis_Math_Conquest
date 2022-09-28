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

    public string playerStatus;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        SetupConponent();
        SetupControl();
    }
    private void SetupConponent()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        PlayerIdle();
        PlayerMove();
        PlayerAnimation();
    }
    private void PlayerIdle()
    {
        upDownInput = playerInput.PlayerControlExploration.MoveUpdown.ReadValue<float>();
        leftRightInput = playerInput.PlayerControlExploration.MoveLeftRight.ReadValue<float>();
        if (upDownInput == 0 && leftRightInput == 0)
        {
            playerStatus = "idle";
        }
    }
    private void PlayerMove()
    {
        upDownInput = playerInput.PlayerControlExploration.MoveUpdown.ReadValue<float>();
        leftRightInput = playerInput.PlayerControlExploration.MoveLeftRight.ReadValue<float>();

        if (upDownInput == 1)
        {
            rigidbody.AddForce(Vector3.forward * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
            playerStatus = "run up";

        }
        if (upDownInput == -1)
        {
            rigidbody.AddForce(-Vector3.forward * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            playerStatus = "run down";
        }
        if (leftRightInput == 1)
        {
            rigidbody.AddForce(Vector3.right * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 90, 0);
            if (leftRightInput == 1 && upDownInput == 0)
            {
                playerStatus = "run right";
            }
        }
        if (leftRightInput == -1)
        {
            rigidbody.AddForce(-Vector3.right * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, -90, 0);
            if (leftRightInput == -1 && upDownInput == 0)
            {
                playerStatus = "run left";
            }
        }
    }
    private void PlayerAnimation()
    {
        if (playerStatus == "idle")
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunUp", false);
            animator.SetBool("isRunDown", false);
            animator.SetBool("isRunSide", false);
        }
        if (playerStatus == "run up")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunUp", true);
            animator.SetBool("isRunDown", false);
            animator.SetBool("isRunSide", false);
        }
        if (playerStatus == "run down")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunUp", false);
            animator.SetBool("isRunDown", true);
            animator.SetBool("isRunSide", false);
        }
        if (playerStatus == "run right")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunUp", false);
            animator.SetBool("isRunDown", false);
            animator.SetBool("isRunSide", true);
            spriteRenderer.flipX = false;
            
        }
        if (playerStatus == "run left")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunUp", false);
            animator.SetBool("isRunDown", false);
            animator.SetBool("isRunSide", true);
            spriteRenderer.flipX = true;
        }
        if (playerStatus == "interact")
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunUp", false);
            animator.SetBool("isRunDown", false);
            animator.SetBool("isRunSide", false);
            spriteRenderer.flipX = true;
        }
    }
    public void PlayerInteract()
    {
        animator.SetTrigger("triggerInteract");
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunUp", false);
        animator.SetBool("isRunDown", false);
        animator.SetBool("isRunSide", false);
    }
}
