using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationModeEnemyControllerAction : MonoBehaviour
{
    [Header("Enemy Action Control")]
    public string enemySpawnType;
    public bool isEnemyActive;
    public bool isEnemyStartActionFirstTime;

    // ** Set by spawn script **
    [HideInInspector] public float enemyMoveSpeed;
    [HideInInspector] public float enemyDestinationDistancePlayer;
    [HideInInspector] public Vector2 enemyDestinationSpawnPoint;

    [Header("Enemy Cycling Time")]
    public float enemyChargeAttackTime;
    public float enemyHoldAttackTime;
    public float enemyWearDownTime;

    [Header("Enemy Interupt Boolean")]
    public bool canBeInteruptedDuringCharge;
    public bool canBeInteruptedDuringHold;
    public bool canBeInteruptedDuringAttack;

    private Transform playerTransform;
    private Animator Animator;
    private EnemyControllerStatus EnemyStatus;
    private EliminationModePlayerControllerAction PlayerAction;

    private void Start()
    {
        SetupEnemyComponent();
        SetupEnemyPosition();
    }
    private void SetupEnemyComponent()
    {
        Animator = GetComponent<Animator>();
        EnemyStatus = GetComponent<EnemyControllerStatus>();
        playerTransform = GameObject.FindGameObjectWithTag("Player Object").transform;
        PlayerAction = GameObject.FindGameObjectWithTag("Player System").GetComponent<EliminationModePlayerControllerAction>();
    }
    private void SetupEnemyPosition()
    {
        switch (enemySpawnType)
        {
            case "move in":
                {
                    transform.position = enemyDestinationSpawnPoint;
                    break;
                }
            case "wait":
                {
                    transform.position = enemyDestinationSpawnPoint;
                    break;
                }
        }
    }


    //---------------------------------- Enemy Action Cycle ----------------------------------//
    private void EnemyActionInitiate()
    {
        //start enemy's action sequence, call from player's script.
        //using invoke and ienumerator.

        Animator.SetTrigger("idle trigger");
        Animator.SetBool("wear down bool", false);
        EnemyStatus.enemyState = "charging";
        Invoke("EnemyAttackHolding", enemyChargeAttackTime);
    }
    private void EnemyAttackHolding()
    {
        Animator.SetTrigger("charge trigger");
        EnemyStatus.enemyState = "holding";
        Invoke("EnemyAttackPerform", enemyHoldAttackTime);
    }
    private void EnemyAttackPerform()
    {
        Animator.SetTrigger("attack trigger");
        EnemyStatus.enemyState = "attacking animation";
    }
    private void EnemyDamageExecute()
    {
        //run in enemy's attack animation.

        EnemyStatus.enemyState = "damage execute";
        PlayerAction.PlayerTakeDamage(EnemyStatus.enemyAttackDamage);
        EnemyActionInitiate();
    }
    private void EnemyWearDown()
    {
        CancelInvoke();
        Animator.ResetTrigger("idle trigger");
        Animator.SetBool("wear down bool", true);
        Invoke("EnemyActionInitiate", enemyWearDownTime);
        EnemyStatus.enemyState = "wear down";
    }
    private void EnemyActionReset()
    {
        CancelInvoke();
        EnemyActionInitiate();
    }

    //---------------------------------- Enemy Interuption Methods ----------------------------------//
    private void EnemyDamageTakenLight()
    {
        Animator.Play("test hurt light");
    }
    public void EnemyDamageTakenMedium()
    {
        Animator.Play("test hurt medium");
    }
    public void EnemyDamageTakenHeavy()
    {
        Animator.SetTrigger("hurt heavy trigger");
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 160, 140, 120), "Click"))
        {
            EnemyActionReset();
        }
    }


    private void FixedUpdate()
    {
        if (isEnemyActive == false)
        {
            if (enemySpawnType == "move in")
            {
                EnemyMoveIn();
            }
            if (enemySpawnType == "wait")
            {
                EnemyWait();
            }
        }
        if (isEnemyActive == true)
        {
            if (isEnemyStartActionFirstTime == true)
            {
                isEnemyStartActionFirstTime = false;
                EnemyActionInitiate();
            }

            EnemyInteruption();
        }
    }
    private void EnemyMoveIn()
    {
        if (Vector3.Distance(transform.position , playerTransform.position) > enemyDestinationDistancePlayer)
        {
            transform.Translate(Vector3.left * enemyMoveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, playerTransform.position) < enemyDestinationDistancePlayer)
        {
            isEnemyActive = true;
            isEnemyStartActionFirstTime = true;
        }
    }
    private void EnemyWait()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < enemyDestinationDistancePlayer)
        {
            isEnemyActive = true;
            isEnemyStartActionFirstTime = true;
        }
    }


    private void EnemyInteruption()
    {
        //interupted during charging attack
        if (EnemyStatus.isEnemyTakenDamage == true
            && canBeInteruptedDuringCharge == true
            && EnemyStatus.enemyState == "charging")
        {
            EnemyStatus.isEnemyTakenDamage = false;
            EnemyDamageTakenMedium();
            EnemyActionReset();
        }

        //interupted during holding attack
        if (EnemyStatus.isEnemyTakenDamage == true
            && canBeInteruptedDuringHold == true
            && EnemyStatus.enemyState == "holding")
        {
            EnemyStatus.isEnemyTakenDamage = false;
            EnemyDamageTakenMedium();
            EnemyActionReset();
        }

        //interupted during attack animation
        if (EnemyStatus.isEnemyTakenDamage == true
            && canBeInteruptedDuringAttack == true
            && EnemyStatus.enemyState == "attacking animation")
        {
            EnemyStatus.isEnemyTakenDamage = false;
            EnemyDamageTakenHeavy();
            EnemyWearDown();
            print("Attack Animation Interupted");
        }

        if (EnemyStatus.isEnemyTakenDamage == true)
        {
            EnemyStatus.isEnemyTakenDamage = false;
            EnemyDamageTakenLight();
            print("Light Reaction");
        }
    }
}
