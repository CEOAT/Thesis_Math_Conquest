using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

// control enemy status (health, animation)

public class EnemyControllerStatus : MonoBehaviour
{
    [Header("Question of Enemy")]
    public string questionAnswer;
    public bool isQuestionActive = false;

    [Header("Enemy UI")]
    public GameObject enemyDetailPrefab;
    private GameObject enemyDetailObject;
    public GameObject enemyTrackPoint;
    [SerializeField] private float enemyUiPointHeight = 1.5f;
    [SerializeField] private float enemyUiPointZ = 0f;

    private TMP_Text enemyNameText;
    private Transform enemyHealthBarObject;
    private float enemyHealthBarScale;
    private TMP_Text enemyQuestionText;

    public GameObject enemySelectorPrefab;
    private GameObject enemySelectorObject;

    public bool isEnemySelectedUI;
    [Range(0,1f)] public float UIUnselectedAlpha;
    private Color HealthBarAlphaTopFull, HealthBarAlphaTopLow;
    private Color HealthBarAlphaBottomFull, HealthBarAlphaBottomLow;

    [Header("Enemy Detail")]
    public string enemyName;
    
    [Header("Enemy Health")]
    public float enemyHealthMax;
    public float enemyHealthCurrent;
    public float enemyTimeToSelfDestroy = 2f;
    private bool isEnemyDeadCheck = false;

    [Header("Enemy Damage")]
    public float enemyAttackDamage;

    [Header("Enemy Studiness")]
    [SerializeField] private bool isEnemySturdyFromAttack;

    [Header("Enemy State (Display Only)")]
    public string enemyState;
    public bool isEnemyTakenDamage;

    private EnemyControllerMovement EnemyMovement;

    

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
        EnemySelectedUI();
    }
    private void SetupEnemyComponent()
    {
        enemyDetailObject = Instantiate(enemyDetailPrefab, transform.position, transform.rotation);
        EnemyStatusObject enemyStatusObject = enemyDetailObject.GetComponent<EnemyStatusObject>();
        enemyStatusObject.enemyOwnDetailObject = this.transform;
        enemyStatusObject.enemyUiPointHeight = enemyUiPointHeight;
        enemyStatusObject.enemyUiPointZ = enemyUiPointZ;

        enemyNameText = enemyDetailObject.transform.GetChild(0).GetComponent<TMP_Text>();
        enemyHealthBarObject = enemyDetailObject.transform.GetChild(1).transform;
        enemyQuestionText = enemyDetailObject.transform.GetChild(3).GetComponent<TMP_Text>();

        HealthBarAlphaTopFull = enemyDetailObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color;       //start color + alpha
        HealthBarAlphaTopLow = HealthBarAlphaTopFull;                                                                 //low opacity alpha
        HealthBarAlphaTopLow.a = UIUnselectedAlpha;

        HealthBarAlphaBottomFull = enemyDetailObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color;
        HealthBarAlphaBottomLow = HealthBarAlphaBottomFull;
        HealthBarAlphaBottomLow.a = UIUnselectedAlpha;

        EnemyMovement = GetComponent<EnemyControllerMovement>();
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
        EnemySelectedUI();
    }
    private void EnemyHealthCheckDestroy()
    {
        if (enemyHealthCurrent <= 0 && isEnemyDeadCheck == false)
        {
            isEnemyDeadCheck = true;
            EnemyMovement.EnemyDead();
        }
    }
    private void EnemyHealthBarChangeScale()
    {
        enemyHealthBarObject.localScale = new Vector3((enemyHealthCurrent / enemyHealthMax) * enemyHealthBarScale,
            enemyHealthBarObject.localScale.y,
            enemyHealthBarObject.localScale.z);
    }
    private void EnemySelectedUI()
    {
        if (isEnemySelectedUI == true)
        {
            enemyDetailObject.transform.GetChild(0).GetComponent<TMP_Text>().alpha = 1f;
            enemyDetailObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = HealthBarAlphaTopFull;
            enemyDetailObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = HealthBarAlphaBottomFull;
            enemyDetailObject.transform.GetChild(3).GetComponent<TMP_Text>().alpha = 1f;
            enemyDetailObject.transform.GetChild(4).GetComponent<SpriteRenderer>().color = HealthBarAlphaTopFull;
        }
        else
        {
            enemyDetailObject.transform.GetChild(0).GetComponent<TMP_Text>().alpha = UIUnselectedAlpha;
            enemyDetailObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = HealthBarAlphaTopLow;
            enemyDetailObject.transform.GetChild(2).GetComponent<SpriteRenderer>().color = HealthBarAlphaBottomLow;
            enemyDetailObject.transform.GetChild(3).GetComponent<TMP_Text>().alpha = UIUnselectedAlpha;
            enemyDetailObject.transform.GetChild(4).GetComponent<SpriteRenderer>().color = HealthBarAlphaBottomLow;
        }
    }

    #region Select Enemy Function
    public void EnemySelected()
    {
        isEnemySelectedUI = true;

        enemySelectorObject = Instantiate(enemySelectorPrefab, transform.position + new Vector3(0,0,-0.35f), enemySelectorPrefab.transform.rotation);
        enemySelectorObject.GetComponent<EnemySelectorObject>().selectorTrackPoint = enemyTrackPoint.transform;
    }
    public void EnemyDeselected()
    {
        if (this.gameObject != null)
        {
            isEnemySelectedUI = false;
            if(enemySelectorObject != null) { Destroy(enemySelectorObject.gameObject);}
        }
    }
    #endregion

    #region Enemy Check Answer
    public void CheckPlayerAnswer(string playerAnswer, float playerDamage)
    {
        if (playerAnswer == questionAnswer)
        {
            PlayerAnswerCorrect();
            EnemyTakenDamage(playerDamage);
            enemyDetailObject.GetComponent<Animator>().SetTrigger("triggerUiWorldSpaceShake");
        }
        else
        {
            PlayerAnswerFalse();
        }
    }
    private void PlayerAnswerCorrect()
    {
        isQuestionActive = false;
    }
    public void EnemyTakenDamage(float receivedDamage)
    {
        enemyHealthCurrent -= receivedDamage;
        isEnemyTakenDamage = true;

        if(!isEnemySturdyFromAttack)
        {
            EnemyMovement.EnemyHurtRecovery();
        }
    }
    private void PlayerAnswerFalse()
    {
        FalseReactionHeal();
    }
    #endregion

    #region False Reaction
    private void FalseReactionHeal()
    {
        if (enemyHealthCurrent + 10 > enemyHealthMax)
        {
            enemyHealthCurrent = enemyHealthMax;
        }
        else
        {
            enemyHealthCurrent += 10;
        }
    }
    #endregion

    [Button] private void EnemyDead()
    {
        enemyHealthCurrent = 0;
    }
}