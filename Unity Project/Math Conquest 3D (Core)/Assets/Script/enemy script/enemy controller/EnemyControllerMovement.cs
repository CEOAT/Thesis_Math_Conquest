using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerMovement : MonoBehaviour
{
    private Transform playerTransform;
    private bool isEnemyChasePlayer;

    [Header("Enemy Type")]
    public EnemyType enemyType;
    public enum EnemyType
    {
        obstacle,
        chaseAndHit
    };

    [Header("Enemy Movement Setting")]
    public float enemyMoveSpeed;
    public SphereCollider enemyDetectionSphere;
    public float enemyDetectionRange;
    public float enemyChasingRange;

    [Header("Enemy Attack System")]
    public Transform enemyAttackPointTransform;
    public float enemyAttackTriggerRange;
    public float enemyAttackPointRange;
    public LayerMask enemyAttackLayerMask;
    private Collider[] playerInAttackCircle;
    public float enemyAttackDamage;

    [Header("Enemy Attack Interval")]
    public bool isEnemyCheckingAttack;
    public float enemyCheckingAttackIntervalTime;
    public bool isEnemyWaitToAttack;
    public float enemyAttackWaitTime;

    [Header("Enemy Recovery System")]
    public bool isEnemyWaitToRecover;
    public float enemyHurtRevoceryTime;

    private NavMeshAgent navMeshAgent;
    private EnemyControllerStatus EnemyStatus;

    private void Start()
    {
        SetupEnemyComponent();
        SetupEnemyType();
        SetupEnemyStat();
    }
    private void SetupEnemyComponent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyStatus = GetComponent<EnemyControllerStatus>();
    }
    private void SetupEnemyType()
    {
        switch (enemyType)
        {
            case EnemyType.obstacle:
            {
                enemyDetectionRange = 0;
                enemyChasingRange = 0;
                enemyAttackTriggerRange = 0;
                enemyAttackPointRange = 0;
                Destroy(navMeshAgent);
                break;
            }
        }
    }
    private void SetupEnemyStat()
    {
        navMeshAgent.speed = enemyMoveSpeed;
        enemyDetectionSphere.radius = enemyDetectionRange;
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
        if (isEnemyChasePlayer == true)
        {
            if (isEnemyWaitToAttack == false && isEnemyWaitToRecover == false)
            {
                EnemyChasePlayer();
                EnemyAttackCheckEnabled();
                if (isEnemyCheckingAttack == true)
                {
                    isEnemyCheckingAttack = false;
                    EnemyAttackCheckEnabled();
                }
            }
            EnemyCheckChaseRange();
        }
    }
    private void EnemyChasePlayer()
    {
        navMeshAgent.SetDestination(playerTransform.transform.position);
    }
    private void EnemyCheckChaseRange()
    {
        if (Vector3.Distance(playerTransform.position, this.transform.position) > enemyChasingRange)
        {
            EnemyStopChasePlayer();
            EnemyAttackCheckDisabled();
        }
    }
    private void EnemyStopChasePlayer()
    {
        isEnemyChasePlayer = false;
    }

    private void EnemyAttackWait()
    {
        
    }
    private void EnemyHurtRecovery()
    {

    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyChasingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttackTriggerRange);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(enemyAttackPointTransform.transform.position, enemyAttackPointRange);
    }
    #endif

    private void EnemyAttackCheckEnabled()
    {
        InvokeRepeating("EnemyAttackPerform", 0f, enemyCheckingAttackIntervalTime);
    }
    private void EnemyAttackCheckDisabled()
    {
        CancelInvoke("EnemyAttackPerform");
    }
    private void EnemyAttackPerform()
    {
        if (playerTransform != null)
        {
            playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackPointRange,
                enemyAttackLayerMask);

            foreach (Collider player in playerInAttackCircle)
            {
                if (player.tag == "Player")
                {
                    // implement animation here

                    print("active attack");
                    EnemyAttackSendDamage();
                }
            }
        }
    }
    private void EnemyAttackSendDamage()   // this will bec called in animation event
    {
        playerInAttackCircle[0].GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(enemyAttackDamage);
    }
}