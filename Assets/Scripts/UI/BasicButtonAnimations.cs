using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasicButtonAnimations : MonoBehaviour
{
    [SerializeField] private float timeToSqueezeHover;
    [SerializeField] private float scaleSqueezeHover;
    [SerializeField] private float timeToSqueezeClick;
    [SerializeField] private float scaleSqueezeClick;
    [SerializeField] private float timeToSqueezeRelease;
    [SerializeField] private float scaleSqueezeRelease;

    Tween scaleSqueezeTween;
    Ease scaleSqueezeEase;

    private void DoScaleSqueezeTween(float time, float scale)
    {
        if (scaleSqueezeTween != null) { scaleSqueezeTween.Kill(); }
        
    }
}
