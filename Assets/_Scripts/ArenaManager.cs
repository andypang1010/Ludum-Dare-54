using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public GameObject prevArena;
    public GameObject arena;
    Vector2 newPosition;
    Quaternion newQuaternion = new Quaternion(0, 0, 0, 0);
    private void Start() {
        
    }

    private void Update() {
        
    }

    public void NewArena() {
        Directions dir = (Directions)Random.Range(0, 3);
        switch(dir) {
            case Directions.UP:
                break;
            case Directions.DOWN:
                break;
            case Directions.LEFT:
                break;
            case Directions.RIGHT:
                break;            
        }
        print("New arena at direction = " + dir.ToString());

        // GameObject newArena = Instantiate(arena, newPos, newQuaternion);
        // newArena.


    }
}

public enum Directions {
    UP,
    DOWN,
    LEFT,
    RIGHT
}