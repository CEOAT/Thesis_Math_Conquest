using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ExplorationModeObjectInteractableWindowUi : MonoBehaviour
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
    private ExplorationModeGameController GameController;

    private void OnEnable()
    {
        ExplorationModeObjectInteractable.puzzleInteract += WindowActivation;
    }
    private void OnDisable()
    {
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
        GameController = GetComponent<ExplorationModeObjectInteractable>().GameController;
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
        windowInputField.text = "";
        GameController.TriggerCutscene();
        GameController.DisablePauseGame();
        playerInput.Enable();
    }
    private void CloseWindow()
    {
        windowGroup.SetActive(false);
        GameController.AllowMovement();
        GameController.EnablePauseGame();
        playerInput.Disable();
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
                    WindowActivation();
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
        CloseWindow();
        puzzleReaction();
    }

    private void FixedUpdate()
    {
        windowInputField.ActivateInputField();
    }
}
