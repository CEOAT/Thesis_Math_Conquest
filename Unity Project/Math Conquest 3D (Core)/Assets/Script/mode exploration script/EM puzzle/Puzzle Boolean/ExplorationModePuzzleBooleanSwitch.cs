using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModePuzzleBooleanSwitch : MonoBehaviour
{
    public GameObject switchLight;
    public bool isSwitchActive;

    private void Awake()
    {
        if (isSwitchActive == true)
        {
            switchLight.SetActive(true);
        }
        else
        {
            switchLight.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            if (isSwitchActive == false)
            {
                SwitchOn();
            }
            else
            {
                SwitchOff();
            }
        }
    }
    private void SwitchOn()
    {
        isSwitchActive = true;
        switchLight.SetActive(true);
    }
    private void SwitchOff()
    {
        isSwitchActive = false;
        switchLight.SetActive(false);
    }
}
