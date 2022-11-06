using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleBooleanController : MonoBehaviour
{
    public string booleanAnswerString;
    public string booleanSwitchString;
    public int booleanNodeCount = 0;

    [Header("Boolean Puzzle Setting")]
    [Tooltip("Allow the answer to be random")]
    public bool enableRandomBooleanAnswer = false;

    [Header("List of Switch and Light Node")]
    public GameObject doorObject;
    public List<ExplorationModePuzzleBooleanSwitch> booleanSwitch = new List<ExplorationModePuzzleBooleanSwitch>();
    public List<ExplorationModePuzzleBooleanNode> booleanNodes = new List<ExplorationModePuzzleBooleanNode>();
    public List<string> booleanOperator = new List<string>();

    public void Start()
    {
        SetupBooleanNode();
        SetupBooleanAnswer();
        InvokeRepeating("CheckSwtichActivation", 1f, 0.3f);
    }
    private void SetupBooleanNode()
    {
        booleanNodeCount = booleanSwitch.Count;
    }
    private void SetupBooleanAnswer()
    {
        string[] booleanString = { "T", "F" };

        if (enableRandomBooleanAnswer == true)
        {
            for (int i = 0; i < booleanNodeCount; i++)
            {
                booleanAnswerString += booleanString[Random.Range(0,2)];
            }
        }
    }

    private void CheckSwtichActivation()
    {
        CheckBooleanSwitch();
        CheckBooleanAnswer();
    }
    private void CheckBooleanSwitch()
    {
        booleanSwitchString = "";
        for (int i = 0; i < booleanNodeCount; i++)
        {
            if (booleanSwitch[i].isSwitchActive == true)
            {
                booleanNodes[i].NodeOn();
                booleanSwitchString += "T";
            }
            if (booleanSwitch[i].isSwitchActive == false)
            {
                booleanNodes[i].NodeOff();
                booleanSwitchString += "F";
            }
        }
    }
    private void CheckBooleanAnswer()
    {
        if (booleanAnswerString == booleanSwitchString)
        {
            doorObject.GetComponent<Animation>().Play();
            CancelInvoke("CheckSwtichActivation");
        }
    }
}
