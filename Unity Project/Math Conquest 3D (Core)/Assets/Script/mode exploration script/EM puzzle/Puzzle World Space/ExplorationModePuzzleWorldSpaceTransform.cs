using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class ExplorationModePuzzleWorldSpaceTransform : MonoBehaviour
{
    [SerializeField] public Transform objectWorldSpacePuzzle;
    [SerializeField] public List<float> minimumValueList;
    [SerializeField] public List<float> maximumValueList;
    [SerializeField] public List<float> vairableList = new List<float>();
    [SerializeField] public float lerpingSpeed = .1f;
    [SerializeField] public bool isPlayerMoveAlongObject;
    [ShowIf("isPlayerMoveAlongObject")] public float playerStandingOffset;

    
    private ExplorationModeObjectInteractable InteractableObject;
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
        InteractableObject = GetComponent<ExplorationModeObjectInteractable>();
    }
    private void SetupVariable()
    {
        vairableList = PuzzleWorldSpaceWindow.puzzleVariableList;
    }
    private void SetupSubscription()
    {
        PuzzleWorldSpaceWindow.ConfirmValueEvent.AddListener(CheckValueMinMaxValues);
        PuzzleWorldSpaceWindow.ConfirmValueEvent.AddListener(ApplyValuesToText);
        PuzzleWorldSpaceWindow.ConfirmValueEvent.AddListener(ApplyValueToObject);
        PuzzleWorldSpaceWindow.ResetValueEvent.AddListener(ResetObjectValue);
    }

    public virtual void ApplyValueToObject()
    {
        // add method here
    }
    private void CheckValueMinMaxValues()
    {
        for(int i = 0; i < vairableList.Count; i++)
        {
            if(vairableList[i] > maximumValueList[i])
            {
                vairableList[i] = maximumValueList[i];
            }
            if(vairableList[i] < minimumValueList[i])
            {
                vairableList[i] = minimumValueList[i];
            }
        }
    }
    public void ApplyValuesToText()
    {
        int inputfieldCount = 0;
        foreach(TMP_InputField text in PuzzleWorldSpaceWindow.puzzleInputFieldList)
        {
            text.text = vairableList[inputfieldCount].ToString();
            inputfieldCount++;
        }
    }

    public virtual void ResetObjectValue()
    {
        // add reset method
    }

    private void FixedUpdate() 
    {
        if(PuzzleWorldSpaceWindow.isPuzzleWindowActive == true)
        {
            LerpToNewValue();
            MovePlayWithObject();
            print("puzzle active");
        }
    }
    public virtual void LerpToNewValue()
    {
        // add lerping method here
    }
    private void MovePlayWithObject()
    {
        if(isPlayerMoveAlongObject)
        {
            InteractableObject.GameController.playerGameObject.transform.position = objectWorldSpacePuzzle.position + (Vector3.up * playerStandingOffset);
            transform.position = objectWorldSpacePuzzle.position + new Vector3(0, playerStandingOffset + 1.5f, 0.25f);
        }
    }

    private void OnDisable()
    {
        PuzzleWorldSpaceWindow.ConfirmValueEvent.RemoveAllListeners();
        PuzzleWorldSpaceWindow.ResetValueEvent.RemoveAllListeners();
    }
}
