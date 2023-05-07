using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Sirenix.OdinInspector;

public class CutsceneControllerInstructionBook : MonoBehaviour
{
    private bool isComponentSetupFirstTime = false;

    [Header("Instruction Page Number")]
    [ReadOnly] private int pageCurrent;
    private int pageTotal;
    private int elementInPageTotal;

    [Header("Instruction Element")]
    [SerializeField] public GameObject instructionGroup;
    [SerializeField] private GameObject contentIndexGroup;
    [SerializeField] private TMP_Text textPageCount;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Button buttonPrevious;
    [SerializeField] private TMP_Text textButtonNext;
    [SerializeField] private TMP_Text textButtonPrevious;

    [Header("Instruction Setting")]
    public bool isDeactivateAfterEnd = true;

    [Header("Instruction Data")]
    public InstructionListClass instructionList;

    private MasterInput PlayerInput;

#region awake/start
    private void Awake()
    {
        if(isComponentSetupFirstTime == false)
        {
            SetupComponent();
            SetupVariable();
            SetupButtonProperty();
            SetupControl();
            isComponentSetupFirstTime = true;
        }
    }
    private void SetupComponent()
    {
        textPageCount.GetComponent<TMP_Text>();
        textButtonNext.GetComponent<TMP_Text>();
        textButtonPrevious.GetComponent<TMP_Text>();
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
        instructionGroup.SetActive(false);
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
            GameObject element = instructionList.instructionSet[pageCurrent - 1].instructionData[i].instructionElementObject;
            element.SetActive(true);
            yield return null;
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
        buttonNext.GetComponent<Button>().onClick.AddListener(NextPage);
        buttonPrevious.GetComponent<Button>().onClick.AddListener(PreviousPage);
    }
    private void ButtonRemoveEvent()
    {
        buttonNext.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonPrevious.GetComponent<Button>().onClick.RemoveAllListeners();
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

        instructionGroup.SetActive(false);
        contentIndexGroup.SetActive(true);

        HideLastPageElementNextButton();

        pageCurrent = pageTotal;

        DeactivateAfterEndInstruction();
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
        if(pageCurrent == 1)
        {
            return;
        }

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
        textPageCount.text = $"{pageCurrent} of {pageTotal}";

        if(pageTotal == 1)
        {
            textButtonNext.text = "End";
            buttonPrevious.gameObject.SetActive(false);
        }
        else if (pageCurrent == pageTotal)
        {
            textButtonNext.text = "End";
            buttonPrevious.gameObject.SetActive(true);
        }
        else if(pageCurrent > 1 && pageCurrent < pageTotal)
        {
            textButtonNext.text = "Next";
            buttonPrevious.gameObject.SetActive(true);
        }
        else if (pageCurrent == 1)
        {
            textButtonNext.text = "Next";
            buttonPrevious.gameObject.SetActive(false);
        }
    }
#endregion

#region instruction cutscene start and stop
    public void StartInstruction()
    {
        PlayerInput.Enable();
        instructionGroup.SetActive(true);
        contentIndexGroup.SetActive(false);
        DisplayFirstPage();
    }
#endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(CutsceneControllerInstructionBook))]
public class CutsceneControllerInstructionBookTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CutsceneControllerInstructionBook instruction = (CutsceneControllerInstructionBook)target;

        EditorGUI.BeginDisabledGroup(instruction.instructionGroup.activeSelf == true);
        if (GUILayout.Button("Start Instuction")) { instruction.StartInstruction(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.instructionGroup.activeSelf == false);
        if (GUILayout.Button("Next Page")) { instruction.NextPage(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.instructionGroup.activeSelf == false);
        if (GUILayout.Button("Previous Page")) { instruction.PreviousPage(); }
        EditorGUI.EndDisabledGroup();
    }
}
#endif