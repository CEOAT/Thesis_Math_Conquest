using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyComponentHealthModification : MonoBehaviour
{
    [Header("Health Drain Setting")]
    [SerializeField] private bool isHealthDrain = false;
    [ShowIf("isHealthDrain")] [SerializeField] private bool isStartDrainOnEncounterPlayer = true;
    [ShowIf("isHealthDrain")] [SerializeField] private float healthDrainInterval = 1f;
    [ShowIf("isHealthDrain")] [SerializeField] private float healthDrainValue = 1f;

    [Header("Health Regen Setting")]
    [SerializeField] private bool isHealthRegen = false;
    [ShowIf("isHealthRegen")] [SerializeField] private bool isStartRegenOnEncounterPlayer = true;
    [ShowIf("isHealthRegen")] [SerializeField] private float healthRegenInterval = 1f;
    [ShowIf("isHealthRegen")] [SerializeField] private float healthRegenValue = 1f;

    public enum StopChaseModifyType
    {
        none, restoreHealht, depleteHealth
    }
    [Header("Heal After Stop Chase")]
    [EnumToggleButtons] [HideLabel]
    public StopChaseModifyType stopChaseModifyType;

    [Header("Zero HP After Collision")]
    [SerializeField] private bool isHealthRemovedWhenCollide = false;

    private EnemyControllerStatus EnemyHealth;
    private EnemyControllerMovement EnemyController;

    private void Start()
    {
        SetupComponent();
        SetupSubscription();
    }
    private void SetupComponent()
    {
        EnemyHealth = GetComponent<EnemyControllerStatus>();
        EnemyController = GetComponent<EnemyControllerMovement>();
    }
    private void SetupSubscription()
    {
        if(isHealthDrain == false && isHealthRegen == false) { return; }
        EnemyController.EventOnEnemyStartChase.AddListener(StartHealthModify);
        EnemyController.EventOnEnemyStopChase.AddListener(StopHealthModify);
    }

    private void OnDestroy() 
    {
        SetupUnsubcription();
    }
    private void SetupUnsubcription()
    {
        if(isHealthDrain == false && isHealthRegen == false) { return; }
        EnemyController.EventOnEnemyStartChase.RemoveListener(StartHealthModify);
        EnemyController.EventOnEnemyStopChase.RemoveListener(StopHealthModify);
    }

    private void StartHealthModify()
    {
        if(isHealthDrain == true)
        {
            InvokeRepeating("HealthDrain", 0f, healthDrainInterval);
        }
        if(isHealthRegen == true)
        {
            InvokeRepeating("HealthRegen", 0f, healthRegenInterval);
        }
    }
    private void StopHealthModify()
    {
        if(isHealthDrain == true)
        {
            CancelInvoke("HealthDrain");
        }
        if(isHealthRegen == true)
        {
            CancelInvoke("HealthRegen");
        }
    }

    private void HealthDrain()
    {

        if(isStartDrainOnEncounterPlayer && EnemyController.isEnemyChasePlayer && EnemyHealth.enemyHealthCurrent > 0)
        {
            EnemyHealth.enemyHealthCurrent -= healthDrainValue;
        }
        else if(!isStartDrainOnEncounterPlayer && EnemyHealth.enemyHealthCurrent > 0)
        {
            EnemyHealth.enemyHealthCurrent -= healthDrainValue;
        }
    }
    private void HealthRegen()
    {
        if(isStartRegenOnEncounterPlayer && EnemyController.isEnemyChasePlayer && EnemyHealth.enemyHealthCurrent < EnemyHealth.enemyHealthMax)
        {
            EnemyHealth.enemyHealthCurrent += healthRegenValue;
        }
        else if(!isStartRegenOnEncounterPlayer && EnemyHealth.enemyHealthCurrent < EnemyHealth.enemyHealthMax)
        {
            EnemyHealth.enemyHealthCurrent += healthRegenValue;
        }
    }

    private void OnTriggerExit(Collider player) 
    {
        if(player.tag == "Player")
        {
            if(stopChaseModifyType == StopChaseModifyType.restoreHealht)
            {
                HealthRestore();
            }
            if(stopChaseModifyType == StopChaseModifyType.depleteHealth)
            {
                DepleteHealth();
            }
        }
    }
    private void HealthRestore()
    {
        EnemyHealth.enemyHealthCurrent = EnemyHealth.enemyHealthMax;
    }
    private void DepleteHealth()
    {
        EnemyHealth.enemyHealthCurrent = 0;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(CheckIfEnemyCollidePlayerOrGround(other))
        {
            EnemyHealth.enemyHealthCurrent = 0;
        }
    }
    private bool CheckIfEnemyCollidePlayerOrGround(Collision collision)
    {
        return isHealthRemovedWhenCollide && (collision.collider.tag == "Player" || collision.collider.tag == "Trap");
    }
}