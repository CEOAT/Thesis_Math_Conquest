using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ExplorationModePlayerAttackClearAnswerAnimation : MonoBehaviour
{
    public TMP_Text answerTextWorldSpace;
    public TMP_Text answerTextUi;
   
    public void ClearText()
    {
        answerTextUi.text = "";
        answerTextWorldSpace.text = "";
    }
}