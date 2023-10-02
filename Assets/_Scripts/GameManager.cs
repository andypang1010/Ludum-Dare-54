using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    // public static int frameRate = 60;
    private static GameState state;

    private static void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public static GameState GetGameStates()
    {
        return state;
    }

    public static void GoMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
        state = GameState.MENU;
        SceneManager.LoadScene("Menu");
    }

    public static void GoGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        state = GameState.IN_GAME;
        SceneManager.LoadScene("In-Game");
    }

    public static void GoLost()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        state = GameState.LOST;
    }
}

public enum GameState
{
    MENU,
    IN_GAME,
    LOST
}
