using UnityEngine;

public class FoodThrower : MonoBehaviour
{
    public GameObject food;
    public float startSpeed = 15;
    public float startThrowTimeInterval = 2;
    private float lastThrowTime = 0;
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

        if (Time.time >= lastThrowTime + throwTimeInterval)
        {
            ThrowFood();
            lastThrowTime = Time.time;
        }

        throwTimeInterval = -(Time.time - gameStartTime) / 100 + startThrowTimeInterval;
        throwTimeInterval = Mathf.Max(throwTimeInterval, 0.5f);
        speed = (Time.time - gameStartTime) / 100 + startSpeed;
        speed = Mathf.Min(speed, 15);
        //Debug.Log("Time: " + throwTimeInterval + " Speed: " + speed);
    }

    private void ThrowFood()
    {
        GameObject foodObj = Instantiate(food);
        Food newFood = foodObj.GetComponent<Food>();

        newFood.Throw(speed);
    }
}
