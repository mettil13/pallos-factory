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
            CurrentPointsChanged.Invoke((int)currentPoints);
        }
    }

    [SerializeField] private uint[] lastPoints;
    
    private short selectedLastPoint = 0;
    private float timeToUpdate = 1;
    private float lastTime;
    private float globalBadLuck;
    public float globalLuck { get => globalBadLuck; set => globalBadLuck = value; }

    public UnityEvent<int> CurrentPointsChanged = new UnityEvent<int>();
    public UnityEvent<float> LastPointsChanged = new UnityEvent<float>();


    private void Awake()
    {
        instance = this;
        globalLuck = 0;
    }
    public void Update() 
    {
        if (Time.time - lastTime > timeToUpdate) 
        {
            lastTime = Time.time;
            selectedLastPoint++;
            if (selectedLastPoint == lastPoints.Length) { selectedLastPoint = 0; }
            lastPoints[selectedLastPoint] = 0;
            LastPointsChanged.Invoke(GetPalloPointsPerSecond());
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
