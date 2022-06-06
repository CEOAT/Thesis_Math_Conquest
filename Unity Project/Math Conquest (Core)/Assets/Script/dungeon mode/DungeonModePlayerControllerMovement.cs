using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonModePlayerControllerMovement : MonoBehaviour
{
    public float playerMoveSpeed;
    
    public float playerJumpForce;
    public bool canPlayerJumpRay = false;
    public bool canPlayerJumpCollider = false;

    private RaycastHit2D raycastHit2D;
    public float ray2DRange;
    public LayerMask interactableLayer;

    public Transform playerAnswerText;

    Rigidbody2D rigidbody2D;
    MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = new MasterInput();
    }
    private void SetupControl()
    {
        playerInput.PlayerControlDungeon.Jump.performed += context => PlayerJump();
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
        PlayerRayCast();
        PlayerMoveLeftRight();
    }
    private void PlayerRayCast()
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

    private void PlayerJump()
    {
        if (canPlayerJumpRay && canPlayerJumpCollider)
        {
            rigidbody2D.AddForce(Vector2.up * playerJumpForce);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
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