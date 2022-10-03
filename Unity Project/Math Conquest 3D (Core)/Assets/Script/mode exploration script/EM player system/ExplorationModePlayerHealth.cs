using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExplorationModePlayerHealth : MonoBehaviour
{
    // control player's HP, regeneration, healing, and hurting

    public float playerHealthCurrent;
    public float playerHealthMaximum;

    private ExplorationModePlayerControllerMovement PlayerMovement;

    public delegate void PlayerDead();
    public static event PlayerDead playerDead;

    private void Awake()
    {
        SetupComponent();
        SetupValue();
    }
    private void SetupComponent()
    {
        PlayerMovement = GetComponent<ExplorationModePlayerControllerMovement>();
    }
    private void SetupValue()
    {
        playerHealthCurrent = playerHealthMaximum;
    }

    public void PlayerTakenDamage(float damageTaken)   // called from enemy
    {
        playerHealthCurrent -= damageTaken;
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

    public void PlayerGameOver()
    {
        playerDead();
        playerHealthCurrent = 0;
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
