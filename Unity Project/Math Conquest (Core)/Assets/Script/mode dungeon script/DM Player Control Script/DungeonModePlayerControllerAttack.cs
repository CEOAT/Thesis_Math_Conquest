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
    DungeonModePlayerControllerTargetSystem SwitchTarget;
    DungeonModePlayerControllerMovement Movement;
    
    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        playerInput = new MasterInput();
        SwitchTarget = GetComponent<DungeonModePlayerControllerTargetSystem>();
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

        if (SwitchTarget.selectedEnemyObject != null)
        {
            SwitchTarget.selectedEnemyObject.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(playerAnswerField.text, 10f);
            PlayerClearInputField();
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
