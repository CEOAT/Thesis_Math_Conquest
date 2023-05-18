using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerStageLoader : MonoBehaviour
{
    [SerializeField] private ExplorationModeGameController GameController;
    [SerializeField] private string stageName;

    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            GameController.TriggerCutscene();
            StartCoroutine(GameController.LoadSceneSequence(stageName));
        }
    }
}