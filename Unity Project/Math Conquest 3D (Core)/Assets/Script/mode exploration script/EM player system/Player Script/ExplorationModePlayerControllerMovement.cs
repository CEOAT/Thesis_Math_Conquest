using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerControllerMovement : MonoBehaviour
{
    [Header("Player Movement Setting")]
    public float playerMoveSpeed = 5f;
    public float playerWalkSpeed = 5f;
    public float playerRunSpeed = 10f;
    public bool isBoostSpeed = false;

    [Header("Player Interaction")]
    public Transform playerRaycastPoint;

    [Header("Player Ground System")]
    [SerializeField] private float playerGroudRaycastRange;
    private Ray groundCheckRay;
    private RaycastHit groundRayCastHit;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Player Status")]
    public string playerStatus = "player status";
    public bool canControlCharacter = false;

    [Header("Player Recovery System")]
    public float playerRecoveryTimeUsed;
    public float playerRecoveryTimeCount;

    [Header("Player In-Game UI")]
    public Animator InGameUiAnimator;

    [Header("Player Invincible System")]
    public Material playerMaterialStart;
    public Material playerMaterialDamaged;
    public GameObject playerInvincibleShieldPrefab;

    private MasterInput playerInput;
    private float upDownInput;
    private float leftRightInput;
    private float runInput;

    private Rigidbody rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        SetupConponent();
        SetupControl();
        PlayerEnabledMovement();
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
            PlayerFloating();
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
            rigidbody.AddForce(transform.forward * Time.deltaTime * playerMoveSpeed);
            playerStatus = "run up";
        }
        if (upDownInput == -1)
        {
            rigidbody.AddForce(-transform.forward * Time.deltaTime * playerMoveSpeed);
            playerStatus = "run down";
        }
        if (leftRightInput == 1)
        {
            rigidbody.AddForce(transform.right * Time.deltaTime * playerMoveSpeed);
            playerRaycastPoint.transform.rotation = Quaternion.Euler(0, 90, 0);
            if (leftRightInput == 1 && upDownInput == 0)
            {
                playerStatus = "run right";
            }
        }
        if (leftRightInput == -1)
        {
            rigidbody.AddForce(-transform.right * Time.deltaTime * playerMoveSpeed);
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

    private bool isRayLeftOnGround;
    private bool isRayRightOnGround;
    private void PlayerFloating()
    {
        CheckRayLeft();
        CheckRayRight();
        CheckIfPlayerFloating();
    }
    private void CheckRayLeft()
    {
        groundCheckRay = new Ray(transform.position, Vector3.down + (Vector3.left * 0.5f));
        if(Physics.Raycast(groundCheckRay, out groundRayCastHit, playerGroudRaycastRange, groundLayerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(groundCheckRay.origin, groundRayCastHit.point, Color.green);
            isRayLeftOnGround = false;
        }

        else
        {
            Debug.DrawLine(groundCheckRay.origin, groundCheckRay.origin + groundCheckRay.direction * playerGroudRaycastRange, Color.red);
            isRayLeftOnGround = true;
        }
    }
    private void CheckRayRight()
    {
        groundCheckRay = new Ray(transform.position, Vector3.down + (Vector3.right * 0.5f));
        if(Physics.Raycast(groundCheckRay, out groundRayCastHit, playerGroudRaycastRange, groundLayerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(groundCheckRay.origin, groundRayCastHit.point, Color.green);
            isRayRightOnGround = false;
        }

        else
        {
            Debug.DrawLine(groundCheckRay.origin, groundCheckRay.origin + groundCheckRay.direction * playerGroudRaycastRange, Color.red);
            isRayRightOnGround = true;
        }
    }
    private void CheckIfPlayerFloating()
    {
        if(isRayLeftOnGround == false && isRayRightOnGround == false)
        {
            animator.SetBool("isFloating", false);
        }
        else if(isRayLeftOnGround == true && isRayRightOnGround == true)
        {
            animator.SetBool("isFloating", true);
        }
    }

    private int attackCount = 1;
    public void PlayerAttackPerform()
    {
        animator.SetTrigger("triggerAttack");
        CountAttack();
        PlayerWait();
        PlayerDisabledMovement();
    }
    private void CountAttack()
    {
        animator.SetInteger("intAttackCount", attackCount);
        attackCount++;
        if(attackCount > 3) { attackCount = 1; }
    }

    [Header("Mirror Setting")]
    [SerializeField] private bool isPlayerMirror = false;

    public void PlayerCheckFacingTarget(Transform targetEnemy)
    {
        if(!isPlayerMirror)
        {
            if (targetEnemy.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else if (targetEnemy.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (targetEnemy.position.x < transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (targetEnemy.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
    public void PlayerAttackCancle()
    {
        PlayerEnabledMovement();
    }

    public void PlayerHurt()
    {
        animator.SetTrigger("triggerHurt");
        InGameUiAnimator.SetTrigger("triggerUiCanvasShake");
        PlayerWait();
        StopCoroutine("PlayerHurtColorSwitch");
        StartCoroutine(PlayerHurtColorSwitch());
    }
    private Color playerColorDefault = new Color(255, 255, 255, 255);
    private Color playerColorRed = new Color(255, 0, 0, 255);
    private IEnumerator PlayerHurtColorSwitch()
    {
        spriteRenderer.material = playerMaterialDamaged;
        spriteRenderer.color = playerColorRed;

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = playerColorDefault;
        spriteRenderer.material = playerMaterialStart;

        GameObject playerShield = Instantiate(playerInvincibleShieldPrefab, transform.position + (Vector3.back * 0.2f), playerInvincibleShieldPrefab.transform.rotation);
        playerShield.transform.parent = transform;
        playerShield.GetComponent<ExplorationModePlayerParticleEffect>().particleLifeTime = GetComponent<ExplorationModePlayerHealth>().playerInvincibleTimeMax;
    }
    public void PlayerInvincibleEffectCancle()
    {
        StopCoroutine("PlayerHurtColorSwitch");
    }
    public void PlayerHurtCancle()
    {
        PlayerEnabledMovement();
    }

    public void PlayerDead()
    {
        animator.SetTrigger("triggerDead");
        Destroy(rigidbody);
        GetComponent<CapsuleCollider>().center = transform.position + new Vector3(0, 50, 0);

        CheckPlayerDeadFacing();
        PlayerWait();
    }
    public void CheckPlayerDeadFacing()
    {
        if (spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    public void PlayerWait()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunUp", false);
        animator.SetBool("isRunDown", false);
        animator.SetBool("isRunSide", false);
        PlayerDisabledMovement();
    }

    public void PlayerDisabledMovement()
    {
        canControlCharacter = false;
        playerInput.Disable();
    }
    public void PlayerEnabledMovement()
    {
        canControlCharacter = true;
        playerInput.Enable();
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