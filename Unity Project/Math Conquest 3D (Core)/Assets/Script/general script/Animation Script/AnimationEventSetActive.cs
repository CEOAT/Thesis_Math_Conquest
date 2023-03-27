using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSetActive : MonoBehaviour
{
    [SerializeField] List<GameObject> activeObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObjectList = new List<GameObject>();
    [Sirenix.OdinInspector.ReadOnly] private bool isActivationDone;
    
    public void SetActiveOnAnimation()
    {
        ActiveObject();
        DeactiveObject();
    }
    private void ActiveObject()
    {
        if(activeObjectList.Count > 0)
        {
            foreach(GameObject activeObject in activeObjectList)
            {
                activeObject.SetActive(true);
            }
        }
    }
    private void DeactiveObject()
    {
        if(deactiveObjectList.Count > 0)
        {
            foreach(GameObject activeObject in deactiveObjectList)
            {
                activeObject.SetActive(false);
            }
        }
    }
}
