using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotCompassManager : MonoBehaviour
{
    [SerializeField] private StatueDectection statuePuzzle;
    [SerializeField] private TextMeshProUGUI DotProductInfo;
    [SerializeField] private TMP_Dropdown VecDirection_Dropdown;
    [SerializeField]private List<string> VectorNotation_UnityTransform;

    private Coroutine moveright, moveleft, movefoward, moveback;
    private CharacterController tempController;

    public void EnableThisPuzzle()
    {
        statuePuzzle.OnSelectThis();
        statuePuzzle.EnableVisual();
        statuePuzzle.enabled = true;
    }
    public void DisableThisPuzzle()
    {
        statuePuzzle.UnSelectThis();
        statuePuzzle.DisableVisual();
        statuePuzzle.enabled = false;
    }
  
    private void Start()
    {
        EnableThisPuzzle();
        VecDirection_Dropdown.ClearOptions();
        VecDirection_Dropdown.AddOptions(VectorNotation_UnityTransform);
         tempController = statuePuzzle.GetComponent<CharacterController>();
    }
    
    public void EventUnhold(BaseEventData data)
    {
        if(moveleft!=null) StopCoroutine(moveleft);
        if(moveright!=null) StopCoroutine(moveright);
        if(movefoward!=null) StopCoroutine(movefoward);
        if(moveback!=null) StopCoroutine(moveback);
    }


    public void ChangeDirectionVector()
    {
        
        switch (VecDirection_Dropdown.options[VecDirection_Dropdown.value].text)
        {
            case "Transform.Foward" :
                statuePuzzle.ThisCompassDirection = StatueDectection.DetectDirection.Blue_Front;
                break;
            case "-Transform.Foward" :
                statuePuzzle.ThisCompassDirection = StatueDectection.DetectDirection.Blue_Back;
                break;
            case "Transform.Right" :
                statuePuzzle.ThisCompassDirection = StatueDectection.DetectDirection.Red_Right;
                break;
            case "-Transform.Right" :
                statuePuzzle.ThisCompassDirection = StatueDectection.DetectDirection.Red_Left;
                break;
            default:
                break;
        }
    }

    public void UpdateInfomation()
    {
        DotProductInfo.text = statuePuzzle.DotproductInfo;
    }

    public void MoveRight(BaseEventData data)
    {
        moveright = StartCoroutine(EnumMove("moveright"));
    }

    public void MoveLeft(BaseEventData data)
    {
        moveleft =StartCoroutine(EnumMove("moveleft"));
    }

    public void MoveFoward(BaseEventData data)
    {
        movefoward = StartCoroutine(EnumMove("movefoward"));
    }

    public void Moveback(BaseEventData data)
    {
        moveback = StartCoroutine(EnumMove("moveback"));
    }
    
   

    IEnumerator EnumMove(string Input)
    {
        float movespeed = 8;
        switch (Input)
        {
            case var temp when temp=="moveright":
                while(true)
                {
                    tempController.SimpleMove(statuePuzzle.transform.right*movespeed);
                    yield return new WaitForFixedUpdate();
                }
                break;
            case var temp when temp=="moveleft" :
                while(true)
                {
                    tempController.SimpleMove(-statuePuzzle.transform.right *movespeed);
                    yield return new WaitForFixedUpdate();
                }
                break;
            case var temp when temp == "movefoward":
                while(true)
                {
                    tempController.SimpleMove(statuePuzzle.transform.forward *movespeed);
                    yield return new WaitForFixedUpdate();
                }
                break;
            case var temp when temp == "moveback":
                while(true)
                {
                    tempController.SimpleMove(-statuePuzzle.transform.forward *movespeed);
                    yield return new WaitForFixedUpdate();
                }
                break;
            default:
                break;
        }
        yield return null;
    }
}
