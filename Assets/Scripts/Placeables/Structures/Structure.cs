using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Structure : Placeable, IBoostableSpeed, IBoostableLuck
{
    [SerializeField] public short capacity = 1;

    [Header("Time")]
    [SerializeField] public float processingTime = 1;
    [SerializeField] protected float processingTimeMultiplayer = 1;
    protected float ProcessingTime => processingTime * processingTimeMultiplayer;
    protected float lastTime = 0;

    private bool waitNextFrame;
    protected bool CanProcess()
    {
        //if (waitNextFrame)
        //{ waitNextFrame = false; return true; }

        if (Time.time - ProcessingTime > lastTime)
        {
            lastTime = Time.time;
            //waitNextFrame = true;
            return true;
        }

        return false;
    }

    [Header("Boosted pallo generation")]
    [SerializeField] public float boostedPalloGenerationProbability;
    [SerializeField] protected float boostedPalloGenerationProbabilityMultiplayer;
    protected float BoostedPalloGenerationProbability => boostedPalloGenerationProbability * boostedPalloGenerationProbabilityMultiplayer;

    [Header("Dark pallo generation")]
    [SerializeField] public float darkPalloGenerationProbability;
    [SerializeField] protected float darkPalloGenerationProbabilityMultiplayer;
    protected float DarkPalloGenerationProbability => darkPalloGenerationProbability * darkPalloGenerationProbabilityMultiplayer;

    [SerializeField] protected List<Pallo> pallos;

    [SerializeField] protected Direction[] inputs;
    [SerializeField] protected Direction output;

    [Header("Particles")]
    [SerializeField] ParticleSystem processParticle;

    Coroutine moveRoutine;
    
    public virtual bool TryInsertPalloFrom(Direction previousStructureDirection, Pallo pallo)
    {
        //Debug.Log(previousStructureDirection + " - " + direction + "  " + gameObject.name);
        foreach (Direction input in inputs)
        {
            //Debug.Log(RotateDirectionBy(direction, input).ToString());
            if (previousStructureDirection == RotateDirectionBy(RotateDirectionBy(direction, (Direction)(2)), input)) { return AddPallo(pallo); }
        }
        return false;
    }
    protected bool AddPallo(Pallo pallo)
    {
        if (pallos.Count >= capacity)
            return false;
        pallo.Replace(this);
        pallos.Add(pallo);
        lastTime = Time.time;
        return true;
    }
    public bool RemovePallo(Pallo pallo)
    {
        if (pallos.Count == 0) return false;
        pallos.Remove(pallo);
        return true;
    }

    protected virtual void MovePalloToNext()
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine());
    }
    protected IEnumerator MoveRoutine()
    {
        bool done = false;
        while (true)
        {
            if (pallos.Count == 0) yield break;
            Pallo palloToMove = pallos[0];
            // research of the nearest structure in the direction
            Direction directionOut = RotateDirectionBy(direction, output);
            //Debug.Log(directionOut.ToString() + " " + output + " " + direction  + "   " + gameObject.name);
            Structure next;
            if (GetNext(out next, directionOut) && next.TryInsertPalloFrom(directionOut, palloToMove))
            {
                // can insert pallo
                ProcessPallo(palloToMove);
                pallos.RemoveAt(0);
                yield break;
            }
            // move pallo to next
            yield return new WaitForSeconds(0.1f);
            // exit if done
            if (done) yield break;
            done = true;
        }
    }

    protected virtual void ProcessPallo(Pallo pallo)
    {
        // do transition animation for pallo
        // do effects for pallo
        // do something when the pallo is ready
        if (processParticle) processParticle.Play();
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

        Placeable placeable;
        //bool thereIsTile = ;
        //bool isStructure = ;
        //Debug.Log("there is a tile : " + thereIsTile + "   is structure : " + isStructure + "   class name "  + placeable.GetType().ToString());
        if (GridManager.Instance.GetTileFromGridPosition(out placeable, referencingPosition) && placeable.IsStructure())
        { structure = (Structure)placeable; return true; }

        structure = null;
        return false;
    }

    public override void Select()
    {
        List<Pallo> tempPallos = new List<Pallo>();
        foreach (Pallo pallo in pallos) tempPallos.Add(pallo);
        foreach (Pallo pallo in tempPallos) pallo.HandCollect();
        base.Select();
    }
    protected override void ApplyRotation()
    {
        List<Pallo> tempPallos = new List<Pallo>();
        foreach (Pallo pallo in pallos) tempPallos.Add(pallo);
        foreach (Pallo pallo in tempPallos) pallo.HandCollect();
        base.ApplyRotation();
    }

    public override void Lock() {
        base.Lock();
        capacity = 0;
    }
}
