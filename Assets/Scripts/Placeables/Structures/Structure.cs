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

    [SerializeField] protected bool canInsertBack;
    [SerializeField] protected bool canInsertRight;
    [SerializeField] protected bool canInsertLeft;
    [SerializeField] protected bool canInsertFront;

    public Direction RotateDirectionRight(Direction direction)
    {
        if (((int)direction) != 0)
            direction = (Direction)((int)direction - 1);
        else
            direction = Direction.XPositive;

        return direction;
    }
    public Direction RotateDirectionLeft(Direction direction)
    {
        if (((int)direction) != 3)
            direction = (Direction)(((int)direction) + 1);
        else
            direction = Direction.YNegative;

        return direction;
    }

    public virtual bool CanInsertPalloFrom(Direction previousStructureDirection)
    {
        Direction tempDirection = direction;
        if (tempDirection == previousStructureDirection && canInsertBack) return true;
        tempDirection = RotateDirectionRight(tempDirection);
        if (tempDirection == previousStructureDirection && canInsertRight) return true;
        tempDirection = RotateDirectionRight(tempDirection);
        if (tempDirection == previousStructureDirection && canInsertFront) return true;
        tempDirection = RotateDirectionRight(tempDirection);
        if (tempDirection == previousStructureDirection && canInsertLeft) return true;

        return false;
    }
    public virtual bool AddPallo(Pallo pallo)
    {
        if (pallos.Count >= capacity)
            return false;

        pallos.Add(pallo);
        return true;
    }
    public virtual Pallo RemovePallo()
    {
        Pallo pallo = pallos[0];
        pallos.RemoveAt(0);
        return pallo;
    }

    public virtual void ProcessingPallo(Pallo pallo)
    {
        // calculate the time of the pallo
        // calculate the probability of the pallo
    }

    public void BoostLuck(float intensity)
    {
        processingTimeMultiplayer = intensity;
    }
    public void BoostSpeed(float intensity)
    {
        darkPalloGenerationProbabilityMultiplayer = intensity;
    }
}
