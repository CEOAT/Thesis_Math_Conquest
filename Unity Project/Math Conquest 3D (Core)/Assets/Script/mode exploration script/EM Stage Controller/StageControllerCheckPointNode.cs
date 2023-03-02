using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerCheckPointNode : MonoBehaviour
{
    [Header("Checkpoint Manager Object")]
    public StageControllerCheckPointManager CheckPointManager;

    [Header("Checkpoint Property")]
    [SerializeField] private string checkpointName;
    [SerializeField] private string checkpointObjective;
    private StageSaveData checkpointSaveData;

    private void Start()
    {
        checkpointName = this.transform.name;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            GetCheckpointName();
            FindIndexOfCheckpoint();
            SendValueToManager();
            CreateEnterCheckpointEffect();
            RestorePlayerHealth();
            this.gameObject.SetActive(false);
        }
    }
    private void GetCheckpointName()
    {
        checkpointSaveData = new StageSaveData();
        checkpointSaveData.checkpointName = checkpointName;
    }
    private void FindIndexOfCheckpoint()
    {
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
    }
    private void SendValueToManager()
    {
        CheckPointManager.SendObjective(checkpointObjective);
        CheckPointManager.SendCheckpointData(checkpointSaveData);
    }
    private void RestorePlayerHealth()
    {
        CheckPointManager.GameController.PlayerHealth.PlayerResetHealth();
    }
    private void CreateEnterCheckpointEffect()
    {
        if(CheckPointManager.isReadyToCreateEnterEffect == true)
        {
            GameObject checkpointEnterParticleEffectObject 
            = Instantiate(CheckPointManager.checkpointEnterParticleEffect, transform.position, transform.rotation);

            Destroy(checkpointEnterParticleEffectObject, 5f);
        }
    }
}