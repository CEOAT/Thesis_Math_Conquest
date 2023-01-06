using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleType : MonoBehaviour
{

    [SerializeField] private List<ExplorationModePuzzleTypeSet> puzzleSet = new List<ExplorationModePuzzleTypeSet>();
    [SerializeField] private int puzzleIndex;

    private ExplorationModeObjectInteractableWindowUi PuzzleWindow;

    private void Awake()
    {
        SetupComponent();
    }
    private void SetupComponent()
    {
        PuzzleWindow = GetComponent<ExplorationModeObjectInteractableWindowUi>();
    }

    private void Start()
    {
        RandomPuzzle();
        SetupWindow();
    }
    private void RandomPuzzle()
    {
        puzzleIndex = Random.Range(0, puzzleSet.Count);
    }
    private void SetupWindow()
    {
        PuzzleWindow.SetupWindow( puzzleSet[puzzleIndex].puzzleProblem,  
            puzzleSet[puzzleIndex].puzzleDescription, 
            puzzleSet[puzzleIndex].puzzleAnswer);
    }
}

[System.Serializable]
public class ExplorationModePuzzleTypeSet
{
    [SerializeField] public string puzzleProblem;
    [SerializeField] public string puzzleDescription;
    [SerializeField] public string puzzleAnswer;
}