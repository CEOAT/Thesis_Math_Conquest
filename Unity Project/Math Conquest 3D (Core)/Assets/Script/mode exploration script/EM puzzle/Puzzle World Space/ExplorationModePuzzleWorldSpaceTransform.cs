using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplorationModePuzzleWorldSpaceTransform : MonoBehaviour
{
    [SerializeField] public Transform objectWorldSpacePuzzle;
    [SerializeField] public List<float> vairableList = new List<float>();

    private ExplorationModePuzzleWorldSpaceWindow PuzzleWorldSpaceWindow;

    private void Awake()
    {
        SetupComponent();
        SetupVariable();
        SetupSubscription();
    }
    private void SetupComponent()
    {
        PuzzleWorldSpaceWindow = GetComponent<ExplorationModePuzzleWorldSpaceWindow>();
    }
    private void SetupVariable()
    {
        vairableList = PuzzleWorldSpaceWindow.puzzleVariableList;
    }
    private void SetupSubscription()
    {
        PuzzleWorldSpaceWindow.ConfirmValueEvent.AddListener(ApplyValueToObject);
    }

    public virtual void ApplyValueToObject()
    {
        // add method here
    }

    private void OnDisable()
    {
        PuzzleWorldSpaceWindow.ConfirmValueEvent.RemoveListener(ApplyValueToObject);
    }
}
