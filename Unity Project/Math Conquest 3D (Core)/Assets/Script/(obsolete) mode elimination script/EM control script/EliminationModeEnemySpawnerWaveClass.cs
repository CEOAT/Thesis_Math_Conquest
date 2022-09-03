using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EliminationModeEnemySpawnerWaveClass
{
    public List<EliminationModeEnemySpawnerWaveEnemy> waveOfEnemy;
}

[System.Serializable]
public class EliminationModeEnemySpawnerWaveEnemy
{
    public List<EliminationModeEnemySpawnerWaveDetail> enemyInWave;
}

[System.Serializable]
public class EliminationModeEnemySpawnerWaveDetail
{
    public GameObject enemyPrefab;
    public SpawnType enemySpawnType;
    public enum SpawnType
    {
        moveIn,
        wait,
        fillIn,
        popUp
    };
    public float moveSpeed;
    public float waitTime;
}