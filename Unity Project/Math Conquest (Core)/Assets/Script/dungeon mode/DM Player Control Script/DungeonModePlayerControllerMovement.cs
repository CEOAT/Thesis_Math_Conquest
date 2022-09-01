using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModePlayerControllerMovement : MonoBehaviour
{
    /* 
        - contain most movement function like run, jump, dash
        - control every animations of player
        - control player's stun and hurt effect
    
    */

    public float playerMoveSpeed;

    public float playerJumpForce;
    public bool canPlayerJumpRay = false;
    public bool canPlayerJumpCollider = false;

    [SerializeField] private bool isPlayerDash;
    public float playerDashDuration = 0.3f;
    [SerializeField] private float playerDashTimeCount = 0f;
    public float playerDashSpeed = 50f;

    private RaycastHit2D raycastHit2D;
    public float ray2DRange;
    public LayerMask interactableLayer;

    public Transform playerAnswerText;

    public string playerStatus;

    Rigidbody2D rigidbody2D;
    MasterInput playerInput;
    Animator animator;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = new MasterInput();
        animator = GetComponent<Animator>();
    }
    private void SetupControl()
    {
        playerInput.PlayerControlDungeon.Jump.performed += context => PlayerJump();
        playerInput.PlayerControlDungeon.WeaponArt.performed += context => PlayerDash();
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
        PlayerJumpRayCast();
        PlayerMoveLeftRight();
        PlayerDashFuction();
        PlayerAnimationCheck();
        PlayerAnimation();
    }
    private void PlayerJumpRayCast()
    {
        raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, ray2DRange, interactableLayer);   //cast ray to floor, check jumping condition

        if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Ground"))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * ray2DRange, Color.red);
            canPlayerJumpRay = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * ray2DRange, Color.green);
            canPlayerJumpRay = false;
        }
    }
    private void PlayerMoveLeftRight()
    {
        if (!isPlayerDash)
        {
            float playerLeftRightInput = playerInput.PlayerControlDungeon.MoveLeftRight.ReadValue<float>();

            if (playerLeftRightInput == 1)
            {
                transform.Translate(Vector3.right * playerMoveSpeed * Time.deltaTime);
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                playerAnswerText.localScale = new Vector2(Mathf.Abs(playerAnswerText.localScale.x), playerAnswerText.localScale.y);
            }
            if (playerLeftRightInput == -1)
            {
                transform.Translate(-Vector3.right * playerMoveSpeed * Time.deltaTime);
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                playerAnswerText.localScale = new Vector2(-Mathf.Abs(playerAnswerText.localScale.x), playerAnswerText.localScale.y);
            }
        }
    }
    private void PlayerAnimationCheck()
    {
        float playerLeftRightInput = playerInput.PlayerControlDungeon.MoveLeftRight.ReadValue<float>();
        if (playerLeftRightInput != 1 && playerLeftRightInput != -1
            && (canPlayerJumpRay == true && canPlayerJumpCollider == true))
        {
            playerStatus = "idle";
        }
        if ((playerLeftRightInput == 1 || playerLeftRightInput == -1)
            && (canPlayerJumpRay == true && canPlayerJumpCollider == true))
        {
            playerStatus = "run";
        }
        ;       
        if (canPlayerJumpRay == false && canPlayerJumpCollider == false)
        {
            playerStatus = "drop";
        }
    }
    private void PlayerAnimation()
    {
        if (playerStatus == "idle")
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRun", false);
            animator.SetBool("isDrop", false);
        }
        if (playerStatus == "run")
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRun", true);
            animator.SetBool("isDrop", false);
        }
        if (playerStatus == "drop")
        {
            animator.SetBool("isDrop", true);
        }
    }

    private void PlayerJump()
    {
        if (canPlayerJumpRay && canPlayerJumpCollider)
        {
            rigidbody2D.AddForce(Vector2.up * playerJumpForce);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.y, 0f);
            animator.SetTrigger("triggerJump");
        }
    }
    public void PlayerAttack()
    {
        animator.SetTrigger("triggerSlash");
    }
    public void PlayerHurt()
    {
        animator.SetTrigger("triggerHurt");
    }
    private void PlayerDash()
    {
        if (isPlayerDash == false)
        {
            isPlayerDash = true;
        }
    }
    private void PlayerDashFuction()
    {
        if (isPlayerDash == true && playerDashTimeCount < playerDashDuration)
        {
            if (transform.localScale.x > 0)
            {
                transform.Translate(Vector3.right * playerDashSpeed * Time.deltaTime);
            }
            if (transform.localScale.x < 0)
            {
                transform.Translate(Vector3.left * playerDashSpeed * Time.deltaTime);
            }
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.y, 0f);
            playerDashTimeCount += Time.deltaTime;
            animator.SetBool("isDash",true);
        }
        if (isPlayerDash == true && playerDashTimeCount >= playerDashDuration)
        {
            isPlayerDash = false;
            playerDashTimeCount = 0f;
            animator.SetBool("isDash", false);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canPlayerJumpCollider = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canPlayerJumpCollider = false;
        }
    }
}
