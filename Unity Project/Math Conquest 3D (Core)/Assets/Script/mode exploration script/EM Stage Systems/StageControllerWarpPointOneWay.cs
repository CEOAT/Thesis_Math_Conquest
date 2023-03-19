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
    public GameObject transitionImagePrefab;

    [Header("Warp Point Setting")]
    private Transform playerTransform;
    public List<Transform> warpPointExitList;
    [SerializeField] private int warpPointExitIndex;
    public float transitionWaitTimeFirst = 1.5f;
    public float transitionWaitTimeSecond = 1.5f;
    public bool isWarpPointDestroyAfterTrigger;

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
        Transform transitionImageObject = Instantiate(transitionImagePrefab.transform);
        transitionImageObject.SetParent(canvasTransform);
        transitionImageObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameController.TriggerCutscene();

        yield return new WaitForSeconds(transitionWaitTimeFirst);
        MovePlayerToExitPoint();

        yield return new WaitForSeconds(transitionWaitTimeSecond);
        GameController.AllowMovement();

        if (isWarpPointDestroyAfterTrigger == true)
        {
            DestroyAfterTrigger();
        }
    }
}
