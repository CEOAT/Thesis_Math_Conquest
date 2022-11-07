using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class ExplorationModePlayerAttackSystem : MonoBehaviour
{
    [Header("Player Attack System")]
    public float playerAttackDamage;
    public TMP_InputField playerAnswerInputField;
    public TMP_Text playerAnswerText;
    public ExplorationModePlayerControllerMovement PlayerController;

    [Header("Auto Add Target System")]
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject enemyCurrentSelected;

    [Header("Switch Target System")]
    public int enemyCurrentSelectedIndex;

    private MasterInput playerInput;

    private void Awake()
    {
        SetupControl();
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlExploration.Attack.performed += context => PlayerAttack();
        playerInput.PlayerControlExploration.SwitchTarget.performed += context => TargetSwitch();
        playerInput.PlayerControlExploration.ClearAnswer.performed += context => PlayerClearAnswer();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        SetupObject();
    }
    private void SetupObject()
    {
        playerAnswerInputField.text = "";
        playerAnswerText.text = "";
    }

    private void Update()
    {
        ActiveInputField();
    }
    private void ActiveInputField()
    {
        playerAnswerInputField.ActivateInputField();
        playerAnswerText.text = playerAnswerInputField.text;
    }

    private void PlayerAttack()
    {
        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(playerAnswerText.text.ToString(), playerAttackDamage);
        playerAnswerInputField.text = "";
    }
    private void PlayerClearAnswer()
    {
        playerAnswerInputField.text = "";
    }

    // target system
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            if (enemyList.Count == 0)
            {
                TargetAddBlankList(enemy.gameObject);
            }
            else if (enemyList.Count > 0)
            {
                TargetAdd(enemy.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            TargetRemove(enemy.gameObject);
            CheckEnemyInList();
            }
    }

    private void TargetAddBlankList(GameObject enemyAdded)
    {
        enemyList.Add(enemyAdded);
        enemyCurrentSelected = enemyList[0];
        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
    }
    private void TargetAdd(GameObject enemyAdded)
    {
        enemyList.Add(enemyAdded);
    }
    private void TargetRemove(GameObject enemyRemoved)
    {
        enemyList.Remove(enemyRemoved.gameObject);

        if (enemyCurrentSelected == enemyRemoved && enemyList.Count > 0)
        {
            enemyCurrentSelected = enemyList[0];
            enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
        }
        if (enemyRemoved != null)
        {
            enemyRemoved.GetComponent<EnemyControllerStatus>().EnemyDeselected();
        }
    }

    private void TargetSwitch()
    {
        if(enemyList.Count == 0 && enemyList.Count == 1) { return; }

        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemyDeselected();

        enemyCurrentSelectedIndex++;
        if (enemyCurrentSelectedIndex + 1 > enemyList.Count)
        {
            enemyCurrentSelectedIndex = 0;
        }

        enemyCurrentSelected = enemyList[enemyCurrentSelectedIndex];
        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
    }

    private void CheckEnemyInList()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
            {
                print("found null");
                enemyList.Remove(enemy);
            }
        }
    }
}