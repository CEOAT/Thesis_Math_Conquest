using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using Sirenix.OdinInspector;

public class CutsceneControllerInstruction : MonoBehaviour
{
    [Header("Instruction Page Number")]
    private int pageCurrent;
    private int pageTotal;
    private int elementInPageTotal;

    [Header("Instruction Manager")]
    [SerializeField] public InstructionManager InstructionManager;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;

    [Header("Active End Instruction Object")]
    [SerializeField] private List<GameObject> activeAfterEndInstructionObjectList = new List<GameObject>();
    [SerializeField] private List<GameObject> disableAfterEndInstructionObjectList = new List<GameObject>();
    private bool isInstructionObjectActivated;
    private bool isInstructionObjectDisabled;

    [Header("Instruction Setting")]
    public bool isDeactivateAfterEnd;
    public bool isSelfDestroyAfterEnd;

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
        InstructionManager.pageNumber.GetComponent<TMP_Text>();
        InstructionManager.buttonNextPageText.GetComponent<TMP_Text>();
        InstructionManager.buttonPreviousPageText.GetComponent<TMP_Text>();
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
        InstructionManager.instructionPageUiGroup.SetActive(false);
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
        InstructionManager.buttonNextPageObject.GetComponent<Button>().onClick.AddListener(NextPage);
        InstructionManager.buttonPreviousPageObject.GetComponent<Button>().onClick.AddListener(PreviousPage);
    }
    private void ButtonRemoveEvent()
    {
        InstructionManager.buttonNextPageObject.GetComponent<Button>().onClick.RemoveAllListeners();
        InstructionManager.buttonPreviousPageObject.GetComponent<Button>().onClick.RemoveAllListeners();
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

        InstructionManager.instructionPageUiGroup.SetActive(false);
        HideLastPageElementNextButton();

        pageCurrent = pageTotal;
        GameController.AllowMovement();

        ActiveObjectAtEndInstruction();
        DisableObjectAfterInstruction();
        DeactivateAfterEndInstruction();
        GameController.EnablePauseGame();
    }
    private void ActiveObjectAtEndInstruction()
    {
        if (activeAfterEndInstructionObjectList.Count > 0 && isInstructionObjectActivated == false)
        {
            isInstructionObjectActivated = true;
            LoopActiveObject();
        }
    }
    private void LoopActiveObject()
    {
        foreach(GameObject activeObject in activeAfterEndInstructionObjectList)
        {
            if(activeObject != null)
            {
                activeObject.SetActive(true);
                if(activeObject.TryGetComponent<BoxCollider>(out BoxCollider collider))
                {
                    collider.GetComponent<BoxCollider>().center = new Vector3(0,5,0);
                    collider.GetComponent<BoxCollider>().center = new Vector3(0,0,0);
                }
            }
        }
    }
    private void DisableObjectAfterInstruction()
    {
        if (disableAfterEndInstructionObjectList.Count > 0 && isInstructionObjectDisabled == false)
        {
            isInstructionObjectDisabled = true;
            LoopDisableObject();
        }
    }
    private void LoopDisableObject()
    {
        foreach(GameObject disableObject in activeAfterEndInstructionObjectList)
        {
            if(disableObject != null)
                disableObject.SetActive(false);
        }
    }

    private void DeactivateAfterEndInstruction()
    {
        if (isDeactivateAfterEnd == true)
        {
            this.gameObject.SetActive(false);
        }
        else if (isSelfDestroyAfterEnd == true)
        {
            Destroy(this.gameObject);
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
        InstructionManager.pageNumber.text = $"{pageCurrent} of {pageTotal}";

        if (pageCurrent == pageTotal)
        {
            InstructionManager.buttonNextPageText.text = "End";
            InstructionManager.buttonPreviousPageObject.SetActive(true);
        }
        else if(pageCurrent > 1 && pageCurrent < pageTotal)
        {
            InstructionManager.buttonNextPageText.text = "Next";
            InstructionManager.buttonPreviousPageObject.SetActive(true);
        }
        else if (pageCurrent == 1)
        {
            InstructionManager.buttonNextPageText.text = "Next";
            InstructionManager.buttonPreviousPageObject.SetActive(false);
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
        InstructionManager.instructionPageUiGroup.SetActive(true);
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

        EditorGUI.BeginDisabledGroup(instruction.InstructionManager.instructionPageUiGroup.activeSelf == true);
        if (GUILayout.Button("Start Instuction")) { instruction.StartInstructionCutscene(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.InstructionManager.instructionPageUiGroup.activeSelf == false);
        if (GUILayout.Button("Next Page")) { instruction.NextPage(); }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(instruction.InstructionManager.instructionPageUiGroup.activeSelf == false);
        if (GUILayout.Button("Previous Page")) { instruction.PreviousPage(); }
        EditorGUI.EndDisabledGroup();
    }
}
#endif