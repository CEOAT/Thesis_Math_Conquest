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

    [Header("Player")]
    public Transform playerGameObject;
    public ExplorationModePlayerControllerMovement PlayerMovement;
    public ExplorationModePlayerHealth PlayerHealth;

    [Header("Game System Script")]
    public SaveController SaveController;
    public StageControllerCheckPointManager CheckpointManager;
    private MasterInput PlayerInput;

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
        PlayerMovement.PlayerEnableddMovement();
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
    private void PauseGame()
    {
        if (GamePauseWindowGroup.activeSelf == false)
        {
            Time.timeScale = 0;
            GamePauseWindowGroup.SetActive(true);
        }
        else if (GamePauseWindowGroup.activeSelf == true)
        {
            Time.timeScale = 1;
            GamePauseWindowGroup.SetActive(false);
        }
    }

    // Game Over Function
    private void GameOver()
    {
        TriggerCutscene();
        GamePauseWindowGroup.SetActive(false);
        GameplayUiGroup.SetActive(false);
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