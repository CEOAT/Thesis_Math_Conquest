
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationModeEventController : MonoBehaviour
{
    public string StageStatus;
    public List<EventTypeClass> EventType;

    private GameObject cutsceneControllerObject;
    private CutsceneControllerDialog CutsceneController;

    private GameObject enemySpawnerObject;
    private EliminationModeEnemySpawnerController EnemySpawner;

    private void Awake()
    {
        SetupComponent();
        DisableCompoentOnAwake();
    }
    private void SetupComponent()
    {
        if (GameObject.FindGameObjectWithTag("Cutscene System") != null)
        {
            enemySpawnerObject = GameObject.FindGameObjectWithTag("Cutscene System");
            CutsceneController = enemySpawnerObject.GetComponent<CutsceneControllerDialog>();
        }
        if (GameObject.FindGameObjectWithTag("Enemy System") != null)
        {
            cutsceneControllerObject = GameObject.FindGameObjectWithTag("Enemy System");
            EnemySpawner = cutsceneControllerObject.GetComponent<EliminationModeEnemySpawnerController>();
        }
    }
    private void DisableCompoentOnAwake()
    {
        
    }

    private void Start()
    {
        
    }

    private void CutscenePhrase()
    {
        StageStatus = "cutscene";
        
    }
    private void MovePhrase()
    {
        StageStatus = "moving";
    }
    private void BattlePhrase()
    {
        StageStatus = "battle";
    }
    private void EvaluatePhrase()
    {
        StageStatus = "evaluate";
    }
}

[System.Serializable]
public class EventTypeClass
{
    public enum EventType
    {
        cutscene,
        battle
    };
}