using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private ExplorationModeGameController GameController;
    [SerializeField] private bool isTriggerStartCutscene;

    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            ControlCutscene();
            this.gameObject.SetActive(false);
        }
    }
    private void ControlCutscene()
    {
        if(isTriggerStartCutscene == true)
        {
            GameController.TriggerCutscene();
        }
        else if(isTriggerStartCutscene == false)
        {
            GameController.AllowMovement();
        }
    }
}
