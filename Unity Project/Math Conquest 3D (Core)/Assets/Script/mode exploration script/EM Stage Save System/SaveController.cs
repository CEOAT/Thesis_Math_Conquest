using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public void SaveGameCheckpoint(StageSaveData checkpointSaveData)
    {
        PlayerPrefs.SetString("CheckpointName", checkpointSaveData.checkpointName);
        PlayerPrefs.SetInt("CheckpointIndex", checkpointSaveData.checkpointIndex);
    }
    public StageSaveData LoadGameCheckpoint()
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        checkpointSaveData.checkpointName = PlayerPrefs.GetString("CheckpointName","Checkpoint-1");
        checkpointSaveData.checkpointIndex = PlayerPrefs.GetInt("CheckpointIndex", 0);

        return checkpointSaveData;
    }
    public void RestartCheckpoint()
    {
        PlayerPrefs.DeleteAll();
    }
}

public class StageSaveData
{
    public string checkpointName;
    public int checkpointIndex;
}