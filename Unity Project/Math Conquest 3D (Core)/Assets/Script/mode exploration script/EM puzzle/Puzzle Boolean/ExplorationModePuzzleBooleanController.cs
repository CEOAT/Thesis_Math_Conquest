using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleBooleanController : MonoBehaviour
{
    public List<ExplorationModePuzzleBooleanSwitch> booleanSwitch = new List<ExplorationModePuzzleBooleanSwitch>();
    public List<ExplorationModePuzzleBooleanNode> booleanNodes = new List<ExplorationModePuzzleBooleanNode>();
    public List<string> booleanOperator = new List<string>();

    public GameObject booleanDoor;
    public GameObject booleanLight;

    public void Start()
    {
        booleanLight.SetActive(false);
        InvokeRepeating("CheckNodeActivation", 1f, 0.3f);
    }

    private void CheckNodeActivation()
    {
        for (int i = 0; i < booleanSwitch.Count; i++)
        {
            if (booleanSwitch[i].isSwitchActive == true)
            {
                booleanNodes[i].NodeOn();
            }
            if (booleanSwitch[i].isSwitchActive == false)
            {
                booleanNodes[i].NodeOff();
            }
        }
        CheckNodeBoolean();
    }
    private void CheckNodeBoolean()
    {
        if (booleanSwitch[0].isSwitchActive == true &&
            booleanSwitch[1].isSwitchActive == true &&
            booleanSwitch[2].isSwitchActive == true)
        {
            booleanLight.SetActive(true);
            booleanDoor.GetComponent<Animation>().Play();
            CancelInvoke("CheckNodeActivation");
        }
    }
    
}
