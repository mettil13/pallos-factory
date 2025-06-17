using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable] public enum Direction { XPositive, YPositive, XNegative, YNegative }
[System.Serializable] public enum AnimationType { StructureGeneric, Transporter, Pallo }
abstract public class Placeable : MonoBehaviour
{

    // monobehaviour for the tiles that have something on
    [SerializeField] protected Vector2Int position;
    public Vector2Int Position => position;

    [SerializeField] protected Direction direction;

    [SerializeField] public PlaceableSO placeableReferenced;

    [SerializeField] protected bool isCorrupted = false;

    [Header("Animation settings")]

    [SerializeField] MeshRenderer[] structureMeshes;
    [SerializeField] MeshRenderer[] palloMeshes;

    [SerializeField] protected AnimationType animationType;
    public AnimationType AnimationType => animationType;

    [SerializeField] public Transform animationTransform;

    public bool IsCorrupted {
        get => isCorrupted;
    }

    private void Start()
    {
        Init(GridManager.Instance.GetCellFromWorldPoint(transform.position), 0);
    }
    public virtual void Init(Vector2Int position, Direction direction)
    {
        // pleacable init
        this.position = position;
        //this.direction = direction;
        GridManager.Instance.AddTileToGridCache(position, this);
        placeableReferenced.SetPlaceableInfo(this);
        //ApplyRotation();
        ApplyRotationAtStartup();
        foreach (MeshRenderer mesh in structureMeshes)
            mesh.material = GridManager.Instance.palloSettings.structureMaterialDefault;
        foreach (MeshRenderer mesh in palloMeshes)
            mesh.material = GridManager.Instance.palloSettings.palloMaterialDefault;
    }

    // update settings
    public void UpdatePlaceableGeneric()
    {
        if (isCorrupted) return;
        UpdatePlaceable();
    }
    public virtual void UpdatePlaceable() { }
    public virtual void UpdateAnimation(Vector3 size)
    {
        animationTransform.localScale = size;
    }

    public void MoveToClosestCellRelativeToWorld(Vector3 worldPosition) {
        Vector2Int cellPos = GridManager.Instance.GetCellFromWorldPoint(worldPosition);
        Move(cellPos, false);
    }
    public void Move(Vector2Int gridPosition, bool instantly = false)
    {
        Vector2Int oldPosition = position;
        if (GridManager.Instance.MoveTileInGridCache(position, gridPosition)) {
            if (isCorrupted) { Repair(); return; }
            position = gridPosition;
            //transform.position = ;
            Select();
            if (instantly)
            {
                transform.DOKill();
                transform.position = GridManager.Instance.GetCellCenter(gridPosition);
            }
            else
            {
                transform.DOComplete();
                transform.DOMove(GridManager.Instance.GetCellCenter(gridPosition), 0.25f);
            }
            GridManager.Instance.placeableAnimations.DoMoveAnimation(this, oldPosition);
        }
    }

    protected Direction RotateDirectionRight(Direction direction)
    {
        if (((int)direction) > 0)
            direction = (Direction)((int)direction - 1);
        else
            direction = Direction.YNegative;

        return direction;
    }
    protected Direction RotateDirectionLeft(Direction direction)
    {
        if (((int)direction) < 3)
            direction = (Direction)(((int)direction) + 1);
        else
            direction = Direction.XPositive;

        return direction;
    }
    protected Direction RotateDirectionBy(Direction direction, Direction rotateBy)
    {
        int finalDirection = ((int)direction) + ((int)rotateBy);
        if (finalDirection > 3) finalDirection -= 4;
        return (Direction)(finalDirection);
    }

    public void RotateRight()
    {
        direction = RotateDirectionRight(direction);
        GridManager.Instance.placeableAnimations.DoRotateAnimation(this);

        // inserire rotazione della mesh
        ApplyRotation();
    }
    public void RotateLeft()
    {
        direction = RotateDirectionLeft(direction);

        // inserire rotazione della mesh
        ApplyRotation();
    }

    Tween rotationTween;
    protected virtual void ApplyRotationAtStartup()
    {
        animationTransform.eulerAngles = -Vector2.up * ((int)direction) * 90;
    }
    protected virtual void ApplyRotation()
    {
        // inserire rotazione della mesh
        if (isCorrupted) { Repair(); return; }
        rotationTween?.Kill();
        animationTransform.DORotate(-Vector2.up * ((int)direction) * 90, 0.1f, RotateMode.Fast);
    }

    public virtual void Lock() {

    }
    public virtual void Unlock() {
        //Repair();
    }

    public virtual void Corrupt()
    {
        isCorrupted = true;
        foreach (MeshRenderer mesh in structureMeshes)
            mesh.material = GridManager.Instance.palloSettings.structureMaterialCorrupted;
        foreach (MeshRenderer mesh in palloMeshes)
            mesh.material = GridManager.Instance.palloSettings.palloMaterialCorrupted;
        ParticleAndSoundManager.instance.CorruptStructure(position);
        GridManager.Instance.placeableAnimations.DoCorrupAnimation(this);
        TryToDeselectFromInside();
        //GridManager.Instance.SelectedTile = null;
        //Deselect();
    }
    public virtual void Repair()
    {
        if (!isCorrupted) return;
        TryToDeselectFromInside();
        //GridManager.Instance.SelectedTile = null;
        //Deselect();
        isCorrupted = false;
        ParticleAndSoundManager.instance.RepairStructure(position);
        GridManager.Instance.placeableAnimations.DoRepairAnimation(this);
        Init(position, direction);
    }

    public virtual void Select() {
        if (isCorrupted) { Repair(); return; }
        //Debug.Log(gameObject.name + " selected");
        SelectGizmo.instance.SetGizmoOnGO(gameObject);
        GridManager.Instance.placeableAnimations.DoSelectAnimation(this);
    }
    public void Deselect() {
        //Debug.Log(gameObject.name + " deselected");
        SelectGizmo.instance.DeactivateGizmo(gameObject);
    }
    private void TryToDeselectFromInside()
    {
        if (GridManager.Instance.SelectedTile == this) GridManager.Instance.SelectedTile = null;
    }

    private void OnDestroy() {
        GridManager.Instance.RemoveTileFromGridCache(position);
    }

    public bool IsStructure()
    {
        return this.GetType().IsSubclassOf(typeof(Structure));
    }
    public bool IsBooster()
    {
        return this.GetType().IsSubclassOf(typeof(Booster));
    }
    public bool IsTurret()
    {
        return this.GetType().IsSubclassOf(typeof(Turret)); 
    }
}