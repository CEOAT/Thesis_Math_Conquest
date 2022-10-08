using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationModeGameController : MonoBehaviour
{
    // control the game when pause, restart the game when game over

    public GameObject GameOverWindowGroup;

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

    private void GameOver()
    {
        print("game over");
        GameOverWindowGroup.SetActive(true);
    }

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
