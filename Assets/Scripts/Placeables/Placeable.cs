using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable] public enum Direction { XPositive, YPositive, XNegative, YNegative }
public class Placeable : MonoBehaviour
{
    // monobehaviour for the tiles that have something on
    // cambiare nome in Placeable

    [SerializeField] protected Vector2Int position;
    [SerializeField] protected Direction direction;
    
    public virtual void Init(Vector2Int position, Direction direction)
    {
        // pleacable init
        this.position = position;
        this.direction = direction;
    }
    public void Move(Vector2Int newPosition)
    {
        if (GridManager.Instance.MoveTile(position, newPosition))
            position = newPosition;
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
}
