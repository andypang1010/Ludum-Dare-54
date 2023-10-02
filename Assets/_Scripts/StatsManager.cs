using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public static float maxSize = 60;   
    private float survivedDuration;
    private int virusCount;
    private int medicineCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        survivedDuration = 0f;
        virusCount = 0;
        medicineCount = 0;
    }

    void Update()
    {
        survivedDuration += Time.deltaTime;
    }

    public int GetSurvivedDuration()
    {
        // print(survivedDuration);
        return (int) survivedDuration;
    }

    public void IncVirusCount()
    {
        virusCount++;
    }

    public void IncMedicineCount()
    {
        medicineCount++;
    }

    public float GetVirusCount()
    {
        return virusCount;
    }

    public float GetMedicineCount()
    {
        return medicineCount;
    }
}
