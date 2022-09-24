using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplorationModeObjectInteractableWindow : MonoBehaviour
{
    public GameObject windowGroup;
    public TMP_Text windowTextPuzzle;
    public TMP_Text windowTextDescription;
    public TMP_InputField windowInputField;

    public string windowAnswer;
    public bool isWindowReset;

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
    private void SetupObject()
    {
        windowGroup.SetActive(false);
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

    public void SetupWindow(string textPuzzle, string textDescription, string puzzleAnswer)
    {
        windowTextPuzzle.text = textPuzzle;
        windowTextDescription.text = textDescription;
        windowAnswer = puzzleAnswer;
    }

    private void WindowActivation()
    {
        if (windowGroup.activeInHierarchy == false)
        {
            windowGroup.SetActive(true);
        }
        else if (windowGroup.activeInHierarchy == true)
        {
            CloseWindow();
        }
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
                print("correct");
                isWindowReset = true;
            }
        }
        windowInputField.text = "";

    }

    private void FixedUpdate()
    {
        windowInputField.ActivateInputField();
    }
}
