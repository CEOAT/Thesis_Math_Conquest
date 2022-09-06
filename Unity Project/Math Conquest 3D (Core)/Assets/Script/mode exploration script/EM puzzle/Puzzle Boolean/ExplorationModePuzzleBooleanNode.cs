using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplorationModePuzzleBooleanNode : MonoBehaviour
{
    public bool isNodeActive;
    public GameObject nodeLight;
    public TMP_Text nodeText;

    private void Start()
    {
        nodeText.GetComponent<TMP_Text>();
    }
    public void NodeOn()
    {
        isNodeActive = true;
        nodeLight.SetActive(true);
        nodeText.text = "T";
    }
    public void NodeOff()
    {
        isNodeActive = false;
        nodeLight.SetActive(false);
        nodeText.text = "F";
    }
}
