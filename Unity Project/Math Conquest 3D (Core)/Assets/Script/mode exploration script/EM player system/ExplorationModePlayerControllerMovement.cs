using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerControllerMovement : MonoBehaviour
{
    public bool canControlCharacter = false;

    public float playerMoveSpeed = 5f;
    public float playerWalkSpeed = 5f;
    public float playerRunSpeed = 10f;
    public bool isBoostSpeed = false;

    public Transform playerRaycastPoint;

    private float upDownInput;
    private float leftRightInput;
    private float runInput;

    public string playerStatus;

    private MasterInput playerInput;
    private Rigidbody rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ExplorationModePlayerControllerMovement PlayerMoverment;

    private void Awake()
    {
        SetupConponent();
        SetupControl();

        PlayerAllowedMovement();
    }
    private void SetupConponent()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerMoverment = GetComponent<ExplorationModePlayerControllerMovement>();
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerMoveSpeed = playerRunSpeed;
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
        if (canControlCharacter == true)
        {
            PlayerIdle();
            PlayerMove();
            PlayerRun();
            PlayerAnimation();
        }
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
    private void PlayerRun()
    {
        runInput = playerInput.PlayerControlExploration.Run.ReadValue<float>();
        if (runInput == 1)
        {
            playerMoveSpeed = playerRunSpeed;
        }
        else
        {
            playerMoveSpeed = playerWalkSpeed;
        }
    }

    public void PlayerHurt()   // temporary disable player's movement
    {
        // player animation hurt
        // PlayerDisabledMovement();
    }
    public void PlayerDead()   // disable all player action. game controller - disable pause button, player controller - diable all movement
    {
        // player animation dead
        PlayerDisabledMovement();
    }
    public void PlayerDisabledMovement()
    {
        canControlCharacter = false;
    }
    public void PlayerAllowedMovement()
    {
        canControlCharacter = true;
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
