using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class CutsceneControllerInstruction : MonoBehaviour
{
    [Header("Instruction Page Number")]
    public int pageCurrent;
    public int pageTotal;
    public int elementInPageTotal;

    [Header("Instruction Page Objects")]
    public GameObject instructionPageUiGroup;
    public TMP_Text pageNumber;
    public GameObject buttonNextPageObject;
    public TMP_Text buttonNextPageText;
    public GameObject buttonPreviousPageObject;
    public TMP_Text buttonPreviousPageText;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;

    [Header("Active End Instruction Object")]
    public GameObject activeAfterEnInstructionObject;

    [Header("Instruction Setting")]
    public bool isDeactivateAfterEnd;

    [Header("Instruction Data")]
    public InstructionListClass instructionList;

    private MasterInput PlayerInput;

#region awake/start
    private void Awake()
    {
        SetupComponent();
        SetupVariable();
        SetupButtonProperty();
        SetupControl();
    }
    private void SetupComponent()
    {
        pageNumber.GetComponent<TMP_Text>();
        buttonNextPageText.GetComponent<TMP_Text>();
        buttonPreviousPageText.GetComponent<TMP_Text>();
    }
    private void SetupVariable()
    {
        pageTotal = instructionList.instructionSet.Count;
        pageCurrent = 1;
    }
    private void SetupButtonProperty()
    {
        CheckCurrentPageUi();
    }
    private void SetupControl()
    {
        PlayerInput = new MasterInput();
        PlayerInput.WindowControl.NextInstructionPage.performed += context => NextPage();
        PlayerInput.WindowControl.PreviousInstructionPage.performed += context => PreviousPage();
    }

    private void Start()
    {
        instructionPageUiGroup.SetActive(false);
    }
#endregion

#region element display functions
    private void DisplayNewPageElement()
    {
        StartCoroutine(DisplayElementSequence());
    }
    private IEnumerator DisplayElementSequence()
    {
        elementInPageTotal = instructionList.instructionSet[pageCurrent - 1].instructionData.Count;
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            yield return new WaitForSeconds(0.175f);

            GameObject element = instructionList.instructionSet[pageCurrent - 1].instructionData[i].instructionElementObject;
            element.SetActive(true);
            if (element.GetComponent<Animator>() != null)
            {
                print("play animation");
                element.GetComponent<Animator>().Play("Instruction Element Slide Up");
            }
        }
    }
    private void HideLastPageElementNextButton()
    {
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            instructionList.instructionSet[pageCurrent - 1].instructionData[i].instructionElementObject.SetActive(false);
        }
    }
    private void HideLastPageElementPreviousButton()
    {
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            instructionList.instructionSet[pageCurrent - 1].instructionData[i].instructionElementObject.SetActive(false);
        }
    }
#endregion

#region event related function
    private void ButtonSetEvent()
    {
        buttonNextPageObject.GetComponent<Button>().onClick.AddListener(NextPage);
        buttonPreviousPageObject.GetComponent<Button>().onClick.AddListener(PreviousPage);
    }
    private void ButtonRemoveEvent()
    {
        buttonNextPageObject.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonPreviousPageObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
    private void DisplayFirstPage()
    {
        pageCurrent = 1;
        ButtonSetEvent();
        CheckCurrentPageUi();
        DisplayNewPageElement();
    }

#endregion

#region button event function
    // press next page button
    public void NextPage()
    {
        StopAllCoroutines();

        if (pageCurrent == pageTotal)
        {
            HideLastPageElementNextButton();
            CheckCurrentPageUi();
            EndPage();
        }
        else if (pageCurrent < pageTotal)
        {
            HideLastPageElementNextButton();

            pageCurrent++;
            CheckCurrentPageUi();
            DisplayNewPageElement();
        }
    }
    private void EndPage()
    {
        ButtonRemoveEvent();
        PlayerInput.Disable();

        instructionPageUiGroup.SetActive(false);
        HideLastPageElementNextButton();

        pageCurrent = pageTotal;
        GameController.AllowMovement();
        GameController.DisablePauseGame();

        ActiveObjectAtEndInstruction();
        DeactivateAfterEndInstruction();
    }
    private void ActiveObjectAtEndInstruction()
    {
        activeAfterEnInstructionObject.SetActive(true);
    }
    private void DeactivateAfterEndInstruction()
    {
        if (isDeactivateAfterEnd == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    // press previous page button
    public void PreviousPage()
    {
        StopAllCoroutines();

        if (pageCurrent > 1)
        {
            HideLastPageElementPreviousButton();

            pageCurrent--;
            CheckCurrentPageUi();
            DisplayNewPageElement();
        }
    }

    private void CheckCurrentPageUi()
    {
        pageNumber.text = $"{pageCurrent} of {pageTotal}";

        if (pageCurrent == pageTotal)
        {
            buttonNextPageText.text = "End";
            buttonPreviousPageObject.SetActive(true);
        }
        else if(pageCurrent > 1 && pageCurrent < pageTotal)
        {
            buttonNextPageText.text = "Next";
            buttonPreviousPageObject.SetActive(true);
        }
        else if (pageCurrent == 1)
        {
            buttonNextPageText.text = "Next";
            buttonPreviousPageObject.SetActive(false);
        }
    }

#endregion

#region instruction cutscene start and stop
    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            StartInstructionCutscene();
        }
    }
    public void StartInstructionCutscene()
    {
        PlayerInput.Enable();
        instructionPageUiGroup.SetActive(true);
        DisplayFirstPage();
        GameController.TriggerCutscene();
        GameController.DisablePauseGame();
    }
#endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(CutsceneControllerInstruction))]
public class CutsceneControllerInstructionTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CutsceneControllerInstruction instruction = (CutsceneControllerInstruction)target;

        EditorGUI.BeginDisabledGroup(instruction.instructionPageUiGroup.activeSelf == true);
        if (GUILayout.Button("Start Instuction")) { instruction.StartInstructionCutscene(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.instructionPageUiGroup.activeSelf == false);
        if (GUILayout.Button("Next Page")) { instruction.NextPage(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.instructionPageUiGroup.activeSelf == false);
        if (GUILayout.Button("Previous Page")) { instruction.PreviousPage(); }
        EditorGUI.EndDisabledGroup();
    }
}
#endif