using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    // TODO: Adjust max scale to be the size when player hits borders
    public float maxScale = 10f;
    public GameStates state;
    public float survivalTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerMain");
        state = GameStates.IN_GAME;
        survivalTime = 0;
    }

    void Update()
    {
        switch(state) {
            case GameStates.IN_GAME:
                //Cursor.visible = false;

                if (player.transform.localScale.x >= maxScale) {
                    state = GameStates.LOST;
                }
                else {
                    survivalTime = Time.time;
                }
                break;

            case GameStates.LOST:
                Cursor.visible = true;
                print("You've survived for: " + survivalTime + "seconds");
                break;
        }
    }
}

public enum GameStates {
    IN_GAME,
    LOST
}