using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ExplorationModeObjectInteractableWindowUi : MonoBehaviour
{
    [SerializeField, ReadOnly] public string windowAnswer;
    [SerializeField, ReadOnly] public bool isWindowFetch;
    [SerializeField, ReadOnly] public bool isWindowGetNewQuestion;

    [SerializeField, ReadOnly] public int puzzleCompleteCount;
    [SerializeField] public int puzzleCompleteMaximum;

    [HideInInspector] public UnityEvent uiUpdateEvent;
    [HideInInspector] public UnityEvent uiUpdateQuestionEvent;

    [SerializeField] private MasterInput playerInput;
    [SerializeField] private ExplorationModeGameController GameController;
    private ExplorationModeObjectInteractable ObjectInteractable;
    private InteractableWindowUiManager InteractUi;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
        SetupObject();
    }
    private void SetupComponent()
    {
        playerInput = new MasterInput();
        InteractUi = GameController.gameObject.GetComponent<InteractableWindowUiManager>();
        InteractUi.windowInputField.GetComponent<TMP_InputField>();
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
        InteractUi.windowGroup.SetActive(false);
        InteractUi.windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
    }

    public void SetupWindow(string textPuzzle, string textDescription, string puzzleAnswer)
    {
        InteractUi.windowTextPuzzleProblem.text = textPuzzle;
        InteractUi.windowTextDescription.text = textDescription;
        windowAnswer = puzzleAnswer;
    }

    public void WindowActivation()
    {
        if (InteractUi.windowGroup.activeInHierarchy == false)
        {
            OpenWindow();
        }
        else if (InteractUi.windowGroup.activeInHierarchy == true)
        {
            CloseWindow();
        }
    }
    private void OpenWindow()
    {
        InteractUi.windowGroup.SetActive(true);
        InteractUi.windowInputField.text = "";
        GameController.TriggerCutscene();
        GameController.DisablePauseGame();
        playerInput.Enable();
        uiUpdateEvent.Invoke();
        isWindowFetch = true;
        InteractUi.windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
    }
    private void CloseWindow()
    {
        InteractUi.windowGroup.SetActive(false);
        GameController.AllowMovement();
        GameController.EnablePauseGame();
        playerInput.Disable();
        this.enabled = false;
    }
    private void ConfirmAnswer()
    {
        if (InteractUi.windowInputField.isActiveAndEnabled == true)
        {
            if (InteractUi.windowInputField.text.ToString().ToUpper() == windowAnswer && isWindowGetNewQuestion == false)
            {
                if (puzzleCompleteCount + 1 == puzzleCompleteMaximum)
                {
                    print("puzzle complete");
                    puzzleCompleteCount++;
                    PuzzleComplete();
                    WindowActivation();
                }
                else
                {
                    print("puzzle next");
                    isWindowGetNewQuestion = true;
                    puzzleCompleteCount++;
                    uiUpdateQuestionEvent.Invoke();
                }
            }
            InteractUi.windowTextPuzzleCompleteCount.text = $"Complete:\n{puzzleCompleteCount} / {puzzleCompleteMaximum}";
        }
        InteractUi.windowInputField.text = "";
    }

    private void PuzzleComplete()
    {
        ObjectInteractable.ActiveReaction();
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        InteractUi.windowInputField.ActivateInputField();
    }
}
