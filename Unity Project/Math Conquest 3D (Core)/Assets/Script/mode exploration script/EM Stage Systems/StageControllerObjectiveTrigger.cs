using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private StageControllerCheckPointManager CheckPointManager;
    [SerializeField] private string objectiveText;
    
    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            CheckPointManager.SendObjective(objectiveText);
            Destroy(this.gameObject);
        }
    }
}