using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EliminationModePlayerControllerInputAnswer : MonoBehaviour
{
    public float playerAnswerNumber;

    public GameObject playerSelectionMarkPrefab;
    public GameObject playerSelectionMarkObject;

    [SerializeField] private GameObject[] enemyObjectArray;
    [SerializeField] private int enemySelectedIndex;
    [SerializeField] private GameObject enemySelectedObject;

    public TMP_InputField inputField;

    private MasterInput playerInput;
    // private PlayerControllerStatus PlayerStatus;
    private EliminationModeEventController EventController;

    private void Awake()
    {
        SetupComponent();
        SetupControl();
    }
    private void SetupComponent()
    {
        // PlayerStatus = GetComponent<PlayerControllerStatus>();
        EventController = GameObject.FindGameObjectWithTag("Event System").GetComponent<EliminationModeEventController>();
    }
    private void SetupControl()
    {
        playerInput = new MasterInput();
        playerInput.PlayerControlElimination.EnterAnswer.performed += context => PlayerAnswerEnter();
        playerInput.PlayerControlElimination.ClearAnswer.performed += context => PlayerAnswerClear();
        playerInput.PlayerControlElimination.SwitchEnemy.performed += context => PlayerSwitchEnemy();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    // *** button method ***
    // press Enter
    private void PlayerAnswerEnter()
    {
        if(inputField.text == "") { return; }

        playerAnswerNumber = float.Parse(inputField.text);
        inputField.text = "";
        //enemyObjectArray[enemySelectedIndex].GetComponent<EnemyControllerStatus>().CheckPlayerAnswer(playerAnswerNumber.ToString(), PlayerStatus.playerAttackDamage);
    }
    // press Space
    private void PlayerAnswerClear()
    {
        inputField.text = "";
    }
    // press S
    private void PlayerSwitchEnemy()
    {
        PlayerChangeSelectionMarkTarget();
        PlayerCreateSelectionMark();
    }
    private void PlayerChangeSelectionMarkTarget()
    {
        enemySelectedIndex++;
        if (enemySelectedIndex >= enemyObjectArray.Length)
        {
            enemySelectedIndex = 0;
        }
        if (playerSelectionMarkObject != null)
        {
            Destroy(playerSelectionMarkObject.gameObject);
        }
        enemySelectedObject = enemyObjectArray[enemySelectedIndex];
    }
    private void PlayerCreateSelectionMark()
    {
        playerSelectionMarkObject = Instantiate(playerSelectionMarkPrefab);
        if (playerSelectionMarkObject != null)
        {
            playerSelectionMarkObject.GetComponent<EliminationModePlayerSelectionMark>().enemyTransform = enemyObjectArray[enemySelectedIndex].transform;
        }
    }
    private void Start()
    {
        InvokeRepeating("EnemyAddToArray", 0.01f, 0.1f);
        Invoke("PlayerSwitchEnemy", 0.15f);
    }

    // *** method when encounter enemy wave start ***
    private void EnemyAddToArray()
    {
        enemyObjectArray = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemyObjectArray.Length == 0) { return; }

        if (playerSelectionMarkObject != null && enemyObjectArray.Length == 0)
        {
            Destroy(playerSelectionMarkObject.gameObject);
        }

        if (enemySelectedObject == null && enemyObjectArray.Length != 0)
        {
            PlayerSwitchEnemy();
        }
    }

    private void FixedUpdate()
    {
        inputField.ActivateInputField();
        if (enemyObjectArray.Length == 0 && playerSelectionMarkObject != null)
        {
            Destroy(playerSelectionMarkObject.gameObject);
        }
    }
}