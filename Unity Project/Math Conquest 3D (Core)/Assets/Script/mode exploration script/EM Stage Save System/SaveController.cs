using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{   
    // called from checkpoint trigger
    public void SaveGameCheckpoint(StageSaveData checkpointSaveData)
    {
        PlayerPrefs.SetString("StageName", checkpointSaveData.stageName);
        PlayerPrefs.SetString("CheckpointName", checkpointSaveData.checkpointName);
        PlayerPrefs.SetInt("CheckpointIndex", checkpointSaveData.checkpointIndex);
    }

    // called from end stage trigger
    public void SaveGameEndStage()
    {
        string stageName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("StageName", "NoStage");
        PlayerPrefs.SetString("CheckpointName","Checkpoint-1");
        PlayerPrefs.SetInt("CheckpointIndex", 0);
        SaveCompleteStage(stageName);
    }
    private void SaveCompleteStage(string stageName)
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        string saveFilePath = Application.dataPath + $"/Streaming Assets/Save File/MQ save file.Json";
        if(System.IO.File.Exists(saveFilePath))
        {
            string loadJson = File.ReadAllText(saveFilePath);
            checkpointSaveData = JsonUtility.FromJson<StageSaveData>(loadJson);
        }
        
        switch(stageName)
        {
            case "stage_test":{
                checkpointSaveData.isStageClearTest = true; break;}
            case "stage_prologue":{
                checkpointSaveData.isStageClear1 = true; break;}
            case "stage_transformation":{
                checkpointSaveData.isStageClear2 = true; break;}
            case "stage_logic":{
                checkpointSaveData.isStageClear3 = true; break;}
            case "stage_encryption":{
                checkpointSaveData.isStageClear4 = true; break;}
            case "stage_vector":{
                checkpointSaveData.isStageClear5 = true; break;}
            case "stage_final":{
                checkpointSaveData.isStageClear6 = true; break;}
        }
        
        string saveJson = JsonUtility.ToJson(checkpointSaveData);
        File.WriteAllText(Application.dataPath + $"/Streaming Assets/Save File/MQ save file.Json", saveJson);
    }

    // call from main menu ---> delete save file
    public void DeleteSaveFile()
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        checkpointSaveData.isStageClearTest = false;
        checkpointSaveData.isStageClear1 = false;
        checkpointSaveData.isStageClear2 = false;
        checkpointSaveData.isStageClear3 = false;
        checkpointSaveData.isStageClear4 = false;
        checkpointSaveData.isStageClear5 = false;
        checkpointSaveData.isStageClear6 = false;

        string saveJson = JsonUtility.ToJson(checkpointSaveData);
        File.WriteAllText(Application.dataPath + $"/Streaming Assets/Save File/MQ save file.Json", saveJson);
        PlayerPrefs.DeleteAll();
    }

    public bool LoadStageClearUI(string stageName)
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        string loadJson = File.ReadAllText(Application.dataPath + $"/Streaming Assets/Save File/MQ save file.Json");
        bool isStageActive = false;
        checkpointSaveData = JsonUtility.FromJson<StageSaveData>(loadJson);

        switch(stageName)
        {
            case "stage_prologue":{
                isStageActive = checkpointSaveData.isStageClear1; break;}
            case "stage_transformation":{
                isStageActive = checkpointSaveData.isStageClear2; break;}
            case "stage_logic":{
                isStageActive = checkpointSaveData.isStageClear3; break;}
            case "stage_encryption":{
                isStageActive = checkpointSaveData.isStageClear4; break;}
            case "stage_vector":{
                isStageActive = checkpointSaveData.isStageClear5; break;}
            case "stage_final":{
                isStageActive = checkpointSaveData.isStageClear6; break;}
        }

        return isStageActive;
    }

    // called from main menu ---> continue game: return stage name
    public void LoadLastScenePlay()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("StageName", "NoStage"));
    }

    // called auto from starting scene
    // called manually from pause menu/ continue game: return checkpoint name, index
    public StageSaveData LoadGameCheckpoint()
    {
        StageSaveData checkpointSaveData = new StageSaveData();
        checkpointSaveData.checkpointName = PlayerPrefs.GetString("CheckpointName","Checkpoint-1");
        checkpointSaveData.checkpointIndex = PlayerPrefs.GetInt("CheckpointIndex", 0);

        return checkpointSaveData;
    }
    // called from pause menu ---> Restart Stage button
    public void RestartCheckpoint()
    {
        PlayerPrefs.SetString("CheckpointName","Checkpoint-1");
        PlayerPrefs.SetInt("CheckpointIndex", 0);
    }
}

public class StageSaveData
{
    public string stageName;
    public string checkpointName;
    public int checkpointIndex;

    public bool isStageClearTest;
    public bool isStageClear1;
    public bool isStageClear2;
    public bool isStageClear3;
    public bool isStageClear4;
    public bool isStageClear5;
    public bool isStageClear6;
}