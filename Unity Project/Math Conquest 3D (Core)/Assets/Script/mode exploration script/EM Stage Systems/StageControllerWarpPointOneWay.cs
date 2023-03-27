using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerWarpPointOneWay : MonoBehaviour
{
    [Header("Game Controller")]
    public ExplorationModeGameController GameController;

    [Header("Canvas")]
    public Transform canvasTransform;

    [Header("Transition Image")]
    [SerializeField] private GameObject transitionInImagePrefab;
    [SerializeField] private GameObject transitionOutImagePrefab;

    [Header("Warp Point Setting")]
    private Transform playerTransform;
    public List<Transform> warpPointExitList;
    private int warpPointExitIndex;
    [SerializeField] private float transitionWaitTime = 1f;
    [SerializeField] private float movePlayerWaitTime = 0.5f;
    [SerializeField] private float playerControlWaitTime = 0.5f;
    [SerializeField] private bool isWarpPointDestroyAfterTrigger;

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            warpPointExitIndex = 0;
            playerTransform = player.transform;

            if (warpPointExitList.Count == 0)
            {
                warpPointExitIndex = 0;
            }
            else if (warpPointExitList.Count > 0)
            {
                RandomExitPoint();
            }

            StartCoroutine(WarpTransition());
        }
    }
    private void RandomExitPoint()
    {
        warpPointExitIndex = Random.Range(0, warpPointExitList.Count);
    }
    private void MovePlayerToExitPoint()
    {
        playerTransform.position = warpPointExitList[warpPointExitIndex].position;
    }
    private void DestroyAfterTrigger()
    {
        foreach (Transform exitPoint in warpPointExitList)  
        {
            Destroy(exitPoint.gameObject);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator WarpTransition()
    {
        Transform transitionInImageObject = Instantiate(transitionInImagePrefab.transform);
        SetTransitionToCanvas(transitionInImageObject);
        GameController.TriggerCutscene();

        yield return new WaitForSeconds(transitionWaitTime);
        MovePlayerToExitPoint();

        yield return new WaitForSeconds(movePlayerWaitTime);
        Transform transitionOutImageObject = Instantiate(transitionOutImagePrefab.transform);
        SetTransitionToCanvas(transitionOutImageObject);
        Destroy(transitionInImageObject.gameObject);

        yield return new WaitForSeconds(playerControlWaitTime);
        GameController.AllowMovement();

        if (isWarpPointDestroyAfterTrigger == true)
        {
            DestroyAfterTrigger();
        }
    }
    private void SetTransitionToCanvas(Transform transitionObject)
    {
        transitionObject.SetParent(canvasTransform);
        transitionObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}
