using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasicButtonAnimations : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] private float timeToSqueezeHover;
    [SerializeField] private float scaleSqueezeHover;
    [SerializeField] private float timeToSqueezeClick;
    [SerializeField] private float scaleSqueezeClick;
    [SerializeField] private float timeToSqueezeRelease;
    [SerializeField] private float scaleSqueezeRelease;

    float baseScale;
    Tween scaleSqueezeTween;
    [SerializeField] Ease scaleSqueezeEase;

    private void Awake()
    {
        baseScale = rectTransform.localScale.x;
    }

    private void DoScaleSqueezeTween(float time, float scale)
    {
        if (scaleSqueezeTween != null) { scaleSqueezeTween.Kill(); }
        scaleSqueezeTween = rectTransform.DOScale(scale * baseScale, time);
        scaleSqueezeTween.SetEase(scaleSqueezeEase);
        scaleSqueezeTween.SetUpdate(true);
    }

    public void ClickSqueeze()
    {
        DoScaleSqueezeTween(timeToSqueezeClick, scaleSqueezeClick);
    }
    public void ReleaseSqueeze()
    {
        DoScaleSqueezeTween(timeToSqueezeRelease, scaleSqueezeRelease);
    }
    public void HoverSqueeze()
    {
        DoScaleSqueezeTween(timeToSqueezeHover, scaleSqueezeHover);
    }
}
