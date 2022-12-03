using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerCheckPointManager : MonoBehaviour
{
    [Header("Game Controller and Save Controller")]
    public ExplorationModeGameController GameController;
    public SaveController SaveController;

    [Header("Checkpoint Manager Data")]
    public int checkpointCurrentIndex;
    private Transform checkpointCurrentPosition;
    public List<Transform> checkpointList;

    public void SendObjective(string objective)
    {
        GameController.SetObjective(objective);
    }
    public void SendCheckpointData(StageSaveData checkpointData)
    {
        SaveController.SaveGameCheckpoint(checkpointData);
    }

    public Transform MoveToCheckpoint()
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        checkpointSaveData = SaveController.LoadGameCheckpoint();

        for (int i = 0; i < checkpointList.Count; i++)
        {
            if (checkpointSaveData.checkpointIndex == i)
            {
                checkpointCurrentPosition = checkpointList[i].transform;
                break;
            }
        }
        return checkpointCurrentPosition;
    }
}
