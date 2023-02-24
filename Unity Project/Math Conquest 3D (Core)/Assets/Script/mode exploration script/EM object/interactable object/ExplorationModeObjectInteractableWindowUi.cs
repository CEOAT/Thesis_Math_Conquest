using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ExplorationModeObjectInteractableWindowUi : MonoBehaviour
{
    [SerializeField] public GameObject windowGroup;
    [SerializeField] public TMP_Text windowTextPuzzleProblem;
    [SerializeField] public TMP_Text windowTextDescription;
    [SerializeField] public TMP_Text windowTextPuzzleCompleteCount;
    [SerializeField] public TMP_InputField windowInputField;

    [SerializeField] public string windowAnswer;
    [SerializeField] public bool isWindowFetch;
    [SerializeField] public bool isWindowGetNewQuestion;

    [SerializeField] public int puzzleCompleteCount;
    [SerializeField] public int puzzleCompleteMaximum;

    [SerializeField] private MasterInput playerInput;
    [SerializeField] private ExplorationModeGameController GameController;
    [SerializeField] private ExplorationModeObjectInteractable ObjectInteractable;

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
        GameController = GetComponent<ExplorationModeObjectInteractable>().GameController;
        ObjectInteractable = GetComponent<ExplorationModeObjectInteractable>();
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

    public void WindowActivation()
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
        windowInputField.text = "";
        GameController.TriggerCutscene();
        GameController.DisablePauseGame();
        playerInput.Enable();
        isWindowFetch = true;
        windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
    }
    private void CloseWindow()
    {
        windowGroup.SetActive(false);
        GameController.AllowMovement();
        GameController.EnablePauseGame();
        playerInput.Disable();
        this.enabled = false;
    }
    private void ConfirmAnswer()
    {
        if (windowInputField.isActiveAndEnabled == true)
        {
            if (windowInputField.text.ToString().ToUpper() == windowAnswer && isWindowGetNewQuestion == false)
            {
                if (puzzleCompleteCount + 1 == puzzleCompleteMaximum)
                {
                    puzzleCompleteCount++;
                    PuzzleComplete();
                    WindowActivation();
                }
                else
                {
                    isWindowGetNewQuestion = true;
                    puzzleCompleteCount++;
                }
            }
            windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
        }
        windowInputField.text = "";
    }

    private void PuzzleComplete()
    {
        ObjectInteractable.ActiveReaction();
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        windowInputField.ActivateInputField();
    }
}
