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

    [Header("Puzzle Problem and Description")]
    [SerializeField] private string puzzleProblem;
    [SerializeField] private string puzzleDescription;
    private TMP_Text textPuzzleProblem;
    private TMP_Text textPuzzleDescription;

    [Header("List of Inputfield")]
    [SerializeField] private List<InputField> puzzleInputFieldList = new List<InputField>();
    [SerializeField] public List<float> puzzleVariableList = new List<float>();
    [SerializeField] private int inputFieldSelectedIndex = 0;
    [SerializeField] private GameObject inputFieldSelectedObject;
    private int inputFieldTotal;

    [Header("Puzzle Camera")]
    [SerializeField] private GameObject worldSpaceCamera;

    [HideInInspector] public UnityEvent ConfirmValueEvent;
    private MasterInput PlayerInput;

    private void Awake()
    {
        SetupInput();
        SetupObject();
        SetupVariable();
    }
    private void SetupInput()
    {
        MasterInput PlayerInput = new MasterInput();
        PlayerInput.WindowControl.CloseWindow.performed += context => CloseWorldSpacePuzzle();
        PlayerInput.WindowControl.SwitchInputField.performed += context => SwitchInputField();
        PlayerInput.WindowControl.ConfirmAnswer.performed += context => ConfirmValue();
    }
    private void SetupObject()
    {
        worldSpaceCamera.SetActive(false);
    }
    private void SetupVariable()
    {
        inputFieldTotal = puzzleInputFieldList.Count;
    }

    private void Start()
    {
        EnableWorldSpacePuzzleCutscenAndInput();
        StartCoroutine(CreatePuzzleWindow());
    }

    private void EnableWorldSpacePuzzleCutscenAndInput()
    {
        PlayerInput.Enable();
        GameController.TriggerCutscene();
    }
    private IEnumerator CreatePuzzleWindow()
    {
        worldSpaceCamera.SetActive(true);
        yield return new WaitForSeconds(2f);
        InstantiatePuzzleWindow();
        SetPuzzleWindowComponent();
        SetPuzzleWindowText();
    }
    private void InstantiatePuzzleWindow()
    {
        puzzleWindowObject = Instantiate(puzzleWindowPrefab);
        puzzleWindowObject.transform.SetParent(inGameCanvas);
    }
    private void SetPuzzleWindowComponent()
    {
        textPuzzleProblem = puzzleWindowObject.transform.GetChild(0).GetComponent<TMP_Text>();
        textPuzzleDescription = puzzleWindowObject.transform.GetChild(1).GetComponent<TMP_Text>();
    }
    private void SetPuzzleWindowText()
    {
        textPuzzleProblem.text = puzzleProblem;
        textPuzzleDescription.text = puzzleDescription;
    }

    private void SwitchInputField()
    {
        if (inputFieldTotal > 1)
        {
            inputFieldSelectedIndex++;
        }
    }
    public void ConfirmValue()
    {
        puzzleVariableList.Clear();
        foreach (InputField inputField in puzzleInputFieldList)
        {
            puzzleVariableList.Add(float.Parse(inputField.text));
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
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        ActiveInputFieldByindex();
    }
    private void ActiveInputFieldByindex()
    {
        puzzleInputFieldList[inputFieldSelectedIndex].ActivateInputField();
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