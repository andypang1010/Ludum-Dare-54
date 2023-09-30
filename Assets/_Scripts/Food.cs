using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool passedCenter = false;
    private float speed;
    private Vector2 dir;
    private float arenaRadius = 3f;
    private float screenRadius = 7f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = dir * speed * Time.deltaTime;
        transform.position = transform.position + moveVector;

        if (transform.position.magnitude < arenaRadius)
            passedCenter = true;
        if (passedCenter && transform.position.magnitude > screenRadius)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, arenaRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, screenRadius);
    }

    public void Throw(Vector2 spawnPos, float speed)
    {
        this.speed = speed;
        transform.position = spawnPos;
        Vector2 perpendicularDir = Vector2.Perpendicular(spawnPos).normalized;
        Vector2 targetPos = perpendicularDir * arenaRadius * Random.Range(-1f, 1f);

        dir = (targetPos - spawnPos).normalized;
    }
}
