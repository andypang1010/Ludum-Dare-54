using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edibles : MonoBehaviour
{
    public List<Sprite> virusSprites;
    public List<Sprite> foodSprites;
    public Vector2 arenaOffset;
    public float arenaRadius = 3f;
    public float destroyRadius = 7f;
    public float maxSpinSpeed = 1.0f;
    private int bounceCount = 3;
    private bool passedCenter = false;
    private float moveSpeed;
    private float spinSpeed;
    private Vector2 dir;
    private float screenHeightWorld;
    private float screenWidthWorld;

    // Start is called before the first frame update
    void Start()
    {
        if (tag == "Medicine")
        {
            int rand = Random.Range(0, virusSprites.Count);
            GetComponent<SpriteRenderer>().sprite = virusSprites[rand];
        }
        if (tag == "Virus")
        {
            int rand = Random.Range(0, foodSprites.Count);
            GetComponent<SpriteRenderer>().sprite = foodSprites[rand];
        }
        spinSpeed = Random.Range(-maxSpinSpeed, maxSpinSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = dir * moveSpeed * Time.deltaTime;
        transform.position = transform.position + moveVector;

        if (CheckPointIsInArena(transform.position))
            passedCenter = true;
        if (passedCenter && transform.position.magnitude > destroyRadius)
        {
            Destroy(gameObject);
        }

        transform.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger: " + collision.gameObject.name);
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
        Gizmos.DrawWireSphere(arenaOffset, arenaRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, destroyRadius);
    }

    public void Throw(float speed)
    {
        if (tag == "Medicine")
            speed /= 2;

        UpdateScreenDimensions();

        float angle = Random.Range(0, 2 * Mathf.PI);

        float x = Mathf.Cos(angle) * Mathf.Abs(screenHeightWorld / 2 / Mathf.Sin(angle));
        float y = Mathf.Sin(angle) * Mathf.Abs(screenWidthWorld / 2 / Mathf.Cos(angle));
        x = Mathf.Clamp(x, -screenWidthWorld / 2, screenWidthWorld / 2);
        y = Mathf.Clamp(y, -screenHeightWorld / 2, screenHeightWorld / 2);

        Vector2 spawnPos = new Vector2(x, y);
        transform.position = spawnPos;

        this.moveSpeed = speed;

        Vector2 perpendicularDir = Vector2.Perpendicular(spawnPos - arenaOffset).normalized;
        Vector2 targetPos = arenaOffset + perpendicularDir * arenaRadius * Random.Range(-1f, 1f);

        dir = (targetPos - spawnPos).normalized;
    }

    private bool CheckPointIsInArena(Vector2 pos)
    {
        return (pos - arenaOffset).magnitude <= arenaRadius;
    }
}
