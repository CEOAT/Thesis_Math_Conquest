using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
using TMPro;


public class ExplorationModePuzzleWorldSpaceWindow : MonoBehaviour
{
    [Header("Game Controller")]
    [SerializeField] private ExplorationModeGameController GameController;

    [Header("World Space Puzzle Window")]
    [SerializeField] private Transform inGameCanvas;
    [SerializeField] private GameObject puzzleWindowPrefab;
    private GameObject puzzleWindowObject;
    [SerializeField] public bool isPuzzleWindowActive = false;

    [Header("Puzzle Problem and Description")]
    [SerializeField] private string puzzleProblem;
    [SerializeField] private string puzzleDescription;
    private TMP_Text textPuzzleProblem;
    private TMP_Text textPuzzleDescription;

    [Header("List of Inputfield")]
    public List<TMP_InputField> puzzleInputFieldList = new List<TMP_InputField>();
    [SerializeField] public List<float> puzzleVariableList = new List<float>();
    private int inputFieldSelectedIndex = 0;
    private GameObject inputFieldSelectedObject;

    [Header("Puzzle Camera")]
    [SerializeField] private GameObject worldSpaceCamera;

    [HideInInspector] public UnityEvent ConfirmValueEvent;
    [HideInInspector] public UnityEvent ResetValueEvent;
    private MasterInput PlayerInput;

    private void Awake()
    {
        SetupObject();
    }
    private void SetupObject()
    {
        worldSpaceCamera.SetActive(false);
    }
    private void Start()
    {
        SetupInput();
    }
    private void SetupInput()
    {
        PlayerInput = new MasterInput();
        PlayerInput.WindowControl.CloseWindow.performed += context => CloseWorldSpacePuzzle();
        PlayerInput.WindowControl.SwitchInputField.performed += context => SwitchInputField();
        PlayerInput.WindowControl.ConfirmAnswer.performed += context => ConfirmValue();
        PlayerInput.WindowControl.ResetAnswer.performed += context => ResetValue();
    }

    public void StartMultipleChoicePuzzleWindow()
    {
        EnableWorldSpacePuzzleCutscenAndInput();
        StartCoroutine(CreatePuzzleWindow());
    }
    private void EnableWorldSpacePuzzleCutscenAndInput()
    {
        GameController.TriggerCutscene();
    }
    private IEnumerator CreatePuzzleWindow()
    {
        worldSpaceCamera.SetActive(true);
        yield return new WaitForSeconds(1f);
        InstantiatePuzzleWindow();
        SetPuzzleWindowComponent();
        SetInputFieldList();
        SetPuzzleWindowText();
        isPuzzleWindowActive = true;
        PlayerInput.Enable();
    }
    private void InstantiatePuzzleWindow()
    {
        puzzleWindowObject = Instantiate(puzzleWindowPrefab);
    }
    private void SetPuzzleWindowComponent()
    {
        textPuzzleProblem = puzzleWindowObject.transform.GetChild(0).GetComponent<TMP_Text>();
        textPuzzleDescription = puzzleWindowObject.transform.GetChild(1).GetComponent<TMP_Text>();
    }
    private void SetInputFieldList()
    {
        puzzleInputFieldList.Clear();
        for(int i = 2; i <= puzzleWindowObject.transform.childCount - 3; i++)
        {
            puzzleInputFieldList.Add(puzzleWindowObject.transform.GetChild(i).GetComponent<TMP_InputField>());
        }
    }
    private void SetPuzzleWindowText()
    {
        textPuzzleProblem.text = puzzleProblem;
        textPuzzleDescription.text = puzzleDescription;
        
    }

    private void SwitchInputField()
    {
        if (puzzleInputFieldList.Count > 1)
        {
            CheckInputFieldIndex();
        }
    }
    private void CheckInputFieldIndex()
    {
        inputFieldSelectedIndex++;
        if(inputFieldSelectedIndex > puzzleInputFieldList.Count - 1)
        {
            inputFieldSelectedIndex = 0;
        }
    }

    public void ConfirmValue()
    {
        puzzleVariableList.Clear();
        foreach (TMP_InputField inputField in puzzleInputFieldList)
        {
            if(inputField.text != "")
            {
                puzzleVariableList.Add(float.Parse(inputField.text));
            }
            else
            {
                puzzleVariableList.Add(0f);
            }
        }
        ConfirmValueEvent.Invoke();
    }
    private void CloseWorldSpacePuzzle()
    {
        StartCoroutine(CloseWorldSpacePuzzleSequence());
    }
    private IEnumerator CloseWorldSpacePuzzleSequence()
    {
        PlayerInput.Disable();
        GameObject.Destroy(puzzleWindowObject);
        worldSpaceCamera.SetActive(false);

        yield return new WaitForSeconds(0.8f);
        GameController.AllowMovement();
        this.enabled = false;
        isPuzzleWindowActive = false;
    }
    private void CloseWorldSpacePuzzleDeactived()
    {
        PlayerInput.Disable();
        GameObject.Destroy(puzzleWindowObject);
        worldSpaceCamera.SetActive(false);
        GameController.AllowMovement();
        this.enabled = false;
        isPuzzleWindowActive = false;
    }

    private void ClearAnswer()
    {
        puzzleInputFieldList[inputFieldSelectedIndex].text = "";
    }
    public void ResetValue()
    {
        ResetValueEvent.Invoke();
    }

    private void FixedUpdate()
    {
        ActiveInputFieldByindex();
    }
    private void ActiveInputFieldByindex()
    {
        if(isPuzzleWindowActive == true)
        {
            puzzleInputFieldList[inputFieldSelectedIndex].ActivateInputField();
        }
    }

    private void OnDisable() 
    {
        CloseWorldSpacePuzzleDeactived();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ExplorationModePuzzleWorldSpaceWindow))]
public class ExplorationModePuzzleWorldSpaceTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExplorationModePuzzleWorldSpaceWindow WorldSpacePuzzle = (ExplorationModePuzzleWorldSpaceWindow)target;

        if (GUILayout.Button("Confirm Value")) { WorldSpacePuzzle.ConfirmValue(); }
    }
}
#endif