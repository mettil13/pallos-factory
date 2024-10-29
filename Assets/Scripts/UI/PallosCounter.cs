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
        UpdatePallosCounter((int)PlayerManager.instance.CurrentPoints);

        PlayerManager.instance.LastPointsChanged.AddListener(UpdatePallosPerSeconds);
        UpdatePallosPerSeconds(PlayerManager.instance.GetPalloPointsPerSecond());
    }

    private void UpdatePallosCounter(int pallos) {
        pallosCounter.text = pallos.ToString();
    }

    private void UpdatePallosPerSeconds(float pallosPerSeconds) {
        this.pallosPerSeconds.text = pallosPerSeconds.ToString("0.00") + " <sprite name=pallo\">/s";
    }

}
