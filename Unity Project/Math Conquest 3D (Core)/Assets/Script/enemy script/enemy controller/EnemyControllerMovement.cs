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
    [SerializeField] private Collider[] playerInAttackCircle;
    public float enemyAttackDamage;

    [Header("Enemy Attack Interval")]
    public bool isEnemyCheckingAttack;
    public float enemyCheckingAttackIntervalTime;
    public bool isEnemyReadyToAttack;

    [Header("Enemy Attack Wait")]
    public bool isEnemyWaitFromAttack;
    [Tooltip("Set to 0 if want enemy to resume activity immediately after attack. If not, set time to be longer than enemy's attack animation")] 
    public float enemyAttackWaitTime;

    [Header("Enemy Recovery System")]
    public bool isEnemyWaitToRecover;
    public float enemyHurtRevoceryTime;

    [Header("Enemy Animation")]
    public string enemyAnimationState;
    private Animator animator;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    private EnemyControllerStatus EnemyStatus;


    private void Start()
    {
        SetupEnemyComponent();
        SetupEnemyType();
        SetupEnemyStat();
    }
    private void SetupEnemyComponent()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
                rigidbody.constraints = 
                      RigidbodyConstraints.FreezePositionX
                    | RigidbodyConstraints.FreezePositionZ;
                break;
            }
        }
    }
    private void SetupEnemyStat()
    {
        navMeshAgent.speed = enemyMoveSpeed;
        enemyDetectionSphere.radius = enemyDetectionRange;
        isEnemyReadyToAttack = true;
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
        if(enemyType == EnemyType.obstacle) { return; }

        EnemyCheckFacing();
        EnemyAnimationUpdate();

        if (isEnemyChasePlayer == true)
        {
            if (isEnemyWaitFromAttack == false && isEnemyWaitToRecover == false)
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
    private void EnemyCheckFacing()
    {
        if (navMeshAgent.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            enemyAnimationState = "Enemy Walk";
        }
        if (navMeshAgent.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs((transform.localScale.x)), transform.localScale.y, transform.localScale.z);
            enemyAnimationState = "Enemy Walk";
        }
        if (navMeshAgent.velocity.x == 0 && navMeshAgent.velocity.y == 0)
        {
            enemyAnimationState = "Enemy Idle";
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
    public void EnemyStopChasePlayer()
    {
        if (navMeshAgent != null)
        {
            isEnemyChasePlayer = false;
            navMeshAgent.SetDestination(transform.position);
        }
    }
    public void EnemyDead()
    {
        EnemyStopChasePlayer();
        GetComponent<CapsuleCollider>().center = transform.position + new Vector3(0, 50, 0);
        Destroy(GetComponent<Rigidbody>());

        enemyAnimationState = "Enemy Dead";
        EnemyAnimationTrigger();
    }
    public void EnemyDeadDestroyOnAnimation()
    {
        Destroy(this.gameObject);
    }

    private void EnemyAttackWait()
    {
        isEnemyWaitFromAttack = true;
        isEnemyReadyToAttack = false;
        navMeshAgent.speed = 0f;

        if (enemyAttackWaitTime >= 1)    // make sure wait time is longer than attack animation if not set to 0
        {
            Invoke("EnemyAttackWaitComplete", enemyAttackWaitTime);
        }
    }
    private void EnemyAttackWaitComplete()
    {
        if(enemyAttackWaitTime >= 1) { return; }

        isEnemyWaitFromAttack = false;
        isEnemyReadyToAttack = true;
        navMeshAgent.speed = enemyMoveSpeed;
    }
    public void EnemyHurtRecovery()
    {
        enemyAnimationState = "Enemy Hurt";
        EnemyAnimationTrigger();

        isEnemyWaitToRecover = true;
        isEnemyReadyToAttack = false;
        navMeshAgent.speed = 0f;

        if (enemyAttackWaitTime >= 1)    // make sure wait time is longer than hurt animation if not set to 0
        {
            Invoke("EnemyHurtRecoveryComplete", enemyHurtRevoceryTime);
        }
    }
    private void EnemyHurtRecoveryComplete()
    {
        if (enemyHurtRevoceryTime >= 1) { return; }

        isEnemyWaitToRecover = false;
        isEnemyReadyToAttack = true;
        navMeshAgent.speed = enemyMoveSpeed;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyChasingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyAttackPointTransform.transform.position, enemyAttackTriggerRange);
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
        if (playerTransform != null && isEnemyReadyToAttack == true)
        {
            playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackPointRange,
                enemyAttackLayerMask);
            
            foreach (Collider player in playerInAttackCircle)
            {
                if (player.tag == "Player")
                {
                    enemyAnimationState = "Enemy Attack";
                    EnemyAnimationTrigger();
                    EnemyAttackWait();
                }
            }
        }
    }
    private void EnemyAttackSendDamage()    // on animation event
    {
        playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackTriggerRange,
                enemyAttackLayerMask);
        foreach (Collider player in playerInAttackCircle)
        {
            if (player.tag == "Player" && player.GetComponent<CapsuleCollider>() != null)
            {
                player.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(enemyAttackDamage);
            }
        }
    }

    private void EnemyAnimationUpdate()
    {
        if (enemyAnimationState == "Enemy Idle")
        {
            animator.SetBool("boolEnemyIdle", true);
            animator.SetBool("boolEnemyWalk", false);
        }
        if (enemyAnimationState == "Enemy Walk")
        {
            animator.SetBool("boolEnemyIdle", false);
            animator.SetBool("boolEnemyWalk", true);
        }
    }
    private void EnemyAnimationTrigger()
    {
        if (enemyAnimationState == "Enemy Attack")
        {
            animator.SetTrigger("triggerEnemyAttack");
        }
        else if (enemyAnimationState == "Enemy Dead")
        {
            animator.SetTrigger("triggerEnemyDead");
        }
        else if (enemyAnimationState == "Enemy Hurt")
        {
            animator.SetTrigger("triggerEnemyHurt");
        }
    }
}