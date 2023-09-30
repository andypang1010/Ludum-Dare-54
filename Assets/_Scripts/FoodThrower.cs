using UnityEngine;

public class FoodThrower : MonoBehaviour
{
    public GameObject food;
    public float speed = 15;
    public float throwTimeInterval = 2;
    private float lastThrowTime = 0;
    private float spawnRadius = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastThrowTime + throwTimeInterval)
        {
            ThrowFood();
            lastThrowTime = Time.time;
        }
    }

    private void ThrowFood()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        GameObject foodObj = Instantiate(food, spawnPos, Quaternion.identity);
        Food newFood = foodObj.GetComponent<Food>();

        newFood.Throw(spawnPos, speed);
    }
}
