using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotNormManager : MonoBehaviour
{
    [SerializeField] private List<StatueDectection> StatuePuzzle = new List<StatueDectection>();
    [SerializeField] private TextMeshProUGUI DotProductInfo;
    [SerializeField] private Transform WayBlock;
    private int _currentStatueIndex =0;
    private void Start()
    {
        StatuePuzzle[_currentStatueIndex].OnSelectThis();
    }

    public void EnableThisPuzzle()
    {
        StatuePuzzle[_currentStatueIndex].OnSelectThis();
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.EnableVisual();
            VARIABLE.enabled = true;
        }
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

    private void Update()
    {
        DotProductInfo.text = $"DotProduct(A•B): {StatuePuzzle[_currentStatueIndex].ThisDotproductvar.ToString("F1")} \n " +
                              $"Acos(A•B): {(Mathf.Acos(StatuePuzzle[_currentStatueIndex].ThisDotproductvar)*Mathf.Rad2Deg).ToString("F0")}°";
        GetCurrentStatus();
    }
    public void nextStatue()
    {
        int tempAllIndex = StatuePuzzle.Count;
        _currentStatueIndex++;
        _currentStatueIndex = _currentStatueIndex %tempAllIndex ;
       
        foreach (var VARIABLE in StatuePuzzle)
        {
            VARIABLE.Isfocus = false;
            VARIABLE.UnSelectThis();
        }
        
        StatuePuzzle[_currentStatueIndex].Isfocus = true;
        StatuePuzzle[_currentStatueIndex].OnSelectThis(); 
    }

    private Coroutine Right, left,StatueMatch;
    private bool puzzlepased = false;
    public void GetCurrentStatus()
    {
        if (!puzzlepased)
        {
            var temppass = 0;
            foreach (var statue in StatuePuzzle)
            {
                if(statue.ThisPuzzleDotMatch)
                {
                    temppass++;
                }
            }
            if (temppass == StatuePuzzle.Count)
            {
                puzzlepased = true;
                StartCoroutine(DotPuzzleEventDone());
            }
        }
       
    }

    IEnumerator DotPuzzleEventDone()
    {
        var tempBlockAnimation = WayBlock.GetComponent<Animator>();
        tempBlockAnimation.Play("RockMove");
        yield return null;
    }

    public void EventHold(BaseEventData data)
    {
     
    }

    public void EventUnhold(BaseEventData data)
    {
        if(left!=null) StopCoroutine(left);
        if(Right!=null) StopCoroutine(Right);
    }

    public void RotateLeft(BaseEventData data)
    {
        left = StartCoroutine(RotateLeftLoop());
    }

    IEnumerator RotateLeftLoop()
    {
        while (true)
        {
            var localAngles = StatuePuzzle[_currentStatueIndex].transform.localEulerAngles;
            localAngles.y = (localAngles.y - 4f) % 360f;
            StatuePuzzle[_currentStatueIndex].transform.localEulerAngles = localAngles;
            yield return new WaitForFixedUpdate();
        }
    }

    public void RotateRight(BaseEventData data)
    {
        Right = StartCoroutine(RotateRightLoop());
    }
    IEnumerator RotateRightLoop()
    {
        while (true)
        {
            var localAngles = StatuePuzzle[_currentStatueIndex].transform.localEulerAngles;
            localAngles.y = (localAngles.y + 4) % 360f;
            StatuePuzzle[_currentStatueIndex].transform.localEulerAngles = localAngles;
            yield return new WaitForFixedUpdate();
        }
    }

   
}
