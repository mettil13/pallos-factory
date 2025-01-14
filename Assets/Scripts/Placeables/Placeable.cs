using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable] public enum Direction { XPositive, YPositive, XNegative, YNegative }
public class Placeable : MonoBehaviour
{
    // monobehaviour for the tiles that have something on
    [SerializeField] protected Vector2Int position;
    public Vector2Int Position => position;
    [SerializeField] protected Direction direction;

    [SerializeField] public PlaceableSO placeableReferenced;

    [SerializeField] protected bool isCorrupted = false;

    [SerializeField] MeshRenderer[] meshes;
    
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
        string name = gameObject.name;
        placeableReferenced.SetPlaceableInfo(this);
        ApplyRotation();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = GridManager.Instance.palloSettings.structureMaterialDefault;
        }
    }

    public void MoveToClosestCellRelativeToWorld(Vector3 worldPosition) {
        Vector2Int cellPos = GridManager.Instance.GetCellFromWorldPoint(worldPosition);
        Move(cellPos);
    }
    public void Move(Vector2Int gridPosition)
    {
        if (GridManager.Instance.MoveTileInGridCache(position, gridPosition)) {
            position = gridPosition;
            transform.position = GridManager.Instance.GetCellCenter(gridPosition);
            Select();
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

        // inserire rotazione della mesh
        ApplyRotation();
    }
    public void RotateLeft()
    {
        direction = RotateDirectionLeft(direction);

        // inserire rotazione della mesh
        ApplyRotation();
    }
    protected virtual void ApplyRotation()
    {
        // inserire rotazione della mesh
        //transform.eulerAngles = -Vector2.up * ((int)direction) * 90;
        transform.DOKill();
        transform.DORotate(-Vector2.up * ((int)direction) * 90, 0.1f, RotateMode.Fast);
    }

    public virtual void Lock() {

    }
    public virtual void Unlock() {
        Repair();
    }
    public virtual void Corrupt()
    {
        isCorrupted = true;
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = GridManager.Instance.palloSettings.structureMaterialCorrupted;
        }
        ParticleAndSoundManager.instance.CorruptStructure(position);
    }
    public virtual void Repair()
    {
        isCorrupted = false;
        ParticleAndSoundManager.instance.RepairStructure(position);
        Init(position, direction);
    }

    public virtual void Select() {
        //Debug.Log(gameObject.name + " selected");
        SelectGizmo.instance.SetGizmoOnGO(gameObject);
        Repair();
    }
    public void Deselect() {
        //Debug.Log(gameObject.name + " deselected");
        SelectGizmo.instance.DeactivateGizmo();
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
