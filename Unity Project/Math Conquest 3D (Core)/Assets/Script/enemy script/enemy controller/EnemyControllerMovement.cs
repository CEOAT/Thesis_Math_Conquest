using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EnemyControllerMovement : MonoBehaviour
{
    private Transform playerTransform;
    [HideInInspector] public bool isEnemyChasePlayer;
    private Vector3 enemyStartPosition;

    [Header("Enemy Type")]
    public EnemyType enemyType;
    public enum EnemyType
    {
        obstacle,
        chaseAndHit,
        chaser,
        shooterTower,
        shooterAndChase
    };

    [Header("Enemy Movement Setting")]
    public float enemyMoveSpeed;
    public float enemyChasingRange;

    [Header("Enemy Attack System")]
    public Transform enemyAttackPointTransform;
    public float enemyAttackTriggerRange;
    public float enemyAttackPointRange;
    public LayerMask enemyAttackLayerMask;
    [HideInInspector] private Collider[] playerInAttackCircle;
    public float enemyAttackDamage;

    [Header("Enemy Attack Interval")]
    [HideInInspector] public bool isEnemyCheckingAttack;
    public float enemyCheckingAttackIntervalTime;
    [HideInInspector] public bool isEnemyReadyToAttack;

    [Header("Enemy Attack Wait")]
    [HideInInspector] public bool isEnemyWaitFromAttack;
    [Tooltip("Set to 0 if want enemy to resume activity immediately after attack. If not, set time to be longer than enemy's attack animation")] 
    public float enemyAttackWaitTime;

    [Header("Enemy Recovery System")]
    [HideInInspector] public bool isEnemyWaitToRecover;
    public float enemyHurtRevoceryTime;

    [HideInInspector] public string enemyAnimationState;
    private Animator animator;

    [Header("Enemy Meterial")]
    public Material materialStart;
    public Material materialDamaged;

    [Header("Enemy Destroy Particle")]
    public GameObject particleDestroyedPrefab;

    [Header("Shooter Setting")]
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private float enemyShootDistance;
    [SerializeField] private Vector3 enemyShootPointOffset;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    private SpriteRenderer spriteRenderer;
    private EnemyControllerStatus EnemyStatus;

    [HideInInspector] public UnityEvent EventOnEnemyStartChase;
    [HideInInspector] public UnityEvent EventOnEnemyStopChase;
    [HideInInspector] public UnityEvent EventOnEnemyDead;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyStatus = GetComponent<EnemyControllerStatus>();
    }
    private void SetupEnemyType()
    {
        if (enemyType == EnemyType.obstacle)
        {
            enemyChasingRange = 0;
            enemyAttackTriggerRange = 0;
            enemyAttackPointRange = 0;
            Destroy(navMeshAgent);
            rigidbody.constraints = 
                  RigidbodyConstraints.FreezePositionX
                | RigidbodyConstraints.FreezePositionZ;
            rigidbody.freezeRotation = true;
        }
        else if (enemyType == EnemyType.chaser)
        {
            enemyAttackTriggerRange = 0;
            enemyAttackPointRange = 0;
            Destroy(navMeshAgent);
            rigidbody.useGravity = false;
        }
        else if (enemyType == EnemyType.shooterTower)
        {
            enemyMoveSpeed = 0;
        }
    }
    private void SetupEnemyStat()
    {
        enemyStartPosition = transform.position;
        navMeshAgent.speed = enemyMoveSpeed;
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
        EventOnEnemyStartChase.Invoke();
        isEnemyChasePlayer = true;
        playerTransform = player;
    }

    private void FixedUpdate()
    {
        if(enemyType == EnemyType.obstacle) 
        { 
            return; 
        }
        else if(enemyType ==  EnemyType.chaser)
        {
            if(playerTransform != null)
            {
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, enemyMoveSpeed * 0.01f);
            }
            EnemyCheckFacing();
        }
        else if((enemyType == EnemyType.chaseAndHit  ||
                enemyType == EnemyType.shooterTower ||
                enemyType == EnemyType.shooterAndChase) 
                && playerTransform != null)
        {
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
    }
    
    private void EnemyCheckFacing()
    {
        if(enemyType == EnemyType.chaseAndHit ||
            enemyType == EnemyType.shooterAndChase)
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
        else
        {
            if(transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if(transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs((transform.localScale.x)), transform.localScale.y, transform.localScale.z);
            }
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
            navMeshAgent.SetDestination(enemyStartPosition);
            EventOnEnemyStopChase.Invoke();
        }
    }
    public void EnemyStopChasePlayerDead()
    {
        if (navMeshAgent != null)
        {
            isEnemyChasePlayer = false;
            navMeshAgent.SetDestination(transform.position);
            EventOnEnemyStopChase.Invoke();
        }
    }
    public void EnemyDead()
    {
        EnemyStopChasePlayerDead();
        GetComponent<CapsuleCollider>().center = transform.position + new Vector3(0, 50, 0);
        Destroy(GetComponent<Rigidbody>());
        
        enemyAnimationState = "Enemy Dead";
        EnemyAnimationTrigger();
    }
    public void EnemyDeadDestroyOnAnimation()
    {
        Destroy(this.gameObject);
        CreateDestroyEffect();
        EventOnEnemyDead.Invoke();
    }
    private void CreateDestroyEffect()
    {
        if(particleDestroyedPrefab != null)
        {
            GameObject particleDestroyedObject = Instantiate( particleDestroyedPrefab, transform.position, transform.rotation);
            Destroy(particleDestroyedObject, 1f);
        }
    }

    private void EnemyAttackWait()
    {
        isEnemyWaitFromAttack = true;
        isEnemyReadyToAttack = false;
        navMeshAgent.speed = 0f;

        Invoke("EnemyAttackWaitComplete", enemyAttackWaitTime);
    }
    private void EnemyAttackWaitComplete()
    {
        isEnemyWaitFromAttack = false;
        isEnemyReadyToAttack = true;
        navMeshAgent.speed = enemyMoveSpeed;
    }
    public void EnemyHurtRecovery()
    {
        enemyAnimationState = "Enemy Hurt";
        EnemyAnimationTrigger();
        StartCoroutine(EnemyHurtColorSwitch());
        
        if(CheckIfEnemyNotDisrupatable()) { return; }

        isEnemyWaitToRecover = true;
        isEnemyReadyToAttack = false;
        navMeshAgent.speed = 0f;
        Invoke("EnemyHurtRecoveryComplete", enemyHurtRevoceryTime);
    }
    public bool CheckIfEnemyNotDisrupatable()
    {
        if(enemyType == EnemyType.chaser || enemyType == EnemyType.obstacle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Color enemyColorDefault = new Color(255, 255, 255, 255);
    private Color enemyColorRed = new Color(255, 0, 0, 255);
    private IEnumerator EnemyHurtColorSwitch()
    {
        spriteRenderer.material = materialDamaged;
        spriteRenderer.color = enemyColorRed;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = enemyColorDefault;
        spriteRenderer.material = materialStart;

    }
    private void EnemyHurtRecoveryComplete()
    {
        isEnemyWaitToRecover = false;
        isEnemyReadyToAttack = true;
        navMeshAgent.speed = enemyMoveSpeed;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
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
        if (playerTransform != null && isEnemyReadyToAttack == true
            && isEnemyWaitFromAttack == false && isEnemyWaitToRecover == false)
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
    private void EnemyAttackSendDamage()    // on animation event ** for melee/range enemy
    {
        playerInAttackCircle = Physics.OverlapSphere(
                enemyAttackPointTransform.transform.position,
                enemyAttackTriggerRange,
                enemyAttackLayerMask);
        foreach (Collider player in playerInAttackCircle)
        {
            if (player.tag == "Player" && player.GetComponent<CapsuleCollider>() != null)
            {
                if(enemyType == EnemyType.chaseAndHit)
                {
                    SendMeleeDamage(player);
                }
                else if(enemyType == EnemyType.shooterTower || enemyType == EnemyType.shooterAndChase)
                {
                    SendRangeDamage(player);
               }
            }
        }
    }
    private void SendMeleeDamage(Collider playerCollider)        // on animation event ** for range enemy
    {
        playerTransform.GetComponent<ExplorationModePlayerHealth>().PlayerTakenDamage(enemyAttackDamage);
    }
    private void SendRangeDamage(Collider playerCollider)
    {
        GameObject bulletObject = Instantiate(enemyBulletPrefab, enemyAttackPointTransform.position + CheckShootSide(), enemyAttackPointTransform.rotation);
        bulletObject.GetComponent<EnemyBullet>().SetupBulletVariable(enemyAttackDamage, playerCollider.transform);
    }
    private Vector3 CheckShootSide()
    {
        Vector3 ShootPoint = new Vector3();

        if(transform.position.x < playerTransform.position.x)
        {
            ShootPoint = new Vector3(enemyShootPointOffset.x, enemyShootPointOffset.y, enemyShootPointOffset.z) ;
        }
        else if(transform.position.x > playerTransform.position.x)
        {
            ShootPoint = new Vector3(-enemyShootPointOffset.x, enemyShootPointOffset.y, enemyShootPointOffset.z) ;
        }

        return ShootPoint;
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

    private void OnDestroy() 
    {
        CancelInvoke();
        StopAllCoroutines();
    }
}