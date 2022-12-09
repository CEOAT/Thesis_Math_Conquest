using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control quetion generating for basic calculation addition, subtraction, nultiplication, division

public class EnemyControllerQuestionBasicCalculation : MonoBehaviour
{
    EnemyControllerStatus enemyStatus;

    [Header("Operators")]
    [Tooltip("True: put addition to random pool.")] public bool useAddition;
    [Tooltip("True: put subtraction to random pool.")] public bool useSubtraction;
    [Tooltip("True: put multiplication to random pool.")] public bool useMultiplication;
    [Tooltip("True: put divition to random pool.")] public bool useDivision;

    [Header("Decimal")]
    [Tooltip("Choose the type of decimal.")] public DecimalSetting decimalSetting;
    public enum DecimalSetting
    {
        nonDecimalNumber,
        decimalNumber,
        nonDecimalAndDecimal
    };
    [Tooltip("Decimal position set for A and B.")] public int decimalPostion;
    [Tooltip("Create diffent decimal position for B. Default is zero, both A and B has same decimal position.")] public int decimalVariation;

    [Header("Negative Type")]
    [Tooltip("The answer and question are positive number.")] public bool negativeNon = true;
    [Tooltip("The A will be set as negative number.")] public bool negativeA = false;
    [Tooltip("Both A and B will be set as negative number.")] public bool negativeAB = false;

    [Header("Limitation")] 
    [Tooltip("1 is not allowed as an answer for multiplication and division.")] public bool isAllowOne;

    [Header("Answer Option")]
    [Tooltip("Minimum answer value.")] public float minimumAnswerValue;
    [Tooltip("Maximum answer value.")] public float maximumAnswerValue;

    [Header("Equation System")]
    [Tooltip("Work as equation?")] public bool isEquation;
    [Tooltip("Choose the type of decimal.")] public EquationSetting equationSetting;
    public enum EquationSetting
    {
        xAsA,
        xAsB,
        xRandom
    };


    private List<string> usedOperatorList = new List<string>();
    private string usedOperatorForQuestion;

    private List<string> usedDecimaTypelList = new List<string>();
    private string usedDecimalType;

    private List<string> usedNegativeTypeList = new List<string>();
    private string usedNegativeType;

    [Header("A B and Answer")]
    [SerializeField] private float valueA;
    [SerializeField] private float valueB;
    [SerializeField] private string answerString;
    private string questionString;

    private void Start()
    {
        SetupEnemyComponent();
        SetupEnemySetting();
        SetupEnemyQuestion();
    }
    private void SetupEnemyComponent()
    {
        enemyStatus = GetComponent<EnemyControllerStatus>();
    }
    private void SetupEnemySetting()
    {
        SetupEnemySettingOperator();
        SetupEnemySettingDecimal();
        SetupEnemySettingNegativity();
    }
    private void SetupEnemySettingOperator()
    {
        if (useAddition)
        {
            usedOperatorList.Add("addition");
        }
        if (useSubtraction)
        {
            usedOperatorList.Add("subtraction");
        }
        if (useMultiplication)
        {
            usedOperatorList.Add("multiplication");
        }
        if (useDivision)
        {
            usedOperatorList.Add("division");
        }
    }
    private void SetupEnemySettingDecimal()
    {
        if (decimalSetting == DecimalSetting.nonDecimalNumber)
        {
            usedDecimaTypelList.Add("non decimal number");
        }
        if (decimalSetting == DecimalSetting.decimalNumber)
        {
            usedDecimaTypelList.Add("decimal number");
        }
        if (decimalSetting == DecimalSetting.nonDecimalAndDecimal)
        {
            usedDecimaTypelList.Add("non decimal number");
            usedDecimaTypelList.Add("decimal number");
        }
    }
    private void SetupEnemySettingNegativity()
    {
        if (negativeNon)
        {
            usedNegativeTypeList.Add("negative non");
        }
        if (negativeA)
        {
            usedNegativeTypeList.Add("negative A");
        }
        if (negativeAB)
        {
            usedNegativeTypeList.Add("negative AB");
        }
    }

    private void SetupEnemyQuestion()   //will be call when enemy start fighting or previous question is off
    {
        EnemyQuestionRandomOperator();
        EnemyQuestionRandomAB();
    }
    private void EnemyQuestionRandomOperator()
    {
        usedOperatorForQuestion = usedOperatorList[Random.Range(0, usedOperatorList.Count)];
    }
    private void EnemyQuestionRandomAB()
    {
        if (usedOperatorForQuestion == "addition")
        {
            Addition();
            EnemyQuestionCreateDecimal();
            questionString = valueA.ToString() + " + " + valueB.ToString();
            answerString = (valueA + valueB).ToString();
            EnemyQuestionCreateNegative();
        }
        if (usedOperatorForQuestion == "subtraction")
        {
            Subtraction();
            EnemyQuestionCreateDecimal();
            questionString = valueA.ToString() + " - " + valueB.ToString();
            answerString = (valueA - valueB).ToString();
            EnemyQuestionCreateNegative();
        }
        if (usedOperatorForQuestion == "multiplication")
        {
            Multiplication();
            EnemyQuestionCreateDecimal();
            questionString = valueA.ToString() + " X " + valueB.ToString();
            answerString = (valueA * valueB).ToString();
            EnemyQuestionCreateNegative();
        }
        if (usedOperatorForQuestion == "division")
        {
            Division();
            EnemyQuestionCreateDecimal();
            questionString = valueA.ToString() + " / " + valueB.ToString();
            answerString = (valueA / valueB).ToString();
            EnemyQuestionCreateNegative();
        }
    }
    private void Addition()
    {
        do
        {
            valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
            valueB = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
        } while (valueA + valueB > maximumAnswerValue);
    }
    private void Subtraction()
    {
        do
        {
            valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
            valueB = Random.Range((int)minimumAnswerValue, (int)valueA + 1);
        } while (valueA - valueB > (int)maximumAnswerValue);
    }
    private void Multiplication()
    {
        isDisallowedAnswer = false;
        do
        {
            do
            {
                valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                valueB = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);

                if (isAllowOne == true)
                {
                    isDisallowedAnswer = CheckOneAnswer(valueA, valueB);
                }

            } while (valueA * valueB > (int)maximumAnswerValue 
            || valueA * valueB == 0);

        } while (isDisallowedAnswer == true);
    }
    private void Division()
    {
        do
        {
            isDisallowedAnswer = false;
            do
            {
                valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                valueB = Random.Range((int)minimumAnswerValue, (int)valueA + 1);

                if (isAllowOne == true)
                {
                    isDisallowedAnswer = CheckOneAnswer(valueA, valueB);
                }

            } while (valueA * valueB == 0
            || valueA % valueB != 0);

        } while (isDisallowedAnswer == true);
    }

    private bool isDisallowedAnswer;
    private bool boolTemp;
    private bool CheckOneAnswer(float A, float B)
    {
        if (usedOperatorForQuestion == "multiplication")
        {
            if (A == 1 || B == 1)
            {
                boolTemp = true;
            }
            else
            {
                boolTemp = false;
            }
        }
        if (usedOperatorForQuestion == "division")
        {
            if (((A / B) / A == 1) || A / B == 1)
            {
                boolTemp = true;
            }
            else
            {
                boolTemp = false;
            }
        }

        return boolTemp;
    }
    private void EnemyQuestionCreateDecimal()
    {
        usedDecimalType = usedDecimaTypelList[Random.Range(0, usedDecimaTypelList.Count)];
        if (usedDecimalType == "decimal number")
        {
            int decimalVariationtemp = Random.Range(0, decimalVariation + 1);
            valueA = valueA / Mathf.Pow(10, decimalPostion + decimalVariationtemp);
            valueB = valueB / Mathf.Pow(10, decimalPostion + decimalVariationtemp);
        }
    }
    private void EnemyQuestionCreateNegative()
    {
        usedNegativeType = usedNegativeTypeList[Random.Range(0, usedNegativeTypeList.Count)];
        if (usedNegativeType == "negative A")
        {
            if (usedOperatorForQuestion == "addition")
            {
                valueA = valueA * -1f;
                questionString = "(" + valueA.ToString() + ")" + " + " + valueB.ToString();
                answerString = (valueA + valueB).ToString();
            }
            if (usedOperatorForQuestion == "subtraction")
            {
                do
                {
                    valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                    valueB = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                } while (valueA - valueB > (int)maximumAnswerValue);

                valueA = valueA * -1f;
                questionString = "(" + valueA.ToString() + ")" + " - " + valueB.ToString();
                answerString = (valueA - valueB).ToString();
            }
            if (usedOperatorForQuestion == "multiplication")
            {
                valueA = valueA * -1f;
                questionString = "(" + valueA.ToString() + ")" + " X " + valueB.ToString();
                answerString = (valueA * valueB).ToString();
            }
            if (usedOperatorForQuestion == "division")
            {
                valueA = valueA * -1f;
                questionString = "(" + valueA.ToString() + ")" + " / " + valueB.ToString();
                answerString = (valueA / valueB).ToString();
            }
        }
        if (usedNegativeType == "negative AB")
        {
            if (usedOperatorForQuestion == "addition")
            {
                valueA = valueA * -1f;
                valueB = valueB * -1f;
                questionString = "(" + valueA.ToString() + ")" + " + " + "(" + valueB.ToString() + ")";
                answerString = (valueA + valueB).ToString();
            }
            if (usedOperatorForQuestion == "subtraction")
            {
                do
                {
                    valueA = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                    valueB = Random.Range((int)minimumAnswerValue, (int)maximumAnswerValue);
                } while (valueA - valueB > (int)maximumAnswerValue);
                valueA = valueA * -1f;
                valueB = valueB * -1f;
                questionString = "(" + valueA.ToString() + ")" + " - " + "(" + valueB.ToString() + ")";
                answerString = (valueA - valueB).ToString();
            }
            if (usedOperatorForQuestion == "multiplication")
            {
                valueA = valueA * -1f;
                valueB = valueB * -1f;
                questionString = "(" + valueA.ToString() + ")" + " X " + "(" + valueB.ToString() + ")";
                answerString = (valueA * valueB).ToString();
            }
            if (usedOperatorForQuestion == "division")
            {
                valueA = valueA * -1f;
                valueB = valueB * -1f;
                questionString = "(" + valueA.ToString() + ")" + " / " + "(" + valueB.ToString() + ")";
                answerString = (valueA / valueB).ToString();
            }
        }
    }

    private void EnemyQuestionCreate()
    {
        if (isEquation == true)
        {
            EnemyQuestionCreateEquation();
        }

        enemyStatus.SetupEnemyQuestion(questionString, answerString);
    }

    private void EnemyQuestionCreateEquation()
    {
        string[] equationString = questionString.Split(' ');
        int randomEquationSetting = 0;

        //type manage
        if(equationSetting == EquationSetting.xAsA)
        {
            randomEquationSetting = 0;
        }
        if (equationSetting == EquationSetting.xAsB)
        {
            randomEquationSetting = 1;
        }
        if (equationSetting == EquationSetting.xRandom)
        {
            randomEquationSetting = Random.Range(0,2);
        }

        //set the equation
        if (randomEquationSetting == 0)
        {
            questionString = $"X {equationString[1]} {equationString[2]} = {answerString}";
            answerString = valueA.ToString();
        }
        if (randomEquationSetting == 1)
        {
            questionString = $"{equationString[0]} {equationString[1]} X = {answerString}";
            answerString = valueB.ToString();
        }
    }

    private void FixedUpdate()
    {
        if (enemyStatus.isQuestionActive == false)
        {
            enemyStatus.isQuestionActive = true;
            SetupEnemyQuestion();
            EnemyQuestionCreate();
        }
    }
}
