using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private uint currentPoints;
    public uint CurrentPoints {
        get => currentPoints;
        private set {
            currentPoints = value;
            CurrentPointsChanged.Invoke(currentPoints);
        }
    }
    [SerializeField] private uint[] lastPoints;
    
    private short selectedLastPoint = 0;
    private float timeToUpdate = 1;
    private float lastTime;

    public UnityEvent<uint> CurrentPointsChanged = new UnityEvent<uint>();


    private void Awake()
    {
        instance = this;
    }
    public void Update() 
    {
        if (Time.time - lastTime > timeToUpdate) 
        {
            lastTime = Time.time;
            selectedLastPoint++;
            if (selectedLastPoint == lastPoints.Length) { selectedLastPoint = 0; }
            lastPoints[selectedLastPoint] = 0;
        }
    }
    public void AddPalloPoints(uint points) 
    {
        CurrentPoints += points;
        lastPoints[selectedLastPoint] += points;
    }
    public uint GetCurrentPalloPoints() { return CurrentPoints; }
    public float GetPalloPointsPerSecond() 
    {
        float sum = 0;
        foreach (float point in lastPoints) 
        {
            sum += point;
        }
        float median = sum / lastPoints.Length;
        return median;
    }
}
