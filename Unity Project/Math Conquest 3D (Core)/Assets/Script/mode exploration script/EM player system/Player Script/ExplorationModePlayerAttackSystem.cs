using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplorationModePlayerAttackSystem : MonoBehaviour
{
    [Header("Player Movement")]
    public ExplorationModePlayerControllerMovement PlayerMovement;

    [Header("Player Attack System")]
    public float playerAttackDamage;

    [Header("Player Shockwave")]
    public GameObject playerAttackShockwavePrefab;
    private GameObject playerAttackShockwaveObject;
    public Vector3 playerShockwaveStartPosition;
    public float playerShockwaveStartDistanceX;
    public float playerShockwaveStartDistanceY;

    [Header("Player UI")]
    public TMP_InputField playerAnswerInputField;
    public TMP_Text playerAnswerText;
    public Animator playerAnswerAnimator;
    public AnimationClip playerAnswerTextAnimationType;
    public AnimationClip playerAnswerTextAnimationClear;

    [Header("Auto Add Target System")]
    public List<Transform> enemyList = new List<Transform>();
    public Transform enemyCurrentSelected;

    [Header("Switch Target System")]
    public int enemyCurrentSelectedIndex;

    [HideInInspector] public MasterInput playerInput;

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
        if(playerAnswerInputField.text == "") { return; }
        if (enemyCurrentSelected != null && PlayerMovement.canControlCharacter == true)
        {
            PlayerLaunchingShockWave();
            PlayerMovement.PlayerAttackPerform();
            PlayerMovement.PlayerCheckFacingTarget(enemyCurrentSelected);
        }
        PlayerClearAnswer();
    }
    private void PlayerLaunchingShockWave()
    {
        PlayerCheckShockwaveStartPoint();
        playerAttackShockwaveObject = Instantiate(playerAttackShockwavePrefab, 
            playerShockwaveStartPosition,
            playerAttackShockwavePrefab.transform.rotation);

        // Assign shockwave property
        ExplorationModePlayerAttackShockwave PlayerShockwave = playerAttackShockwaveObject.GetComponent<ExplorationModePlayerAttackShockwave>();
        PlayerShockwave.enemyTargetTransform = enemyCurrentSelected;
        PlayerShockwave.shockwaveAnswer = playerAnswerText.text.ToString();
        PlayerShockwave.shockwaveDamage = playerAttackDamage;
    }
    private void PlayerCheckShockwaveStartPoint()
    {
        if (enemyCurrentSelected.position.x > transform.position.x)
        {
            playerShockwaveStartPosition = new Vector3(
                transform.position.x + playerShockwaveStartDistanceX,
                transform.position.y + playerShockwaveStartDistanceY,
                transform.position.z);
        }
        else if (enemyCurrentSelected.position.x < transform.position.x)
        {
            playerShockwaveStartPosition = new Vector3(
                transform.position.x - playerShockwaveStartDistanceX,
                transform.position.y + playerShockwaveStartDistanceY,
                transform.position.z);
        }
    }
    public void PlayerTypeAnswerAnimation()
    {
        playerAnswerAnimator.Play(playerAnswerTextAnimationType.name);
    }
    public void PlayerClearAnswer()
    {
        playerAnswerInputField.text = "";
        playerAnswerText.text = "";
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + playerShockwaveStartDistanceX,
                                            transform.position.y + playerShockwaveStartDistanceY,
                                            transform.position.z),
                                            0.1f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - playerShockwaveStartDistanceX,
                                            transform.position.y + playerShockwaveStartDistanceY,
                                            transform.position.z),
                                            0.1f);
    }

    // target system
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            if (enemyList.Count == 0)
            {
                TargetAddBlankList(enemy.transform);
            }
            else if (enemyList.Count > 0)
            {
                TargetAdd(enemy.transform);
            }
        }
    }
    private void OnTriggerExit(Collider enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            TargetRemove(enemy.transform);
        }
        if (enemyList.Count == 0)
        {
            enemyCurrentSelected = null;
        }
    }

    private void TargetAddBlankList(Transform enemyAdded)
    {
        enemyList.Add(enemyAdded);
        enemyCurrentSelected = enemyList[0];
        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
    }
    private void TargetAdd(Transform enemyAdded)
    {
        enemyList.Add(enemyAdded);
    }
    private void TargetRemove(Transform enemyRemoved)
    {
        enemyList.Remove(enemyRemoved.transform);

        if (enemyCurrentSelected == enemyRemoved && enemyList.Count > 0)
        {
            enemyCurrentSelected = enemyList[0];
            enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
        }
        if (enemyRemoved != null)
        {
            if(enemyRemoved.TryGetComponent<EnemyControllerStatus>(out EnemyControllerStatus enemyStatus))
            {
                enemyStatus.GetComponent<EnemyControllerStatus>().EnemyDeselected();
            }
        }
    }

    private void TargetSwitch()
    {
        if(enemyList.Count == 0 || enemyList.Count == 1) { return; }

        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemyDeselected();
        enemyCurrentSelectedIndex++;
        if (enemyCurrentSelectedIndex + 1 > enemyList.Count)
        {
            enemyCurrentSelectedIndex = 0;
        }

        enemyCurrentSelected = enemyList[enemyCurrentSelectedIndex];
        enemyCurrentSelected.GetComponent<EnemyControllerStatus>().EnemySelected();
    }
}