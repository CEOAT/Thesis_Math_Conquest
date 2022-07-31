using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonModePlayerControllerAttack : MonoBehaviour
{
    private MasterInput playerInput;
    private PlayerControllerStatus PlayerStatus;

    public Transform playerAttackPoint;
    public float playerAttackRadius;
    public LayerMask playerAttackLayerMask;

    public TMP_InputField playerAnswerField;
    
    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        PlayerStatus = GetComponent<PlayerControllerStatus>();
        playerInput = new MasterInput();
    }
    private void SetupControl()
    {
        playerInput.PlayerControlDungeon.ConfirmAnswer.performed += context => PlayerAttack();
        playerInput.PlayerControlDungeon.ClearAnswer.performed += context => PlayerClearInputField();
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
        Gizmos.DrawWireSphere(playerAttackPoint.position, playerAttackRadius);
    }
    private void PlayerAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerAttackPoint.position, playerAttackRadius, playerAttackLayerMask);
        Transform closestEnemyTransform = null;
        float closestEnemyDistance = 0;

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            if (closestEnemyTransform == null)
            {
                closestEnemyTransform = hitEnemies[i].transform;
                closestEnemyDistance = Vector2.Distance(transform.position, hitEnemies[i].transform.position);
            }
            if (Vector2.Distance(transform.position, hitEnemies[i].transform.position) < closestEnemyDistance
                && hitEnemies.Length > 0)
            {
                closestEnemyTransform = hitEnemies[i].transform;
                closestEnemyDistance = Vector2.Distance(transform.position, hitEnemies[i].transform.position);
            }
        }

        if (closestEnemyTransform != null)
        {
            closestEnemyTransform.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(playerAnswerField.text, 10f);
            PlayerClearInputField();
        }
    }
    private void PlayerClearInputField()
    {
        playerAnswerField.text = "";
    }

    private void FixedUpdate()
    {
        playerAnswerField.ActivateInputField();
    }
}
