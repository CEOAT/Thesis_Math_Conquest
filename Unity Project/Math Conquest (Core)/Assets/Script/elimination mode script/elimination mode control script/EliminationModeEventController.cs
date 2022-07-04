
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationModeEventController : MonoBehaviour
{
    public string StageStatus;
    public List<EventTypeClass> EventType;

    private EliminationModeEnemySpawnerController EnemySpawner;

    private void Awake()
    {
        EnemySpawner = GetComponent<EliminationModeEnemySpawnerController>();
    }

    private void Start()
    {
        CutscenePhrase();
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