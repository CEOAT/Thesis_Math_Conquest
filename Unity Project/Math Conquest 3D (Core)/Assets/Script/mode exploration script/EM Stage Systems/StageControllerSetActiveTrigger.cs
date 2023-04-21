using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerSetActiveTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> activeObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> reverseActiveObjectList = new List<GameObject>();
    [SerializeField] private bool isSetActiveFalseAfterTrigger = true;
    [SerializeField] private bool isDestroyAfterTrigger = false;
    [SerializeField] private bool isActiveWhenLeaveTrigger = false;
    [Sirenix.OdinInspector.ReadOnly] private bool isActivationDone;
    
    private void OnTriggerEnter(Collider player) 
    {
        if(player.tag == "Player")
        {
            ActiveTrigger();
        }
    }
    private void ActiveTrigger()
    {
        DeactiveObject();
        ActiveObject();
        ReverseActiveObject();
        CheckDeactivationType();
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
    private void ReverseActiveObject()
    {
        if(reverseActiveObjectList.Count > 0)
        {
            foreach(GameObject reverseActiveObject in reverseActiveObjectList)
            {
                if(reverseActiveObject != null)
                    reverseActiveObject.SetActive(!reverseActiveObject.activeInHierarchy);
            }
        }
    }
    private void CheckDeactivationType()
    {
        if(isSetActiveFalseAfterTrigger == true)
        {
            this.gameObject.SetActive(false);
        }
        else if(isDestroyAfterTrigger == true)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if(isActiveWhenLeaveTrigger == true && player.tag == "Player")
        {
            ActiveTrigger();
        }
    }
}
