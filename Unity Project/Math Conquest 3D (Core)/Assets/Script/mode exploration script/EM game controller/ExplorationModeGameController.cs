using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationModeGameController : MonoBehaviour
{
    // control the game when pause, restart the game when game over

    public GameObject GameOverWindowGroup;
    public GameObject GameplayUiGroup;
    public ExplorationModePlayerControllerMovement PlayerMovement;
    public ExplorationModePlayerHealth PlayerHealth;

    private void Awake()
    {
        SetupObject();
    }
    private void SetupObject()
    {
        GameOverWindowGroup.SetActive(false);
    }

    private void OnEnable()
    {
        ExplorationModePlayerHealth.playerDead += GameOver;
    }
    private void OnDisable()
    {
        ExplorationModePlayerHealth.playerDead -= GameOver;
    }

    // disable action method
    public void TriggerCutscene()
    {
        PlayerMovement.PlayerWait();
        PlayerHealth.canPlayerTakeDamage = false;
        GameplayUiGroup.SetActive(false);   // *** need animated UI method ***
    }

    // resume action method
    public void AllowMovement()
    {
        PlayerMovement.PlayerEnableddMovement();
        PlayerHealth.canPlayerTakeDamage = true;
        GameplayUiGroup.SetActive(true);
    }

    // Game Over Function
    private void GameOver()
    {
        GameOverWindowGroup.SetActive(true);
        GameplayUiGroup.SetActive(false);
    }

    // Game Over UI
    public void GameOverWindowLoadCheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOverWindowReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOverWindowQuitGame()
    {
        Application.Quit();
    }
}