using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationModeEnemySpawnerController : MonoBehaviour
{
    [Header("Testing Variable")]
    public bool testSpawnOnStart;

    [Header("Count wave/enemy")]
    [SerializeField] private int enemyWaveCount;
    [SerializeField] private int enemyDeadCount;

    private GameObject enemyObject;
    private int enemySpawnCountInWave;
    
    private List<int> enemyRandomPositionList = new List<int>();
    private int enemyRandomPositionIndex;

    [Header("List of Enemy")]
    public EliminationModeEnemySpawnerWaveClass EnemyWave = new EliminationModeEnemySpawnerWaveClass();
    private EliminationModeEnemyControllerAction EnemyAction;
    private Transform playerTransform;


    private void Start()
    {
        SetupComponent();

        //enabled for testing
        if (testSpawnOnStart == true)
        {
            CreateEnemyWave();
        }
    }
    private void SetupComponent()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player Object").transform;
    }


    //---------------------------------- Spawning Methods For Event Controller ----------------------------------//
    public void CreateEnemyWave()
    {
        for (int i = 0; i < EnemyWave.waveOfEnemy[enemyWaveCount].enemyInWave.Count; i++)
        {
            enemySpawnCountInWave = i;

            CreateEnemy();
            SetEnemySpawningType();
            SetEnemyToPosition();
        }
    }
    public void ClearEnemyWave()
    {
        enemyWaveCount++;
        enemySpawnCountInWave = 0;
        enemyRandomPositionIndex = 0;
        enemyRandomPositionList.Clear();
    }


    //---------------------------------- Create Enemy Methods ----------------------------------//
    private void CreateEnemy()
    {
        enemyObject = Instantiate(EnemyWave.waveOfEnemy[enemyWaveCount].enemyInWave[enemySpawnCountInWave].enemyPrefab,
            playerTransform.position + Vector3.right * 15f,
            playerTransform.rotation);

        EnemyAction = enemyObject.GetComponent<EliminationModeEnemyControllerAction>();
    }
    private void SetEnemySpawningType()
    {
        switch (EnemyWave.waveOfEnemy[enemyWaveCount].enemyInWave[enemySpawnCountInWave].enemySpawnType)
        {
            case EliminationModeEnemySpawnerWaveDetail.SpawnType.moveIn:
                {
                    EnemyAction.enemySpawnType = "move in";
                    EnemyAction.enemyMoveSpeed = EnemyWave.waveOfEnemy[enemyWaveCount].enemyInWave[enemySpawnCountInWave].moveSpeed;
                    break;
                }
            case EliminationModeEnemySpawnerWaveDetail.SpawnType.wait:
                {
                    EnemyAction.enemySpawnType = "wait";
                    EnemyAction.enemyMoveSpeed = 0f;
                    break;
                }
            case EliminationModeEnemySpawnerWaveDetail.SpawnType.fillIn:
                {
                    break;
                }
            case EliminationModeEnemySpawnerWaveDetail.SpawnType.popUp:
                {
                    break;
                }
        }
    }
    private void SetEnemyToPosition()
    {
        if (enemySpawnCountInWave == 0)
        {
            enemyRandomPositionIndex = 0;
            enemyRandomPositionList.Add(0);
        }
        else
        {
            if (enemySpawnCountInWave == 1)
            {
                enemyRandomPositionIndex = Random.Range(1, 4);
                enemyRandomPositionList.Add(enemyRandomPositionIndex);
            }
            else if (enemySpawnCountInWave >= 2)
            {
                bool isApprovedIndex = false;

                do
                {
                    enemyRandomPositionIndex = Random.Range(1, 4);
                    for (int i = 0; i < enemyRandomPositionList.Count; i++)
                    {
                        if (enemyRandomPositionIndex == enemyRandomPositionList[i])
                        {
                            isApprovedIndex = false;
                            break;
                        }
                        if (i + 1 == enemyRandomPositionList.Count)
                        {
                            isApprovedIndex = true;
                        }
                    }
                } while (isApprovedIndex == false);

                enemyRandomPositionList.Add(enemyRandomPositionIndex);
            }
        }

        switch (enemyRandomPositionIndex)
        {
            case 0:
                {
                    EnemyAction.enemyDestinationSpawnPoint = new Vector2(playerTransform.position.x + 30f + 3.2f, -0.7f);
                    EnemyAction.enemyDestinationDistancePlayer = 9.2f;
                    break;               
                }
            case 1:
                {
                    EnemyAction.enemyDestinationSpawnPoint = new Vector2(playerTransform.position.x + 30f + 6.4f, -0.7f);
                    EnemyAction.enemyDestinationDistancePlayer = 12.4f;
                    break;
                }
            case 2:
                {
                    EnemyAction.enemyDestinationSpawnPoint = new Vector2(playerTransform.position.x + 30f + 2.6f, 2.6f);
                    EnemyAction.enemyDestinationDistancePlayer = 8.6f;
                    break;
                }
            case 3:
                {
                    EnemyAction.enemyDestinationSpawnPoint = new Vector2(playerTransform.position.x + 30f + 6f, 2.6f);
                    EnemyAction.enemyDestinationDistancePlayer = 12.0f;
                    break;
                }
        }
    }

    private void OnGUI()
    {
        
    }
}
