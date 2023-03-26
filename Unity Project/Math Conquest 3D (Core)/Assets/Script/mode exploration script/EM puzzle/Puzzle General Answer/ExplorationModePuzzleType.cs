using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplorationModePuzzleType : MonoBehaviour
{

    [SerializeField] private List<ExplorationModePuzzleTypeSet> puzzleSet = new List<ExplorationModePuzzleTypeSet>();
    private List<int> puzzleSetUsedIndex = new List<int>();
    private bool puzzleRandomFirstTime;
    private int puzzleIndexUsed;
    [SerializeField] private int puzzleIndex;

    private ExplorationModeObjectInteractableWindowUi PuzzleWindow;
    
    private void Awake()
    {
        SetupComponent();
        SetupPuzzleWindowSetting();
    }
    private void SetupComponent()
    {
        PuzzleWindow = GetComponent<ExplorationModeObjectInteractableWindowUi>();
    }
    private void SetupPuzzleWindowSetting()
    {
        PuzzleWindow.uiUpdateEvent.AddListener(SetupWindow);
        PuzzleWindow.uiUpdateQuestionEvent.AddListener(RandomAndSetupPuzzle);
    }

    private void Start()
    {
        RandomAndSetupPuzzle();
    }
    private void RandomAndSetupPuzzle()
    {
        PuzzleWindow.isWindowGetNewQuestion = false;
        RandomPuzzle();
        SetupWindow();
    }
    private void RandomPuzzle()
    {
        if(puzzleRandomFirstTime)
        {
            puzzleIndex = Random.Range(0, puzzleSet.Count);
            puzzleRandomFirstTime = false;
        }
        else if(!puzzleRandomFirstTime && puzzleSetUsedIndex.Count < puzzleSet.Count)
        {
            do
            {
                puzzleIndex = Random.Range(0, puzzleSet.Count);
                foreach(int indexUsed in puzzleSetUsedIndex)
                {
                    if(puzzleIndex == indexUsed)
                    {
                        puzzleIndexUsed = indexUsed;
                        break;
                    }
                }
            }while(puzzleIndex == puzzleIndexUsed);
            puzzleSetUsedIndex.Add(puzzleIndex);
        }
        else
        {
            puzzleIndex = 0;
        }
    }
    private void SetupWindow()
    {
        PuzzleWindow.SetupWindow(puzzleSet[puzzleIndex].puzzleProblem,  
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