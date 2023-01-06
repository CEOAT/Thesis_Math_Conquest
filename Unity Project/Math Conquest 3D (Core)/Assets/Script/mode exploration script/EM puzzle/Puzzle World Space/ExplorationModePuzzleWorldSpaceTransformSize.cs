using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplorationModePuzzleWorldSpaceTransform : MonoBehaviour
{
    [SerializeField] private Transform objectWorldSpacePuzzle;

    private ExplorationModePuzzleWorldSpace PuzzleWorldSpace;

    private void Awake()
    {
        SetupComponent();
        SetupSubscription();
    }
    private void SetupComponent()
    {
        PuzzleWorldSpace = GetComponent<ExplorationModePuzzleWorldSpace>();
    }
    private void SetupSubscription()
    {
        PuzzleWorldSpace.ConfirmValueEvent.AddListener(ApplyValueToObject);
    }

    public virtual void ApplyValueToObject()
    {
        // add method here
        /*objectWorldSpacePuzzle.localScale = new Vector3(PuzzleWorldSpace.puzzleVariableList[0],
                                                            PuzzleWorldSpace.puzzleVariableList[1],
                                                            PuzzleWorldSpace.puzzleVariableList[2]);*/
    }

    private void OnDisable()
    {
        PuzzleWorldSpace.ConfirmValueEvent.RemoveListener(ApplyValueToObject);
    }
}
