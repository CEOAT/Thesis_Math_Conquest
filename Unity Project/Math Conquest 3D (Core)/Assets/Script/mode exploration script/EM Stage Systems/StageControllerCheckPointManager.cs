using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerCheckPointManager : MonoBehaviour
{
    [Header("Game Controller and Save Controller")]
    [HideInInspector] public ExplorationModeGameController GameController;
    [SerializeField] private SaveController SaveController;

    [Header("Checkpoint Manager Data")]
    public int checkpointCurrentIndex;
    private Transform checkpointCurrentPosition;
    [HideInInspector] public bool isReadyToCreateEnterEffect = false;
    [SerializeField] public GameObject checkpointEnterParticleEffect;
    public List<Transform> checkpointList;

    private void Start()
    {
        SetupComponent();
        Invoke("ReadyCountToCreateEffect", 1f);
    }
    private void SetupComponent()
    {
        GameController = GetComponent<ExplorationModeGameController>();
    }
    private void ReadyCountToCreateEffect()
    {
        isReadyToCreateEnterEffect = true;
    }

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