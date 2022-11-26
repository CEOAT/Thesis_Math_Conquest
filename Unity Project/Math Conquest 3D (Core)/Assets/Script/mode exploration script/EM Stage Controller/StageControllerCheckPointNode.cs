using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerCheckPointNode : MonoBehaviour
{
    [Header("Checkpoint Manager Object")]
    public StageControllerCheckPointManager CheckPointManager;

    [Header("Checkpoint Property")]
    public string checkpointName;
    public string checkpointObjective;

    private void Start()
    {
        checkpointName = this.transform.name;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            StageSaveData checkpointSaveData = new StageSaveData();
            checkpointSaveData.checkpointName = checkpointName;

            int count = 0;
            foreach (Transform checkpoint in CheckPointManager.checkpointList)
            {
                if (checkpointName == checkpoint.name)
                {
                    checkpointSaveData.checkpointIndex = count;
                    break;
                }
                count++;
            }

            CheckPointManager.SendObjective(checkpointObjective);
            CheckPointManager.SendCheckpointData(checkpointSaveData);
        }
    }
}