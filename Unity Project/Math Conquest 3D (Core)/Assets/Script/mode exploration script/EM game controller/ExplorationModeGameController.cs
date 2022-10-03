using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationModeGameController : MonoBehaviour
{
    // control the game when pause, restart the game when game over

    private void OnEnable()
    {
        ExplorationModePlayerHealth.playerDead += GameOver;
    }
    private void OnDisable()
    {
        ExplorationModePlayerHealth.playerDead -= GameOver;
    }

    public void GameOver()
    {
        print("game over");
    }
}
