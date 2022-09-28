using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplorationModeObjectInteractableWindow : MonoBehaviour
{
    public GameObject windowGroup;
    public TMP_Text windowTextPuzzleProblem;
    public TMP_Text windowTextDescription;
    public TMP_Text windowTextPuzzleCompleteCount;
    public TMP_InputField windowInputField;

    public string windowAnswer;
    public bool isWindowReset;

    public int puzzleCompleteCount;
    public int puzzleCompleteMaximum;

    public delegate void PuzzleReaction();
    public static event PuzzleReaction puzzleReaction;

    private MasterInput playerInput;

    private void OnEnable()
    {
        playerInput.Enable();
        ExplorationModeObjectInteractable.puzzleInteract += WindowActivation;
    }
    private void OnDisable()
    {
        playerInput.Disable();
        ExplorationModeObjectInteractable.puzzleInteract -= WindowActivation;
    }

    private void Awake()
    {
        SetupComponent();
        SetupControl();
        SetupObject();
    }
    private void SetupComponent()
    {
        playerInput = new MasterInput();
        windowInputField.GetComponent<TMP_InputField>();
    }
    private void SetupControl()
    {
        playerInput.WindowControl.CloseWindow.performed += context => CloseWindow();
        playerInput.WindowControl.ConfirmAnswer.performed += context => ConfirmAnswer();
    }
    private void SetupObject()
    {
        windowGroup.SetActive(false);
        windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
    }

    public void SetupWindow(string textPuzzle, string textDescription, string puzzleAnswer)
    {
        windowTextPuzzleProblem.text = textPuzzle;
        windowTextDescription.text = textDescription;
        windowAnswer = puzzleAnswer;
    }

    private void WindowActivation()
    {
        if (windowGroup.activeInHierarchy == false)
        {
            OpenWindow();
        }
        else if (windowGroup.activeInHierarchy == true)
        {
            CloseWindow();
        }
    }
    private void OpenWindow()
    {
        windowGroup.SetActive(true);
    }
    private void CloseWindow()
    {
        windowGroup.SetActive(false);
    }
    private void ConfirmAnswer()
    {
        if (windowInputField.isActiveAndEnabled == true)
        {
            if (windowInputField.text.ToString().ToUpper() == windowAnswer && isWindowReset == false)
            {
                if (puzzleCompleteCount + 1 == puzzleCompleteMaximum)
                {
                    puzzleCompleteCount++;
                    PuzzleComplete();
                    WindowActivation();   //will be replaced with close window animation
                }
                else
                {
                    isWindowReset = true;
                    puzzleCompleteCount++;
                }
            }
            windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
        }
        windowInputField.text = "";
    }

    private void PuzzleComplete()
    {
        print("puzzle complete");
        puzzleReaction();
    }

    private void FixedUpdate()
    {
        windowInputField.ActivateInputField();
    }
}
