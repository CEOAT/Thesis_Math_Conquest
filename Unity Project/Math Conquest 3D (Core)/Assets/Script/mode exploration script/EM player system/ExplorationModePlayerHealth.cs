using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ExplorationModePlayerHealth : MonoBehaviour
{
    // control player's HP, regeneration, healing, and hurting

    public float playerHealthCurrent;
    public float playerHealthMaximum;
    public float playerHealthPercentage;

    public float playerInvincibleTimeMax = 2f;
    public float playerInvincibleTimeCurrent = 0f;
    public bool isPlayerOnInvincible = false;
    public bool canPlayerTakeDamage = true;

    private ExplorationModePlayerControllerMovement PlayerMovement;

    public delegate void PlayerDead();
    public static event PlayerDead playerDead;

    public GameObject PlayerStatusUIGroup;
    private Image PlayerHealthBar;

    private void Awake()
    {
        SetupComponent();
        SetupValue();
        SetupObject();
    }
    private void SetupComponent()
    {
        PlayerMovement = GetComponent<ExplorationModePlayerControllerMovement>();
    }
    private void SetupValue()
    {
        playerHealthCurrent = playerHealthMaximum;
    }
    private void SetupObject()
    {
        PlayerHealthBar = PlayerStatusUIGroup.transform.GetChild(1).GetComponent<Image>();
    }

    public void PlayerTakenDamage(float damageTaken)   // called from enemy
    {
        if (canPlayerTakeDamage == true && isPlayerOnInvincible == false)
        {
            playerHealthCurrent -= damageTaken;
            canPlayerTakeDamage = true;
            isPlayerOnInvincible = true;

            PlayerHealthBarControl();
            if (playerHealthCurrent > 0)
            {
                PlayerMovement.PlayerHurt();
            }
            else if (playerHealthCurrent <= 0)
            {
                PlayerMovement.PlayerDead();
                PlayerGameOver();
            }
        }
    }   
    public void PlayerGameOver()
    {
        playerDead();
        canPlayerTakeDamage = false;
        playerHealthCurrent = 0;
    }

    private void PlayerHealthBarControl()
    {
        playerHealthPercentage = (playerHealthCurrent * 100f) / playerHealthMaximum;
        PlayerHealthBar.fillAmount = playerHealthPercentage / 100f;
    }

    private void FixedUpdate()
    {
        PlayerActiveInvincible();
    }
    private void PlayerActiveInvincible()
    {
        if ((playerInvincibleTimeCurrent < playerInvincibleTimeMax) && isPlayerOnInvincible == true)
        {
            playerInvincibleTimeCurrent += Time.deltaTime;
            canPlayerTakeDamage = false;
        }
        if (playerInvincibleTimeCurrent > playerInvincibleTimeMax)
        {
            playerInvincibleTimeCurrent = 0;
            canPlayerTakeDamage = true;
            isPlayerOnInvincible = false;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ExplorationModePlayerHealth))]
public class PlayerHealthTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExplorationModePlayerHealth health = (ExplorationModePlayerHealth)target;

        if (GUILayout.Button("Game Over"))
        {
            health.PlayerGameOver();
        }
    }
}
#endif
