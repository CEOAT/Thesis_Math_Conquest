using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationModeGameController : MonoBehaviour
{
    // control the game when pause, restart the game when game over

    [Header("UI Object")]
    public GameObject GamePauseWindowGroup;
    public GameObject GameOverWindowGroup;
    public GameObject GameplayUiGroup;
    public GameObject ObjectiveText;

    [Header("Game Over Object")]
    public GameObject WorldSpaceBlackScreenCube;
    public GameObject ImageFadeBlackPrefab;
    public Transform canvasTransform;

    [Header("Player")]
    public Transform playerGameObject;
    public ExplorationModePlayerControllerMovement PlayerMovement;
    public ExplorationModePlayerHealth PlayerHealth;
    public ExplorationModePlayerAttackSystem PlayerAttackSystem;

    [Header("Game System Script")]
    public SaveController SaveController;
    public StageControllerCheckPointManager CheckpointManager;
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
        PlayerInput.PlayerControlGeneral.PauseGame.performed += context => PauseGame();
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
        GameplayUiGroup.SetActive(false);
    }
    public void AllowMovement()
    {
        PlayerMovement.PlayerEnabledMovement();
        GameplayUiGroup.SetActive(true);
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
    private void PauseGame()
    {
        if (isPauseGameDiabled == true) { return; }

        if (GamePauseWindowGroup.activeSelf == false)
        {
            Time.timeScale = 0;
            GameplayUiGroup.SetActive(false);
            GamePauseWindowGroup.SetActive(true);
        }
        else if (GamePauseWindowGroup.activeSelf == true)
        {
            Time.timeScale = 1;
            GameplayUiGroup.SetActive(true);
            GamePauseWindowGroup.SetActive(false);
        }
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
    }

    // In-Game Menu UI
    public void MenuResumeGame()
    {
        GamePauseWindowGroup.SetActive(false);
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