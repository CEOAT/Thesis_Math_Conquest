using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCompassDisable : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private DotCompassManager ThisPuzzleManager;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Canvas.SetActive(true);
        ThisPuzzleManager.enabled = true;
        ThisPuzzleManager.EnableThisPuzzle();
    }

    private void OnDisable()
    {
        Canvas.SetActive(false);
        ThisPuzzleManager.enabled = false;
        ThisPuzzleManager.DisableThisPuzzle();
    }
}
