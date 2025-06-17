using UnityEngine;

public abstract class PalloGenericAnimation : MonoBehaviour
{
    [SerializeField] Transform animationTransform;

    public virtual void Create()
    {
        GridManager.Instance.AddPallo(this, gameObject);
    }
    public virtual void Remove()
    {
        GridManager.Instance.RemovePallo(gameObject);
        Destroy(gameObject);
    }
    public virtual void UpdateAnimation(Vector3 size)
    {
        animationTransform.localScale = size;
    }
}