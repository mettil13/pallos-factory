using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ElementAnimator : MonoBehaviour
{
    [System.Serializable] public struct AnimationSizeTweenStruct
    {
        [SerializeField] public Vector3 newSize;
        [SerializeField] public Ease animationEase;
        [SerializeField] public float duration;
    }
    [System.Serializable] public class AnimationPeriod
    {
        [SerializeField] public AnimationType referencedType;
        [SerializeField] public AnimationSizeTweenStruct[] animations;
        private Tween animationTween;

        public IEnumerator AnimationUpdate()
        {
            Vector3 prevSize = Vector3.one;
            while (true)
            {
                foreach (AnimationSizeTweenStruct animation in animations)
                {
                    animationTween?.Kill();
                    animationTween = DOVirtual.Vector3(
                        prevSize,
                        animation.newSize,
                        animation.duration,
                        (vector) => UpdateAllAnimations(vector))
                        .SetEase(animation.animationEase);
                    prevSize = animation.newSize;
                    yield return new WaitForSeconds(animation.duration);
                }
            }
        }
        public void UpdateAllAnimations(Vector3 vector)
        {
            switch (referencedType)
            {
                case AnimationType.StructureGeneric: case AnimationType.Transporter:
                    List<Placeable> placeables = GridManager.Instance.PlaceablesPlaced;
                    foreach (Placeable placeable in placeables)
                        if (placeable.AnimationType == referencedType)
                            placeable.UpdateAnimation(vector);
                    break;
                case AnimationType.Pallo:
                    List<PalloGenericAnimation> pallos = GridManager.Instance.pallosAnimations;
                    foreach (PalloGenericAnimation pallo in pallos)
                        pallo.UpdateAnimation(vector);
                    break;
            }
        }
    }

    [SerializeField] private AnimationPeriod[] periods;
    
    private void Awake()
    {
        StartAnimationUpdate();
    }
    public void StartAnimationUpdate()
    {
        StopAllCoroutines();
        foreach (AnimationPeriod period in periods)
            StartCoroutine(period.AnimationUpdate());
    }
}