using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtilities : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
