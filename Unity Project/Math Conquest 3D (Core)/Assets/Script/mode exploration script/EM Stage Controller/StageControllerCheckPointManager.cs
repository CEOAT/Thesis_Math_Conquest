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
    public string checkpointCurrentName = "None";
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

        int count = 0;
        foreach (Transform checkpoint in checkpointList)
        {
            if (checkpointSaveData.checkpointName == checkpoint.name)
            {
                checkpointCurrentPosition = checkpoint.transform;
                break;
            }
            count++;
        }
        return checkpointCurrentPosition;
    }
}
