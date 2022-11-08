using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerMovement : MonoBehaviour
{
    private Transform playerTransform;
    private bool isEnemyChasePlayer;
    private bool isEnemyDisabled;

    [Header("Enemy Movement Setting")]
    public float enemyMoveSpeed;

    [Header("Enemy Attack System")]
    public Transform enemyAttackPointTransform;
    public float enemyAttackPointRadius;
    public LayerMask enemyAttackLayerMask;

    private NavMeshAgent navMeshAgent;
    private EnemyControllerStatus EnemyStatus;

    private void Start()
    {
        SetupEnemyComponent();
    }
    private void SetupEnemyComponent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyStatus = GetComponent<EnemyControllerStatus>();
    }

    private void PlayerMovementDisabled()
    {
        isEnemyDisabled = true;
    }
    private void PlayerMovementEnabled()
    {
        isEnemyDisabled = false;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            EnemyStartChasePlayer(player.transform);
        }
    }
    private void EnemyStartChasePlayer(Transform player)
    {
        isEnemyChasePlayer = true;
        playerTransform = player;
    }

    private void FixedUpdate()
    {
        EnemyChasePlayer();
        EnemyCheckChaseRange();
    }
    private void EnemyChasePlayer()
    {
        if (isEnemyChasePlayer == true)
        {
            navMeshAgent.SetDestination(playerTransform.transform.position);
        }
    }
    private void EnemyCheckChaseRange()
    {

    }
    private void EnemyStopChasePlayer()
    {

    }

    private void OnCollisionEnter(Collision player)
    {
        if (player.collider.CompareTag("Player"))
        {
            EnemyAttack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyAttackPointTransform.transform.position, enemyAttackPointRadius);
    }
    private void EnemyAttack()
    {
        if (playerTransform != null)
        {
            Collider[] playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackPointRadius,
                enemyAttackLayerMask);

            foreach (Collider player in playerInAttackCircle)
            {
                if (player.tag == "Player")
                {
                    print("active attack");
                    EnemyPerformAttack();
                }
            }
        }
    }
    private void EnemyPerformAttack()
    {
        if (playerTransform != null)
        {
            Collider[] playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackPointRadius,
                enemyAttackLayerMask);

            foreach (Collider player in playerInAttackCircle)
            {
                if (player.tag == "Player")
                {
                    playerInAttackCircle[0].GetComponent<ExplorationModePlayerControllerMovement>().PlayerHurt();
                }
            }
        }
    }
}