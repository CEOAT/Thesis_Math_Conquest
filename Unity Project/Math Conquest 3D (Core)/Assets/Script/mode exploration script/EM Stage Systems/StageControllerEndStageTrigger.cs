using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageControllerEndStageTrigger : MonoBehaviour
{
    [SerializeField] private string stageName;
    [SerializeField] private GameObject endStageCanvasPrefab;
    private Transform endStageCanvasObject;
    [SerializeField] private GameObject endStageWindowPrefab;
    private Transform endStageWindowObject;

    [SerializeField] private ExplorationModeGameController GameController;
    private StageControllerEndStageScore ScoreController;
    private SaveController SaveData;
    private GameObject playerObject;

    private void Start()
    {
        SetupConponent();
    }
    private void SetupConponent()
    {
        ScoreController = GetComponent<StageControllerEndStageScore>();
        SaveData = GetComponent<SaveController>();
    }

    private void OnTriggerEnter(Collider player) 
    {
        if(player.CompareTag("Player"))
        {
            playerObject = player.gameObject;
            TriggerEndStage();
        }
    }
    private void TriggerEndStage()
    {
        GameController.TriggerCutscene();
        ScoreController.StopTimeCount();
        StartCoroutine(EndStageSequence());
    }
    private IEnumerator EndStageSequence()
    {
        yield return new WaitForSeconds(1.5f);
        endStageCanvasObject = Instantiate(endStageCanvasPrefab).transform;
        
        yield return new WaitForSeconds(3f);
        endStageWindowObject = Instantiate(endStageWindowPrefab, endStageCanvasObject.transform).transform;
        Destroy(GameController.playerGameObject.GetComponent<Rigidbody>());
        GameController.playerGameObject.position += new Vector3(0, 100, 0);
        AssignStringToText();
        SaveData.SaveGameEndStage();
    }
    private void AssignStringToText()
    {
        endStageWindowObject.GetChild(1).GetComponent<TMP_Text>().text = stageName;
        endStageWindowObject.GetChild(2).GetComponent<TMP_Text>().text = ScoreController.CheckTime();
        endStageWindowObject.GetChild(3).GetComponent<TMP_Text>().text = ScoreController.CheckGrade().ToString();
        endStageWindowObject.GetChild(4).GetComponent<Button>().onClick.AddListener(GameController.MenuReturnToStageSelection);
        endStageWindowObject.GetChild(5).GetComponent<Button>().onClick.AddListener(GameController.MenuRestartStage);
        endStageWindowObject.GetChild(6).GetComponent<Button>().onClick.AddListener(GameController.MenuQuitGame);
    }
}