using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

public class CutsceneControllerInstructionBook : MonoBehaviour
{
    private bool isComponentSetupFirstTime = false;

    [Header("Instruction Page Number")]
    [SerializeField] private int pageCurrent;
    private int pageTotal;
    private int elementInPageTotal;

    [Header("Turn Page SFX")]
    [SerializeField] private AudioSource audioSFX;

    [Header("Instruction Element")]
    [SerializeField] public GameObject instructionGroup;
    [SerializeField] public GameObject instructionButtonGroup;
    [SerializeField] private GameObject contentIndexGroup;
    [SerializeField] private TMP_Text textPageCount;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Button buttonPrevious;
    [SerializeField] private Button buttonBack;
    [SerializeField] private TMP_Text textButtonNext;
    [SerializeField] private TMP_Text textButtonPrevious;


    [Header("Instruction Data")]
    public InstructionListClassBook instructionList;

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
        PlayerInput.WindowControl.CloseWindow.performed += context => EndPage();
    }
#endregion

#region element display functions
    private void DisplayNewPageElement()
    {
        StartCoroutine(DisplayElementSequence());
    }
    private IEnumerator DisplayElementSequence()
    {
        elementInPageTotal = instructionList.instructionSet[pageCurrent - 1].instructionElementObjectList.Count;
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            GameObject element = instructionList.instructionSet[pageCurrent - 1].instructionElementObjectList[i];
            element.SetActive(true);
            yield return null;
        }
    }
    private void HideLastPageElementNextButton()
    {
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            instructionList.instructionSet[pageCurrent - 1].instructionElementObjectList[i].SetActive(false);
        }
    }
    private void HideLastPageElementPreviousButton()
    {
        for (int i = 0; i <= elementInPageTotal - 1; i++)
        {
            instructionList.instructionSet[pageCurrent - 1].instructionElementObjectList[i].SetActive(false);
        }
    }
#endregion

#region event related function
    private void ButtonSetEvent()
    {
        buttonNext.GetComponent<Button>().onClick.AddListener(NextPage);
        buttonPrevious.GetComponent<Button>().onClick.AddListener(PreviousPage);
        buttonBack.GetComponent<Button>().onClick.AddListener(EndPage);
    }
    private void ButtonRemoveEvent()
    {
        buttonNext.GetComponent<Button>().onClick.RemoveListener(NextPage);
        buttonPrevious.GetComponent<Button>().onClick.RemoveListener(PreviousPage);
        buttonBack.GetComponent<Button>().onClick.RemoveListener(EndPage);
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
        audioSFX.Play();

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
    public void EndPage()
    {
        ButtonRemoveEvent();
        PlayerInput.Disable();

        instructionGroup.SetActive(false);
        contentIndexGroup.SetActive(true);

        HideLastPageElementNextButton();

        pageCurrent = pageTotal;

        BackInstruction();
    }

    // press previous page button
    public void PreviousPage()
    {
        if(pageCurrent == 1)
        {
            return;
        }

        StopAllCoroutines();
        audioSFX.Play();

        if (pageCurrent > 1)
        {
            HideLastPageElementPreviousButton();

            pageCurrent--;
            CheckCurrentPageUi();
            DisplayNewPageElement();
        }
    }

    public void BackInstruction()
    {
        contentIndexGroup.SetActive(true);
        instructionButtonGroup.SetActive(false);
        this.gameObject.SetActive(false);
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
    private void OnEnable() 
    {
        StartInstruction();
    }

    public void StartInstruction()
    {
        PlayerInput.Enable();
        instructionGroup.SetActive(true);
        instructionButtonGroup.SetActive(true);
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