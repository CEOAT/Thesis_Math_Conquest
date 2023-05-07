using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionDisable : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private LineOfsightManager ThisPuzzleManager;

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
