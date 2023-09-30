using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float speed;
    private Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = dir * speed * Time.deltaTime;
        transform.position = transform.position + moveVector;
    }

    public void SetSpeedAndDir(float speed, Vector2 dir) {
        this.speed = speed;
        this.dir = dir;
    }
}
