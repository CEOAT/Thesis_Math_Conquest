using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EliminationModePlayerControllerInputAnswer : MonoBehaviour
{
    public TMP_InputField inputField;

    MasterInput playerInput;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {

    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void FixedUpdate()
    {
        inputField.ActivateInputField();
    }

    private void PlayerAnswerEnter()
    {
        inputField.text = "";
    }
    private void PlayerAnswerClear()
    {
        inputField.text = "";
    }
    
}
