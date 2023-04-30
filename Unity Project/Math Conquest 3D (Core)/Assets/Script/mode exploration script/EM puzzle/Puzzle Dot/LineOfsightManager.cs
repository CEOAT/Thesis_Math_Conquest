using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineOfsightManager : MonoBehaviour
{
    [SerializeField] private List<StatueDectection> StatuePuzzle = new List<StatueDectection>();
    [SerializeField] private TMP_Dropdown VecDirection_Dropdown;

    [SerializeField]private List<string> VectorNotation_UnityTransform;
    // Start is called before the first frame update

    private int _currentStatueIndex =0;
    void Start()
    {
        VecDirection_Dropdown.ClearOptions();
        VecDirection_Dropdown.AddOptions(VectorNotation_UnityTransform);
        GetCurrentDirection();
        StatuePuzzle[_currentStatueIndex].Isfocus = true;
    }

    public void nextStatue()
    {
        int tempAllIndex = StatuePuzzle.Count;
        _currentStatueIndex++;
        _currentStatueIndex = _currentStatueIndex %tempAllIndex ;
        GetCurrentDirection();
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.Isfocus = false;
        }
        StatuePuzzle[_currentStatueIndex].Isfocus = true;
      
        Debug.Log(_currentStatueIndex);
    }

   

    public void GetCurrentDirection()
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
