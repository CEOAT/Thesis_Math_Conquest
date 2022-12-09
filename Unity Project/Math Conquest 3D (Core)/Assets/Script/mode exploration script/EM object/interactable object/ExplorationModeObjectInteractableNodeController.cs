using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeObjectInteractableNodeController : MonoBehaviour
{
    [Header("Node Complete Setting")]
    public GameObject nodeControllerCamera;
    public float nodeCompleteActiveEntryTime;
    public float nodeCompleteActiveExitTime;
    public float nodeCompleteCameraBlendTime;

    [Header("Node Activation")]
    [SerializeField] private List<GameObject> nodeObjectList = new List<GameObject>();
    [SerializeField] private List<bool> nodeActivationList = new List<bool>();

    private ExplorationModeObjectInteractable ObjectInteractable;

    private void Start()
    {
        SetupComponent();
        SetupObject();
    }
    private void SetupComponent()
    {
        ObjectInteractable = GetComponent<ExplorationModeObjectInteractable>();
    }
    private void SetupObject()
    {
        nodeControllerCamera.SetActive(false);
    }

    public void ActiveNode()    // called from node script
    {
        nodeActivationList.Add(true);
        CheckAllNodeActivation();
    }

    private void CheckAllNodeActivation()   // need to set type as instance
    {
        if (nodeActivationList.Count == nodeObjectList.Count)
        {
            StartCoroutine(ActiveNodeCompleteCutscene());
        }
    }
    private IEnumerator ActiveNodeCompleteCutscene()
    {
        nodeControllerCamera.SetActive(true);
        ObjectInteractable.GameController.TriggerCutscene();
        ObjectInteractable.GameController.DisablePauseGame();

        yield return new WaitForSeconds(nodeCompleteActiveEntryTime);
        ObjectInteractable.Interacted();

        yield return new WaitForSeconds(nodeCompleteActiveExitTime);
        ObjectInteractable.GameController.EnablePauseGame();
        nodeControllerCamera.SetActive(false);
    }
}