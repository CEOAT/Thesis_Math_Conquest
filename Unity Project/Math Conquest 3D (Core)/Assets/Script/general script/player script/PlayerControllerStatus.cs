using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStatus : MonoBehaviour
{
    [Header("Player Status")]
    [Tooltip("Current status of the player. Idle, attacking, stunning, etc.")]public string playerCurrentState;
    
    [Header("Player Health")]
    [Tooltip("Maximum health of player.")] public float playerHealthMaximum;
    [Tooltip("Player's current health.")] public float playerHealthCurrent;

    [Header("Player Attack Damge")]
    [Tooltip("Damage player can make when attack enemy.")] public float playerAttackDamage;

    private void Awake()
    {
        SetupStatus();
        SetupComponent();
    }
    private void SetupStatus()
    {
        playerHealthCurrent = playerHealthMaximum;
        playerCurrentState = "idle";
    }
    private void SetupComponent()
    {

    }

    public void PlayerDamageTaken(float damageTaken)
    {
        playerHealthCurrent -= damageTaken;
    }

    private void FixedUpdate()
    {
        if (playerHealthCurrent <= 0)
        {
            //player gameover
        }
    }
}
