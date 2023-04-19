using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationModeGameController : MonoBehaviour
{
    // control the game when pause, restart the game when game over

    [Header("UI Object")]
    [SerializeField] public GameObject GamePauseWindowGroup;
    [SerializeField] public GameObject GameOverWindowGroup;
    [SerializeField] public GameObject GameplayUiGroup;
    [SerializeField] public GameObject ObjectiveText;
    [SerializeField] public Animator CutsceneBlackBar;

    [Header("Game Over Object")]
    [SerializeField] public GameObject WorldSpaceBlackScreenCube;
    [SerializeField] public GameObject ImageFadeBlackPrefab;
    [SerializeField] public Transform canvasTransform;

    [Header("Player")]
    [SerializeField] public Transform playerGameObject;
    [SerializeField] public ExplorationModePlayerControllerMovement PlayerMovement;
    [SerializeField] public ExplorationModePlayerHealth PlayerHealth;
    [SerializeField] public ExplorationModePlayerAttackSystem PlayerAttackSystem;

    [Header("Game System Script")]
    [SerializeField] public SaveController SaveController;
    [SerializeField] public StageControllerCheckPointManager CheckpointManager;
    private MasterInput PlayerInput;

    [Header("Menu Active Checking")]
    public bool isPauseGameDiabled;

    private void Awake()
    {
        SetupObject();
        SetupControl();
    }
    private void SetupObject()
    {
        GamePauseWindowGroup.SetActive(false);
        GameOverWindowGroup.SetActive(false);
    }
    private void SetupControl()
    {
        PlayerInput = new MasterInput();
        PlayerInput.PlayerControlGeneral.PauseGame.performed += context => CheckPauseGame();
    }

    private void Start()
    {
        MovePlayerToCheckpoint();
    }
    private void MovePlayerToCheckpoint()
    {
        Time.timeScale = 1;
        playerGameObject.position = CheckpointManager.MoveToCheckpoint().position;
    }

    private void OnEnable()
    {
        PlayerInput.Enable();
        ExplorationModePlayerHealth.playerDead += GameOver;
    }
    private void OnDisable()
    {
        PlayerInput.Disable();
        ExplorationModePlayerHealth.playerDead -= GameOver;
    }

    #region disable/enable action method
    public void TriggerCutscene()
    {
        PlayerMovement.PlayerWait();
        PlayerHealth.EnableInvincibleDuringCutscene();
        PlayerAttackSystem.PlayerClearAnswer();
        PlayerAttackSystem.playerInput.Enable();
        PlayerInput.Disable();
        GameplayUiGroup.SetActive(false);
        CutsceneBlackBar.SetBool("isBlackBarMoveIn", true);
        playerGameObject.gameObject.layer = LayerMask.NameToLayer("Default");
        // playerGameObject.GetComponent<Rigidbody>().useGravity = false;
        // playerGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void TriggerHideUi()
    {
        PlayerMovement.PlayerWait();
        GameplayUiGroup.SetActive(false);
    }
    public void AllowMovement()
    {
        PlayerMovement.PlayerEnabledMovement();
        PlayerHealth.DisableInvincibleAfterCutscene();
        PlayerAttackSystem.playerInput.Enable();
        PlayerInput.Enable();
        GameplayUiGroup.SetActive(true);
        CutsceneBlackBar.SetBool("isBlackBarMoveIn", false);
        playerGameObject.gameObject.layer = LayerMask.NameToLayer("Player");
        // playerGameObject.GetComponent<Rigidbody>().useGravity = true;
        // playerGameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
    }
    #endregion

    #region Objective System
    public void SetObjective(string objective)
    {
        ObjectiveText.GetComponent<TMPro.TMP_Text>().text = objective;
    }
    #endregion

    #region Pause/Game Over Function
    // Pause Game Button
    public void DisablePauseGame()
    {
        isPauseGameDiabled = true;
    }
    public void EnablePauseGame()
    {
        isPauseGameDiabled = false;
    }
    private void CheckPauseGame()
    {
        if (isPauseGameDiabled == true) { return; }

        if (GamePauseWindowGroup.activeSelf == false)
        {
            PauseGame();
        }
        else if (GamePauseWindowGroup.activeSelf == true)
        {
            ResumeGame();
        }
    }
    private void PauseGame()
    {
        StartCoroutine(PauseGameSequence());
    }
    private IEnumerator PauseGameSequence()
    {
        TriggerCutscene();
        PlayerInput.Disable();
        GamePauseWindowGroup.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        PlayerInput.Enable();
        Time.timeScale = 0;
    }
    private void ResumeGame()
    {
        AllowMovement();
        Time.timeScale = 1;
        GamePauseWindowGroup.SetActive(false);
    }
    

    // Game Over Function
    private void GameOver()
    {
        TriggerCutscene();
        DisablePauseGame();
        StartCoroutine(GameOverCutscene());
    }
    private IEnumerator GameOverCutscene()
    {
        Time.timeScale = 0.2f;

        PlayerAttackSystem.PlayerClearAnswer();
        PlayerMovement.PlayerDead();
        
        GamePauseWindowGroup.SetActive(false);
        GameplayUiGroup.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        Time.timeScale = 1f;
        Transform transitionImageObject = Instantiate(ImageFadeBlackPrefab.transform);
        transitionImageObject.SetParent(canvasTransform);
        transitionImageObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        transitionImageObject.SetAsFirstSibling();

        yield return new WaitForSeconds(3f);
        GameOverWindowGroup.SetActive(true);
        playerGameObject.transform.position += Vector3.up * 500;
    }

    // In-Game Menu UI
    public void MenuResumeGame()
    {
        CheckPauseGame();
    }
    public void MenuLoadCheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuRestartStage()
    {
        SaveController.RestartCheckpoint();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuReturnToStageSelection()
    {
        SceneManager.LoadScene(0);
    }
    public void MenuReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void MenuQuitGame()
    {
        Application.Quit();
    }
    #endregion
}