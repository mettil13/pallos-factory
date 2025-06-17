using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable] public class PlaceableAnimation
{
    [SerializeField] public Vector3 startSize;
    [SerializeField] public Vector3 startPosition;
    [SerializeField] public Vector3 endSize;
    [SerializeField] public Vector3 endPosition;
    [SerializeField] public float transitionDuration;
    [SerializeField] public AnimationCurve sizeTransitionCurve;
    [SerializeField] public AnimationCurve positionTransitionCurve;
    [SerializeField] public int maxTweens;
    [SerializeField] private Tween[] sizeActiveTweens;
    [SerializeField] private Tween[] positionActiveTweens;
    [SerializeField] private int currentTween;

    public void Init()
    {
        sizeActiveTweens = new Tween[maxTweens];
        positionActiveTweens= new Tween[maxTweens];
        //Debug.Log("tween init " + maxTweens + " size : " + sizeActiveTweens.Length);
    }
    public void DoTweens(Transform placeable, Transform subTransform)
    {
        currentTween++;
        if (currentTween >= maxTweens) { currentTween = 0; }
        //Debug.Log("size : " + sizeActiveTweens.Length);
        sizeActiveTweens[currentTween]?.Complete();
        positionActiveTweens[currentTween]?.Complete();
        //placeable.DOKill();
        //subTransform.DOKill();
        placeable.localScale = startSize;
        subTransform.localPosition = startPosition;
        sizeActiveTweens[currentTween] = placeable.DOScale(endSize, transitionDuration).SetEase(sizeTransitionCurve);
        positionActiveTweens[currentTween] = subTransform.DOLocalMove(endPosition, transitionDuration).SetEase(positionTransitionCurve);
    }
}

[CreateAssetMenu(fileName = "animation", menuName = "ScriptableObjects/PlaceableAnimations", order = 0)]
public class PlaceableAnimationsSO : ScriptableObject
{
    [Header("Select animation")]
    [SerializeField] bool doSelectAnimation;
    [SerializeField, ShowIf(nameof(doSelectAnimation))] PlaceableAnimation selectAnimation;
    [Header("Move animation")]
    [SerializeField] bool doMoveAnimation;
    [SerializeField, ShowIf(nameof(doMoveAnimation))] PlaceableAnimation moveAnimation;
    [SerializeField, ShowIf(nameof(doMoveAnimation))] float rotationMultiplier = 40;
    [SerializeField, ShowIf(nameof(doMoveAnimation))] float rotationToSpeedTransitionDuration = 0.2f;
    [SerializeField, ShowIf(nameof(doMoveAnimation))] float rotationTransitionDuration = 0.6f;
    private Transform lastRotatedTransform;
    private Tween rotationTween;
    private Tween rotationTweenSecond;
    [Header("Rotate animation")]
    [SerializeField] bool doRotateAnimation;
    [SerializeField, ShowIf(nameof(doRotateAnimation))] PlaceableAnimation rotateAnimation;
    [Header("Buy animation")]
    [SerializeField] bool doBuyAnimation;
    [SerializeField, ShowIf(nameof(doBuyAnimation))] PlaceableAnimation buyAnimation;
    [Header("Repair animation")]
    [SerializeField] bool doRepairAnimation;
    [SerializeField, ShowIf(nameof(doRepairAnimation))] PlaceableAnimation repairAnimation;
    [Header("Corrupt animation")]
    [SerializeField] bool doCorruptAnimation;
    [SerializeField, ShowIf(nameof(doCorruptAnimation))] PlaceableAnimation corruptAnimation;

    public void Init()
    {
        selectAnimation.Init();
        moveAnimation.Init();
        rotateAnimation.Init();
        buyAnimation.Init();
        buyAnimation.Init();
        repairAnimation.Init();
        corruptAnimation.Init();
    }
    public void DoSelectAnimation(Placeable placeable)
    {
        if (!doSelectAnimation) return;
        selectAnimation.DoTweens(placeable.transform, placeable.animationTransform);
    }
    public void DoMoveAnimation(Placeable placeable, Vector2Int previousPosition)
    {
        if (!doMoveAnimation) return;
        moveAnimation.DoTweens(placeable.transform, placeable.animationTransform);

        Vector2Int positionDifference = placeable.Position - previousPosition;
        positionDifference.Clamp(-Vector2Int.one, Vector2Int.one);
        if (lastRotatedTransform && lastRotatedTransform != placeable.transform)
        {
            lastRotatedTransform.DOComplete();
            lastRotatedTransform.transform.eulerAngles = Vector3.zero;
        }
        rotationTween?.Kill();
        rotationTweenSecond?.Kill();
        lastRotatedTransform = placeable.transform;
        Vector3 speedRotation = new Vector3(-positionDifference.y * rotationMultiplier, 0, positionDifference.x * rotationMultiplier);
        rotationTween = placeable.transform.DORotate(speedRotation, rotationToSpeedTransitionDuration, RotateMode.Fast).OnComplete(() =>
        {
            rotationTweenSecond = lastRotatedTransform.DORotate(Vector3.zero, rotationTransitionDuration, RotateMode.Fast);
        });
    }
    public void DoRotateAnimation(Placeable placeable)
    {
        if (!doRotateAnimation) return;
        rotateAnimation.DoTweens(placeable.transform, placeable.animationTransform);
    }
    public void DoBuyAnimation(Placeable placeable)
    {
        if (!doBuyAnimation) return;
        buyAnimation.DoTweens(placeable.transform, placeable.animationTransform);
    }
    public void DoRepairAnimation(Placeable placeable)
    {
        if (!doRepairAnimation) return;
        repairAnimation.DoTweens(placeable.transform, placeable.animationTransform);
    }
    public void DoCorrupAnimation(Placeable placeable)
    {
        if (!doCorruptAnimation) return;
        corruptAnimation.DoTweens(placeable.transform, placeable.animationTransform);
    }
}
