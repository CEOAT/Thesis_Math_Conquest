using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneControllerInstruction : MonoBehaviour
{
    [Header("Instruction Page Number")]
    public int pageIndex;
    public int pageTotal;

    [Header("Instruction Page Objects")]
    public GameObject instructionPageUiGroup;
    public TMP_Text pageNumber;
    public GameObject buttonNextPageObject;
    public TMP_Text buttonNextPageText;
    public GameObject buttonPreviousPageObject;
    public TMP_Text buttonPreviousPageText;

    [Header("Game Controller Object")]
    public ExplorationModeGameController GameController;

    [Header("Instruction Data")]
    public InstructionListClass instructionList = new InstructionListClass();

#region awake/start
    private void Awake()
    {
        SetupComponent();
        SetupVariable();
    }
    private void SetupComponent()
    {
        pageNumber.GetComponent<TMP_Text>();
        buttonNextPageText.GetComponent<TMP_Text>();
        buttonPreviousPageText.GetComponent<TMP_Text>();
    }
    private void SetupVariable()
    {
        pageIndex = 0;
        pageTotal = instructionList.instructionSet[0].instructionData.Count;
    }

    private void Start()
    {
        DisplayFirstTime();
        GameController.TriggerCutscene();
    }
    private void DisplayFirstTime()
    {
        DisplayElement();
    }
#endregion

#region element display functions
    private void DisplayElement()
    {
        StartCoroutine(DisplayElementSequence());
    }
    private IEnumerator DisplayElementSequence()
    {
        for (int i = 0; i <= pageTotal - 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            instructionList.instructionSet[0].instructionData[i].instructionElementObject.SetActive(true);
        }
    }
#endregion

#region button event function

    // press next page button
    private void NextPage()
    {
        if (pageIndex < pageTotal)
        {
            pageIndex++;
        }
        if (pageIndex == pageTotal)
        {
            ChangeNextPageToEnd();
        }
    }
    private void ChangeNextPageToEnd()
    {
        buttonNextPageText.text = "End";
    }

    // press previous page button
    private void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
        }
        if (pageIndex == 0)
        {
            HidePreviousPage();
        }
    }
    private void HidePreviousPage()
    {
        buttonPreviousPageObject.SetActive(false);
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
    private void StartInstructionCutscene()
    {
        instructionPageUiGroup.SetActive(true);
        GameController.TriggerCutscene();
    }
    private void EndPage()
    {
        instructionPageUiGroup.SetActive(false);
    }
#endregion

}