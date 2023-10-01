using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Edibles : MonoBehaviour
{
    public float arenaRadius = 3f;
    public float screenRadius = 7f;
    public int bounceCount = 1;
    private List<Collider2D> colliders;
    private bool passedCenter = false;
    private float speed;
    private Vector2 dir;
    private float screenHeightWorld;
    private float screenWidthWorld;

    // Start is called before the first frame update
    void Start()
    {
        //colliders = new List<Collider2D>();
        //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Wall"))
        //{
        //    if (obj.TryGetComponent(out Collider2D collider))
        //    {
        //        colliders.Add(collider);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = dir * speed * Time.deltaTime;
        //transform.position = transform.position + moveVector;

        if (transform.position.magnitude < arenaRadius)
            passedCenter = true;
        if (passedCenter && transform.position.magnitude > screenRadius)
        {
            Destroy(gameObject);
        }

        //foreach(Collider2D collider in colliders)
        //{
        //    if(GetComponent<Collider2D>().IsTouching(collider))
        //    {
        //        Debug.Log(collider.name);
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger: " + collision.gameObject.name);
        if (passedCenter && bounceCount > 0)
        {
            if (collision.gameObject.name.StartsWith("Left") || collision.gameObject.name.StartsWith("Right"))
            {
                dir.x *= -1;
                bounceCount--;
            }
            if (collision.gameObject.name.StartsWith("Top") || collision.gameObject.name.StartsWith("Bottom"))
            {
                dir.y *= -1;
                bounceCount--;
            }
        }
    }

    void UpdateScreenDimensions()
    {
        float aspect = (float)Screen.width / Screen.height;
        screenHeightWorld = Camera.main.orthographicSize * 2;
        screenWidthWorld = screenHeightWorld * aspect;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, arenaRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, screenRadius);
    }

    public void Throw(float speed)
    {
        UpdateScreenDimensions();

        float angle = Random.Range(0, 2 * Mathf.PI);

        float x = Mathf.Cos(angle) * Mathf.Abs(screenHeightWorld / 2 / Mathf.Sin(angle));
        float y = Mathf.Sin(angle) * Mathf.Abs(screenWidthWorld / 2 / Mathf.Cos(angle));
        x = Mathf.Clamp(x, -screenWidthWorld / 2, screenWidthWorld / 2);
        y = Mathf.Clamp(y, -screenHeightWorld / 2, screenHeightWorld / 2);

        Vector2 spawnPos = new Vector2(x, y);
        transform.position = spawnPos;

        this.speed = speed;

        Vector2 perpendicularDir = Vector2.Perpendicular(spawnPos).normalized;
        Vector2 targetPos = perpendicularDir * arenaRadius * Random.Range(-1f, 1f);

        dir = (targetPos - spawnPos).normalized;
    }
}
