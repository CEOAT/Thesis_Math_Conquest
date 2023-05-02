using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineOfsightManager : MonoBehaviour
{
    [SerializeField] private List<StatueDectection> StatuePuzzle = new List<StatueDectection>();
    [SerializeField] private TMP_Dropdown VecDirection_Dropdown;
    [SerializeField] private TextMeshProUGUI Radius;
    [SerializeField]private List<string> VectorNotation_UnityTransform;
    // Start is called before the first frame update

    private int _currentStatueIndex =0;
    void Start()
    {
        VecDirection_Dropdown.ClearOptions();
        VecDirection_Dropdown.AddOptions(VectorNotation_UnityTransform);
        GetCurrentStatus();
        StatuePuzzle[_currentStatueIndex].OnSelectThis();
    }

    public void EnableThisPuzzle()
    {
        VecDirection_Dropdown.ClearOptions();
        VecDirection_Dropdown.AddOptions(VectorNotation_UnityTransform);
        EnebleThisPuzzle();
        GetCurrentStatus();
        StatuePuzzle[_currentStatueIndex].Isfocus = true;
    }

    public void DisableThisPuzzle()
    {
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.Isfocus = false;
            VARIABLE.DisableVisual();
            VARIABLE.UnSelectThis();
            VARIABLE.enabled = false;
        }
    }

    public void EnebleThisPuzzle()
    {
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.EnableVisual();
            VARIABLE.enabled = true;
        }
    }

    public void nextStatue()
    {
        int tempAllIndex = StatuePuzzle.Count;
        _currentStatueIndex++;
        _currentStatueIndex = _currentStatueIndex %tempAllIndex ;
        GetCurrentStatus();
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.Isfocus = false;
        }
        StatuePuzzle[_currentStatueIndex].Isfocus = true;
      
        Debug.Log(_currentStatueIndex);
    }

    public void IncreaseRadius()
    {
        var temp_radius = Mathf.Clamp(StatuePuzzle[_currentStatueIndex].Radius, 2.5f, 8.5f);
        temp_radius += 0.5f;
        StatuePuzzle[_currentStatueIndex].Radius = temp_radius;
        Radius.text = "Radius : " + temp_radius;
            
    }
    public void decreaseRadius()
    {
        var temp_radius = Mathf.Clamp(StatuePuzzle[_currentStatueIndex].Radius, 2.5f, 8.5f);
        temp_radius -= 0.5f;
        StatuePuzzle[_currentStatueIndex].Radius = temp_radius;
        Radius.text = "Radius : " + temp_radius;
    }

   

    public void GetCurrentStatus()
    {
        switch (StatuePuzzle[_currentStatueIndex].DetectDirectionVector)
        {
            case StatueDectection.DetectDirection.Blue_Front :
                VecDirection_Dropdown.value = 0;
                break;
            case StatueDectection.DetectDirection.Blue_Back :
                VecDirection_Dropdown.value = 1;
                break;
            case StatueDectection.DetectDirection.Red_Right :
                VecDirection_Dropdown.value = 2;
                break;
            case StatueDectection.DetectDirection.Red_Left :
                VecDirection_Dropdown.value = 3;
                break;
            default:
                break;;
        }

        Radius.text = "Radius : " + StatuePuzzle[_currentStatueIndex].Radius;
    }
    public void SetCurrentDirection()
    {
        switch (VecDirection_Dropdown.value)
        {
            case 0 :
                StatuePuzzle[_currentStatueIndex].DetectDirectionVector = StatueDectection.DetectDirection.Blue_Front;
               
                break;
            case 1 :
                StatuePuzzle[_currentStatueIndex].DetectDirectionVector = StatueDectection.DetectDirection.Blue_Back;
                break;
            case 2 :
                StatuePuzzle[_currentStatueIndex].DetectDirectionVector = StatueDectection.DetectDirection.Red_Right;
                break;
            case 3 :
                StatuePuzzle[_currentStatueIndex].DetectDirectionVector = StatueDectection.DetectDirection.Red_Left;
                break;
            default:
                break;
        }
    }
}
