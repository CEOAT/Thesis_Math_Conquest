using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePlayerBuff : MonoBehaviour
{
    private ExplorationModePlayerControllerMovement PlayerMovement;
    private ExplorationModePlayerHealth PlayerHealth;
    private ExplorationModePlayerAttackSystem PlayerAttackSystem;

    private void Start()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        PlayerMovement = GetComponent<ExplorationModePlayerControllerMovement>();
        PlayerHealth = GetComponent<ExplorationModePlayerHealth>();
        PlayerAttackSystem = transform.GetChild(0).GetComponent<ExplorationModePlayerAttackSystem>();
    }

    public void RestoreHealth(float healthRestoreValue)
    {
        PlayerHealth.PlayerStoreHealth(healthRestoreValue);
    }
    public void IncreaseMaxHealth(float healthMaxIncreaseValue)
    {
        PlayerHealth.playerHealthMaximum += healthMaxIncreaseValue;
        PlayerHealth.PlayerStoreHealth(healthMaxIncreaseValue);
    }
    public void RestoreFullHealth()
    {
        PlayerHealth.PlayerStoreHealth(PlayerHealth.playerHealthMaximum);
    }
    public void IncreaseMoveSpeed(float moveSpeedIncreaseValue)
    {
        PlayerMovement.playerWalkSpeed += moveSpeedIncreaseValue;
    }
    public void IncreaseDamage(float damageIncreaseValue)
    {
        PlayerAttackSystem.playerAttackDamage += damageIncreaseValue;
    }
}
