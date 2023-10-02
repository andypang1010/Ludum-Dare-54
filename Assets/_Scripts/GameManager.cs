using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
        }
    }


    public GameState GetGameStates()
    {
        return state;
    }

    public void GoMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
        state = GameState.MENU;
        print("In menu");
        SceneManager.LoadScene("Menu");
    }

    public void GoGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        state = GameState.IN_GAME;
        print("In game");
        SceneManager.LoadScene("In-Game");
    }

    public void GoLost()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        print("In lost");
        state = GameState.LOST;
    }
}

public enum GameState
{
    MENU,
    IN_GAME,
    LOST
}
