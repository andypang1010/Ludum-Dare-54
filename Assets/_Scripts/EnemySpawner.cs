using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float startSpawnTimeInterval = 2;
    private float lastSpawnTime = 0;
    private float spawnRadius = 10f;
    private float gameStartTime = 0;
    private float throwTimeInterval = 2;
    private float speed;

    private void Start()
    {
        gameStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= lastSpawnTime + throwTimeInterval)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }

        throwTimeInterval = -(Time.time - gameStartTime) / 100 + startSpawnTimeInterval;
        throwTimeInterval = Mathf.Max(throwTimeInterval, 0.5f);
        //Debug.Log("Time: " + throwTimeInterval + " Speed: " + speed);
    }

    private void SpawnEnemy()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        GameObject enemyObj = Instantiate(enemy, spawnPos, Quaternion.identity);
    }
}
