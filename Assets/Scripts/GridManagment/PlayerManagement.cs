using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    [SerializeField] private uint currentPoints;
    [SerializeField] private uint[] lastPoints;
    
    private short selectedLastPoint = 0;
    private float timeToUpdate = 1;
    private float lastTime;



    public void Update() {
        if (Time.time - lastTime > timeToUpdate) {
            lastTime = Time.time;
            selectedLastPoint++;
            if (selectedLastPoint == lastPoints.Length) { selectedLastPoint = 0; }
        }



    }
    public void AddPalloPoints(uint points) 
    {
        currentPoints += points;
        lastPoints[selectedLastPoint] += points;
    }
    public uint GetCurrentPalloPoints() { return currentPoints; }
    public float GetPalloPointsPerSecond() {
        float sum = 0;
        foreach (float point in lastPoints) {
            sum += point;
        }
        float median = sum / lastPoints.Length;
        return median;
    }
}
