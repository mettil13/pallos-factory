using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable] public enum Direction { XPositive, YPositive, XNegative, YNegative }
public class Placeable : MonoBehaviour
{
    // monobehaviour for the tiles that have something on

    [SerializeField] protected Vector2Int position;
    [SerializeField] protected Direction direction;

    private void Start() {
        Init(GridManager.Instance.GetCellFromWorldPoint(transform.position), 0);
    }

    public virtual void Init(Vector2Int position, Direction direction)
    {
        // pleacable init
        this.position = position;
        this.direction = direction;
        GridManager.Instance.AddTileToGridCache(position, this);
    }

    public void MoveToClosestCellRelativeToWorld(Vector3 worldPosition) {
        Vector2Int cellPos = GridManager.Instance.GetCellFromWorldPoint(worldPosition);
        Move(cellPos);
    }
    public void Move(Vector2Int gridPosition)
    {
        if (GridManager.Instance.MoveTileInGridCache(position, gridPosition)) {
            position = gridPosition;
            Debug.Log(gridPosition + " " + GridManager.Instance.GetCellCenter(gridPosition));
            transform.position = GridManager.Instance.GetCellCenter(gridPosition);
        }
        Debug.Log(gridPosition);
    }
    public void RotateRight()
    {
        if (((int)direction) != 0)
            direction = (Direction)((int)direction - 1);
        else
            direction = Direction.XPositive;

        // inserire rotazione della mesh
        ApplyRotation();
    }
    public void RotateLeft()
    {
        if (((int)direction) != 3)
            direction = (Direction)(((int)direction) + 1);
        else
            direction = Direction.YNegative;

        // inserire rotazione della mesh
        ApplyRotation();
    }
    private void ApplyRotation()
    {
        // inserire rotazione della mesh
        transform.eulerAngles = Vector2.up * ((int)direction) * 90;
    }

    public virtual void Corrupt()
    {
        // corrupt the structure
    }
    public virtual void Repair()
    {
        // repair the structure
    }

    public void Select() {
        Debug.Log(gameObject.name + " selected");
    }

    public void Deselect() {
        Debug.Log(gameObject.name + " deselected");
    }

    private void OnDestroy() {
        GridManager.Instance.RemoveTileFromGridCache(position);
    }
}
