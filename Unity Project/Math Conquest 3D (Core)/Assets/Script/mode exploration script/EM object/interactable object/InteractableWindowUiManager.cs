using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableWindowUiManager : MonoBehaviour
{
    [SerializeField] public GameObject windowGroup;
    [SerializeField] public TMP_Text windowTextPuzzleProblem;
    [SerializeField] public TMP_Text windowTextDescription;
    [SerializeField] public TMP_Text windowTextPuzzleCompleteCount;
    [SerializeField] public TMP_InputField windowInputField;
}
