using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonModePlayerControllerAttack : MonoBehaviour
{
    //InputSystem
    private MasterInput playerInput;

    //Melee Attack System
    public Transform playerAttackPoint;
    public float playerAttackRadius;
    public LayerMask playerAttackLayerMask;

    //Input Field
    public TMP_InputField playerAnswerField;

    //Script In Same Object
    DungeonModePlayerControllerTargetSystem TargetSystem;
    DungeonModePlayerControllerMovement Movement;
    
    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        playerInput = new MasterInput();
        TargetSystem = GetComponent<DungeonModePlayerControllerTargetSystem>();
        Movement = GetComponent<DungeonModePlayerControllerMovement>();
    }
    private void SetupControl()
    {
        playerInput.PlayerControlDungeon.ConfirmAnswer.performed += context => PlayerAttack();
        playerInput.PlayerControlDungeon.ClearAnswer.performed += context => PlayerClearInputField();
        playerInput.PlayerControlDungeon.SwitchTarget.performed += context => PlayerSwitchTarget();
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
        Movement.PlayerAttack();

        if (TargetSystem.selectedEnemyObject != null)
        {
            Collider2D[] enemyInAttackCircle = Physics2D.OverlapCircleAll(playerAttackPoint.position, 
            playerAttackRadius,
            playerAttackLayerMask);
            
            foreach (Collider2D enemy in enemyInAttackCircle)
            {
                if (TargetSystem.enemyList.Contains(enemy.transform) == true)
                {
                    TargetSystem.selectedEnemyObject.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(playerAnswerField.text, 50f);
                    PlayerClearInputField();
                }
            }
        }
    }
    private void PlayerClearInputField()
    {
        playerAnswerField.text = "";
    }

    private void PlayerSwitchTarget()
    {
        //will implement later
    }

    private void FixedUpdate()
    {
        playerAnswerField.ActivateInputField();
    }
}
