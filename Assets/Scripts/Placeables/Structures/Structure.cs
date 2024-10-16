using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Structure : Placeable, IBoostableSpeed, IBoostableLuck
{
    [SerializeField] protected short capacity;
    [SerializeField] protected float processingTime;
    [SerializeField] protected float processingTimeMultiplayer;
    [SerializeField] protected float darkPalloGenerationProbability;
    [SerializeField] protected float darkPalloGenerationProbabilityMultiplayer;
    [SerializeField] protected List<Pallo> pallos;

    [SerializeField] protected Direction[] inputs;
    [SerializeField] protected Direction output;

    public virtual bool TryInsertPalloFrom(Direction previousStructureDirection, Pallo pallo)
    {
        if (pallos.Count >= capacity)
            return false;

        foreach (Direction input in inputs)
            if (previousStructureDirection == RotateDirectionBy(direction, previousStructureDirection)) { AddPallo(pallo); return true; }

        return false;
    }
    private void AddPallo(Pallo pallo)
    {
        pallos.Add(pallo);
    }

    protected virtual void MovePalloToNext(Pallo palloToMove)
    {
        // research of the nearest structure in the direction
        Direction directedOut = RotateDirectionBy(direction, output);
        Structure next;
        if (GetNext(out next, directedOut) && next.TryInsertPalloFrom(directedOut, palloToMove))
        {
            // can insert pallo
            ProcessPallo(palloToMove);
            pallos.RemoveAt(0);
        }
        // move pallo to next
    }
    protected virtual void ProcessPallo(Pallo pallo)
    {
        // do transition animation for pallo
        // do effects for pallo
        // do something when the pallo is ready
    }

    public void BoostLuck(float intensity)
    {
        processingTimeMultiplayer = intensity;
    }
    public void BoostSpeed(float intensity)
    {
        darkPalloGenerationProbabilityMultiplayer = intensity;
    }


    public bool GetNext(out Structure structure, Direction output)
    {
        Vector2Int referencingPosition = Vector2Int.zero;
        switch (output)
        {
            case Direction.XPositive: referencingPosition = position + Vector2Int.right;
                break;
            case Direction.YPositive: referencingPosition = position + Vector2Int.up;
                break;
            case Direction.XNegative: referencingPosition = position + Vector2Int.left;
                break;
            case Direction.YNegative: referencingPosition = position + Vector2Int.down;
                break;
        }

        Placeable Placeable;
        GridManager.Instance.GetTileFromGridPosition(out Placeable, referencingPosition);
        if (Placeable.GetType() == typeof(Structure)) { structure = (Structure)Placeable; return true; }

        structure = null;
        return false;
    }
}
