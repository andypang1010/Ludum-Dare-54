using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; set; }

    private float survivedDuration;
    private int bacteriaCount;
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
        bacteriaCount = 0;
        medicineCount = 0;
    }

    void Update()
    {
        survivedDuration += Time.deltaTime;
    }

    public int GetSurvivedDuration()
    {
        return (int)survivedDuration;
    }

    public void IncBacteriaCount()
    {
        bacteriaCount++;
    }

    public void IncMedicineCount()
    {
        medicineCount++;
    }

    public float GetBacteriaCount()
    {
        return bacteriaCount;
    }

    public float GetMedicineCount()
    {
        return medicineCount;
    }
}
