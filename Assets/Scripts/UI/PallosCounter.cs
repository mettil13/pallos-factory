using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PallosCounter : MonoBehaviour
{
    public TextMeshProUGUI pallosCounter;
    public TextMeshProUGUI pallosPerSeconds;



    void Start()
    {
        PlayerManager.instance.CurrentPointsChanged.AddListener(UpdatePallosCounter);
        UpdatePallosCounter(PlayerManager.instance.CurrentPoints);
    }

    private void UpdatePallosCounter(uint pallos) {
        pallosCounter.text = pallos.ToString();
    }

}
