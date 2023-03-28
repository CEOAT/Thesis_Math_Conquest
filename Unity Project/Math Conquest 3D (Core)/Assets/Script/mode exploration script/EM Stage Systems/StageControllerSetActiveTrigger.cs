using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerSetActiveTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> activeObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObjectList = new List<GameObject>();
    [Sirenix.OdinInspector.ReadOnly] private bool isActivationDone;
    
    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player" && isActivationDone == false)
        {
            ActiveObject();
            DeactiveObject();
            isActivationDone = true;
        }
    }
    private void ActiveObject()
    {
        if(activeObjectList.Count > 0)
        {
            foreach(GameObject activeObject in activeObjectList)
            {
                if(activeObject != null)
                    activeObject.SetActive(true);
            }
        }
    }
    private void DeactiveObject()
    {
        if(deactiveObjectList.Count > 0)
        {
            foreach(GameObject deactiveObject in deactiveObjectList)
            {
                if(deactiveObject != null)
                    deactiveObject.SetActive(false);
            }
        }
    }
}
