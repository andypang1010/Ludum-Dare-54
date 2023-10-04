using UnityEngine;

public class EdiblesThrower : MonoBehaviour
{
    public GameObject virus,
        medicine;
    public float startSpeed = 15;
    public float startThrowTimeInterval = 2;
    public float medSpawnProbability = 0.05f;
    private float lastThrowTime = 0;
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
        if (Time.time > lastThrowTime + throwTimeInterval)
        {
            ThrowEdible();
            lastThrowTime = Time.time;
        }

        throwTimeInterval = -(Time.time - gameStartTime) / 100 + startThrowTimeInterval;
        throwTimeInterval = Mathf.Max(throwTimeInterval, 0.3f);
        speed = (Time.time - gameStartTime) / 20 + startSpeed;
        speed = Mathf.Min(speed, 20);
        Debug.Log("Time: " + throwTimeInterval + " Speed: " + speed);
    }

    private void ThrowEdible()
    {
        GameObject edibleItem =
            (Random.value <= medSpawnProbability) ? Instantiate(medicine) : Instantiate(virus);
        edibleItem.GetComponent<Edibles>().Throw(speed);
    }
}
