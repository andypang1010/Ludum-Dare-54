using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int frameRate = 60;
    public GameState state;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Application.targetFrameRate = frameRate;
            Cursor.visible = false;
        }
    }


    public GameState GetGameStates()
    {
        return state;
    }

    public void GoMenu()
    {
        Time.timeScale = 1f;
        state = GameState.MENU;
        SceneManager.LoadScene("Menu");
    }

    public void GoGame()
    {
        Time.timeScale = 1f;
        state = GameState.IN_GAME;
        SceneManager.LoadScene("In-Game");
    }

    public void GoLost()
    {
        Time.timeScale = 0f;
        state = GameState.LOST;
    }

    public void GoTutorial() {
        Time.timeScale = 1f;
        state = GameState.TUTORIAL;
        SceneManager.LoadScene("Tutorial");
    }
}

public enum GameState
{
    MENU,
    IN_GAME,
    LOST,
    TUTORIAL
}
