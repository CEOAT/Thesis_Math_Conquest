using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// control enemy status (health, animation)

public class EnemyControllerStatus : MonoBehaviour
{
    [Header("Question of Enemy")]
    public string questionAnswer;
    public bool isQuestionActive = false;

    [Header("Enemy UI On Head")]
    public GameObject enemyDetailPrefab;
    private GameObject enemyDetailObject;

    private TMP_Text enemyNameText;
    private Transform enemyHealthBarObject;
    private float enemyHealthBarScale;
    private TMP_Text enemyQuestionText;

    [Header("Enemy Detail")]
    public string enemyName;
    
    [Header("Enemy Health")]
    public float enemyHealthMax;
    public float enemyHealthCurrent;

    [Header("Enemy Damage")]
    public float enemyAttackDamage;

    [Header("Enemy State")]
    public string enemyState;
    public bool isEnemyTakenDamage;

    public void CheckPlayerAnswer(string playerAnswer, float playerDamage)
    {
        if (playerAnswer == questionAnswer)
        {
            print("hit");
            PlayerAnswerCorrect(playerDamage);

            
            //random new question
            //enemy lose health
        }
        else
        {
            print("wrong");


            //wrong statement
            //reduce player hp, restore enemy's health?, gain shield?
        }
    }
    private void PlayerAnswerCorrect(float receivedDamage)
    {
        enemyHealthCurrent -= receivedDamage;
        isQuestionActive = false;
        isEnemyTakenDamage = true;
    }


    private void Awake()
    {
        SetupEnemyStatusAwake();
    }
    private void SetupEnemyStatusAwake()
    {
        enemyHealthCurrent = enemyHealthMax;
        enemyState = "idle";
        isEnemyTakenDamage = false; 
    }


    private void Start()
    {
        SetupEnemyComponent();
        SetupEnemyStatus(enemyName);
    }
    private void SetupEnemyComponent()
    {
        enemyDetailObject = Instantiate(enemyDetailPrefab, transform.position, transform.rotation);
        enemyDetailObject.GetComponent<EnemyDetailObject>().enemyOwnDetailObject = this.transform;
        enemyNameText = enemyDetailObject.transform.GetChild(0).GetComponent<TMP_Text>();
        enemyHealthBarObject = enemyDetailObject.transform.GetChild(1).transform;
        enemyQuestionText = enemyDetailObject.transform.GetChild(2).GetComponent<TMP_Text>();
    }
    private void SetupEnemyStatus(string enemyName)
    {
        enemyNameText.text = enemyName;
        enemyHealthBarScale = enemyHealthBarObject.localScale.x;
    }

    public void SetupEnemyQuestion(string enemyQuestion, string enemyAnswer)
    {
        enemyQuestionText.text = enemyQuestion;
        questionAnswer = enemyAnswer;
    }


    private void FixedUpdate()
    {
        EnemyHealthCheckDestroy();
        EnemyHealthBarChangeScale();
    }
    private void EnemyHealthCheckDestroy()
    {
        if (enemyHealthCurrent <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void EnemyHealthBarChangeScale()
    {
        enemyHealthBarObject.localScale = new Vector3((enemyHealthCurrent / enemyHealthMax) * enemyHealthBarScale,
            enemyHealthBarObject.localScale.y,
            enemyHealthBarObject.localScale.z);
    }
}