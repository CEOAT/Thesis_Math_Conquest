using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private StageControllerCheckPointManager CheckPointManager;
    [SerializeField] private string objectiveText;

    [SerializeField] private bool isDestroyAfterLeaveTrigger = false;
    
    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player" && isDestroyAfterLeaveTrigger == false)
        {
            CheckPointManager.SendObjective(objectiveText);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if(player.tag == "Player" && isDestroyAfterLeaveTrigger == true)
        {
            CheckPointManager.SendObjective(objectiveText);
            Destroy(this.gameObject);
        }
    }
}