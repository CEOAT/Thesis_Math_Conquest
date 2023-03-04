using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponentHealthModification : MonoBehaviour
{
    [Header("Health Drain Setting")]
    [SerializeField] private bool isHealthDrain = false;
    [SerializeField] private float healthDrainInterval = 1f;
    [SerializeField] private float healthDrainValue = 1f;

    [Header("Health Regen Setting")]
    [SerializeField] private bool isHealthRegen = false;
    [SerializeField] private float healthRegenInterval = 1f;
    [SerializeField] private float healthRegenValue = 1f;

    [Header("Zero HP After Collision")]
    [SerializeField] private bool isHealthRemovedWhenCollide = false;


    private EnemyControllerStatus EnemyHealth;

    private void Start()
    {
        SetupComponent();
        CheckModificationOption();
    }
    private void SetupComponent()
    {
        EnemyHealth = GetComponent<EnemyControllerStatus>();
    }
    private void CheckModificationOption()
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

    private void HealthDrain()
    {
        if(EnemyHealth.enemyHealthCurrent > 0)
        {
            EnemyHealth.enemyHealthCurrent -= healthDrainValue;
        }
    }
    private void HealthRegen()
    {
        if(EnemyHealth.enemyHealthCurrent < EnemyHealth.enemyHealthMax)
        {
            EnemyHealth.enemyHealthCurrent += healthRegenValue;
        }
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