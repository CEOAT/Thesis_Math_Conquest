using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDotDisable : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;
    [SerializeField] private DotNormManager ThisPuzzleManager;

    private void OnEnable()
    {
        Canvas.SetActive(true);
        ThisPuzzleManager.EnableThisPuzzle();
    }

    private void OnDisable()
    {
        Canvas.SetActive(false);
        ThisPuzzleManager.DisableThisPuzzle();
    }
}
