using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonModePlayerControllerAttack : MonoBehaviour
{
    MasterInput playerInput;
    
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
        
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlExploration.Attack.performed += context => PlayerAttack();

    }
    private void Start()
    {
        
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
        float closetEnemyDistance = 0;

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            if (closestEnemyTransform == null)
            {
                closestEnemyTransform = hitEnemies[i].transform;
                closetEnemyDistance = Vector2.Distance(transform.position, hitEnemies[i].transform.position);
            }
            if (Vector2.Distance(transform.position, hitEnemies[i].transform.position) < closetEnemyDistance
                && hitEnemies.Length > 0)
            {
                closestEnemyTransform = hitEnemies[i].transform;
                closetEnemyDistance = Vector2.Distance(transform.position, hitEnemies[i].transform.position);
            }
        }

        //send damage to the enemy
        
        print(closestEnemyTransform);
        print(closetEnemyDistance);
    }

    private void FixedUpdate()
    {
        playerAnswerField.ActivateInputField();
    }
}
