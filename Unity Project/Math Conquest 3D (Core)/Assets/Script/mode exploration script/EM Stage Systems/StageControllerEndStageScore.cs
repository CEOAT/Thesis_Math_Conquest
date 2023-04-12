using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerEndStageScore : MonoBehaviour
{
    [SerializeField] List<int> timeCountGrade;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] private int timeCount;
    private bool isIimeCounting = false;

    private void Start()
    {
        StartTimeCount();
    }
    private void StartTimeCount()
    {
        isIimeCounting = true;
        InvokeRepeating("TimeCount",0f,1f);
    }
    public void StopTimeCount()
    {
        isIimeCounting = false;
    }
    private void TimeCount()
    {
        if(isIimeCounting)
        {
            timeCount += 1;
        }
    }

    public string CheckTime()
    {
        string timeText = $"{timeCount/60} Minute {timeCount%60} Second";
        return timeText;
    }
    public string CheckGrade()
    {
        string gradeDescription = "";
        if(timeCount < timeCountGrade[0])
        {
            gradeDescription = "Quick as a flash!!";
        }
        else if(timeCountGrade[0] <= timeCount && timeCount < timeCountGrade[1])
        {
            gradeDescription = "You can be much faster!";
        }
        else if(timeCountGrade[1] <= timeCount && timeCount <= timeCountGrade[2])
        {
            gradeDescription = "You need to be faster!";
        }
        return gradeDescription;
    }
}