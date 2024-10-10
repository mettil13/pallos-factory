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
    [SerializeField] protected Direction[] outputs;

    public virtual bool CanInsertPalloFrom(Direction previousStructureDirection)
    {
        foreach (Direction input in inputs)
            if (previousStructureDirection == RotateDirectionBy(direction, previousStructureDirection)) return true;

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
